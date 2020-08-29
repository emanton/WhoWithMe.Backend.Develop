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
using WhoWithMe.DTO.Model.User;

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
            return await _meetingRepository.GetAllAsync(meetingSearchDTO.Count, meetingSearchDTO.Offset, 
                x => meetingSearchDTO.MeetingTypeId == null ? true : meetingSearchDTO.MeetingTypeId == x.MeetingType.Id);
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
            return meetingView;
        }

        public async Task<int> AddMeeting(MeetingDTO meeting)
        {
            _meetingRepository.Insert(meeting.GetMeeting());
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> EditMeeting(Meeting meeting)
        {
            _meetingRepository.Update(meeting);
            return await _unitOfWork.SaveChangesAsync();
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
            _meetingRepository.Delete(meeting);
			foreach (MeetingSubscriber subscriber in subscribers)
			{
                _meetingSubscriberRepository.Delete(subscriber);
            }
            foreach (MeetingImage image in meetingImages)
            {
                _meetingImageRepository.Delete(image);
                // delete in s3 TODO REDO!! anton
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
