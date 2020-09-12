using Core.Data;
using Core.Data.Repositories;
using WhoWithMe.Core.Entities;
using WhoWithMe.Core.Entities.dictionaries;
using WhoWithMe.DTO.Model.Meeting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoWithMe.Core.Data;
using WhoWithMe.DTO.Meeting;
using System.Diagnostics.Eventing.Reader;
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


        public MeetingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _meetingTypeRepository = unitOfWork.GetRepository<MeetingType>();
            _meetingRepository = unitOfWork.GetRepository<Meeting>();
            _meetingImageRepository = unitOfWork.GetRepository<MeetingImage>();
            _meetingSubscriberRepository = unitOfWork.GetRepository<MeetingSubscriber>();
            _commentMeetingRepository = unitOfWork.GetRepository<CommentMeeting>();
            _cityRepository = unitOfWork.GetRepository<City>();
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

			//return await _meetingRepository.GetAllAsync(meetingSearchDTO.Count, meetingSearchDTO.Offset,
			//    new System.Linq.Expressions.Expression<Func<Meeting, bool>>(x =>
			//    {
			//        var someLocalVar = meetingSearchDTO.MeetingTypeIds.Contains(x.MeetingType.Id);

			//        return true;
			//    }));
			;
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
            meetingView.MeetingImageUrls = (await _meetingImageRepository.GetAllAsync(6,0, x => x.MeetingId == meetingId)).Select(x=>x.ImageUrl).ToList();
            return meetingView;
        }

        // -----------------------------added
        public async Task<List<MeetingImage>> GetMeetingImages(PaginationMeetingId pagMet)
        {
            return await _meetingImageRepository.GetAllAsync(pagMet.Count, pagMet.Offset, x => x.MeetingId == pagMet.MeetingId);
        }

        public async Task<int> AddMeeting(MeetingCreateDTO meetingDTO, long currentUserId)
        {
            Meeting meeting = meetingDTO.GetMeeting();
            _meetingRepository.Insert(meeting);
            int res = await _unitOfWork.SaveChangesAsync();
            await CreateMeetingImages(meeting.Id, meetingDTO.MeetingImages);

            return res;
        }

        public async Task<int> EditMeeting(MeetingEditDTO meetingDTO, long currentUserId)
        {
            Meeting meeting = meetingDTO.GetMeeting();
            _meetingRepository.Update(meeting);
            int res = await _unitOfWork.SaveChangesAsync();
            await DeleteMeetingImages(meetingDTO.RemovedImageIds);
            await CreateMeetingImages(meetingDTO.Id, meetingDTO.MeetingImages);
            return res;
        }

        public async Task<int> DeleteMeeting(CurrentUserIdMeetingId meetingId)
        {
            Meeting meeting = await _meetingRepository.GetSingleAsync(x => x.Id == meetingId.MeetingId);
            if (meeting == null)
            {
                throw new BadRequestException("meeting not found");
            }

			List<MeetingSubscriber> subscribers = await _meetingSubscriberRepository.FindByAsync(x => x.MeetingId == meetingId.MeetingId);
			List<MeetingImage> meetingImages = await _meetingImageRepository.FindByAsync(x => x.MeetingId == meetingId.MeetingId);
            List<long> meetingImageIds = meetingImages.Select(x => x.Id).ToList();
            await DeleteMeetingImages(meetingImageIds);
			foreach (MeetingSubscriber subscriber in subscribers)
			{
                _meetingSubscriberRepository.Delete(subscriber);
            }

            _meetingRepository.Delete(meeting);
            return await _unitOfWork.SaveChangesAsync();
        }

        private async Task<int> CreateMeetingImages(long meetingId, IEnumerable<IFormFile> meetingImages)
        {
            if (meetingImages == null)
            {
                return 0;
            }

            foreach (IFormFile formFile in meetingImages)
            {
                string imageUrl = await S3StorageService.UploadMeetingFile(meetingId, formFile);
                MeetingImage meetingImage = new MeetingImage
                {
                    ImageUrl = imageUrl,
                    MeetingId = meetingId
                };

                _meetingImageRepository.Insert(meetingImage);
            }

            return await _unitOfWork.SaveChangesAsync();
        }

        private async Task<int> DeleteMeetingImages(List<long> meetingImageIds)
        {
            if (meetingImageIds == null)
            {
                return 0;
            }

            foreach (long imageId in meetingImageIds)
            {
                MeetingImage meetingImage = await _meetingImageRepository.GetSingleAsync(x => x.Id == imageId);
                _meetingImageRepository.Delete(meetingImage);
                // delete in s3!!!!!
            }

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
