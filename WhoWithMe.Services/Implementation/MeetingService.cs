using Core.Data;
using Core.Data.Repositories;
using WhoWithMe.Core.Entities;
using WhoWithMe.Core.Entities.dictionaries;
using WhoWithMe.DTO.Model.Meeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhoWithMe.Core.Data;
using WhoWithMe.DTO.Meeting;
using WhoWithMe.DTO;
using WhoWithMe.DTO.Model;
using WhoWithMe.Services.Exceptions;
using WhoWithMe.Services.Interfaces;
using WhoWithMe.Services.Helpers;
using Microsoft.AspNetCore.Http;
using WhoWithMe.DTO.UserDTOs;

namespace WhoWithMe.Services.Implementation
{
    public class MeetingService : IMeetingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<MeetingType> _meetingTypeRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<Meeting> _meetingRepository;
        private readonly IRepository<MeetingImage> _meetingImageRepository;
        private readonly IRepository<MeetingSubscriber> _meetingSubscriberRepository;
        private readonly IRepository<CommentMeeting> _commentMeetingRepository;
        private readonly IMeetingImageService _meetingImageService;

        public MeetingService(IUnitOfWork unitOfWork, IMeetingImageService meetingImageService)
        {
            _unitOfWork = unitOfWork;
            _meetingTypeRepository = unitOfWork.GetRepository<MeetingType>();
            _meetingRepository = unitOfWork.GetRepository<Meeting>();
            _meetingImageRepository = unitOfWork.GetRepository<MeetingImage>();
            _meetingSubscriberRepository = unitOfWork.GetRepository<MeetingSubscriber>();
            _commentMeetingRepository = unitOfWork.GetRepository<CommentMeeting>();
            _cityRepository = unitOfWork.GetRepository<City>();
            _meetingImageService = meetingImageService;
        }

        // add sortType TODO
        public async Task<List<Meeting>> GetMeetingsByTypeAndTitleAndSortType(MeetingSearchDTO meetingSearchDTO)
        {
            if (meetingSearchDTO.MeetingTypeIds == null || meetingSearchDTO.MeetingTypeIds.Count == 0)
            {
                return await _meetingRepository.GetAllAsync(meetingSearchDTO.Count, meetingSearchDTO.Offset);
            }

            return await _meetingRepository.GetAllAsync(meetingSearchDTO.Count, meetingSearchDTO.Offset,
                x => meetingSearchDTO.MeetingTypeIds.Contains(x.MeetingType.Id));
        }

        public async Task<List<Meeting>> GetMeetingsByOwner(PaginationUserId paginationUserId)
        {
            return await _meetingRepository.GetAllAsync(paginationUserId.Count, paginationUserId.Offset, x => x.Creator.Id == paginationUserId.UserId);
        }

        public async Task<MeetingView> GetMeeting(CurrentUserIdMeetingId cumeeting)
        {
            long meetingId = cumeeting.MeetingId;
            Meeting meeting = await _meetingRepository.GetSingleAsync(meetingId);
            if (meeting == null)
            {
                throw new BadRequestException("Meeting not found");
            }

            MeetingView meetingView = new MeetingView(meeting);
            meetingView.ParticipantsCount = await GetMeetingParticipantsCount(meetingId);
            meetingView.SubscribersCount = await GetMeetingSubscribersCount(meetingId);
            meetingView.CommentsCount = await GetMeetingCommentsCount(meetingId);
            // PaginationMeetingId.MeetingId is int, cast from long
            meetingView.MeetingImageUrls = (await _meetingImageService.GetMeetingImages(new PaginationMeetingId { MeetingId = (int)meetingId, Count = 6, Offset = 0 })).Select(x => x.ImageUrl).ToList();
            return meetingView;
        }

        public async Task<int> AddMeeting(MeetingCreateDTO meetingDTO, long currentUserId)
        {
            Meeting meeting = meetingDTO.GetMeeting();
            _meetingRepository.Insert(meeting);
            int res = await _unitOfWork.SaveChangesAsync();
            await _meetingImageService.CreateMeetingImages(meeting.Id, meetingDTO.MeetingImages);
            return res;
        }

        public async Task<int> EditMeeting(MeetingEditDTO meetingDTO, long currentUserId)
        {
            Meeting meeting = meetingDTO.GetMeeting();
            _meetingRepository.Update(meeting);
            int res = await _unitOfWork.SaveChangesAsync();
            await _meetingImageService.DeleteMeetingImages(meetingDTO.RemovedImageIds);
            await _meetingImageService.CreateMeetingImages(meetingDTO.Id, meetingDTO.MeetingImages);
            return res;
        }

        public async Task<int> DeleteMeeting(CurrentUserIdMeetingId meetingId)
        {
            Meeting meeting = await _meetingRepository.GetSingleAsync(x => x.Id == meetingId.MeetingId);
            if (meeting == null)
            {
                throw new BadRequestException("meeting not found");
            }

            var subscribers = await _unitOfWork.GetRepository<MeetingSubscriber>().FindByAsync(x => x.MeetingId == meetingId.MeetingId);
            var meetingImages = await _unitOfWork.GetRepository<MeetingImage>().FindByAsync(x => x.MeetingId == meetingId.MeetingId);
            var meetingImageIds = meetingImages.Select(x => x.Id).ToList();

            await _meetingImageService.DeleteMeetingImages(meetingImageIds);
            foreach (var sub in subscribers) _unitOfWork.GetRepository<MeetingSubscriber>().Delete(sub);

            _meetingRepository.Delete(meeting);
            return await _unitOfWork.SaveChangesAsync();
        }

        private async Task<int> GetMeetingParticipantsCount(long meetingId)
        {
            return await _meetingSubscriberRepository.GetCount(x => x.MeetingId == meetingId && x.IsAccepted);
        }

        private async Task<int> GetMeetingSubscribersCount(long meetingId)
        {
            return await _meetingSubscriberRepository.GetCount(x => x.MeetingId == meetingId && !x.IsAccepted);
        }

        private async Task<int> GetMeetingCommentsCount(long meetingId)
        {
            return await _commentMeetingRepository.GetCount(x => x.Meeting.Id == meetingId);
        }
    }
}
