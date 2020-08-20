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

namespace WhoWithMe.Services.Implementation
{
    public class MeetingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<MeetingType> _meetingTypeRepository;
        private readonly IRepository<Meeting> _meetingRepository;
        private readonly IRepository<ParticipantMeeting> _participantMeetingRepository;
        private readonly IRepository<MeetingSubscriber> _meetingSubscriberRepository;


        public MeetingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _meetingTypeRepository = unitOfWork.GetRepository<MeetingType>();
            _meetingRepository = unitOfWork.GetRepository<Meeting>();
            _participantMeetingRepository = unitOfWork.GetRepository<ParticipantMeeting>();
            _meetingSubscriberRepository = unitOfWork.GetRepository<MeetingSubscriber>();
        }

        public async Task<List<MeetingType>> GetMeetingTypes()
        {
            return await _meetingTypeRepository.GetAllAsync();
        }

        public async Task<List<Meeting>> GetMeetingsByTypeAndTitleAndSortType(int count, int offset, MeetingType meetingType, MeetingSortType meetingSortType)
        {
            return await _meetingRepository.GetAllAsync(count, offset, x => x.MeetingType.Name == meetingType.Name); // TODO
        }

        public async Task<List<Meeting>> GetMeetingsByTypeAndTitleAndSortType(int count, int offset, int userId)
        {
            return await _meetingRepository.GetAllAsync(count, offset, x => x.Creator.Id == userId);
        }

        public async Task<List<ParticipantMeeting>> GetMeetingParticipants(int count, int offset, int meetingId)
        {
            return await _participantMeetingRepository.GetAllAsync(count, offset, x => x.Meeting.Id == meetingId);
        }

        public async Task<List<MeetingSubscriber>> GetMeetingSubscribers(int count, int offset, int meetingId)
        {
            return await _meetingSubscriberRepository.GetAllAsync(count, offset, x => x.Meeting.Id == meetingId);
        }

        public async Task<int> DeleteMeetingParticipant(UserMeetingId participantMeetingId)
        {
			List<ParticipantMeeting> res = await _participantMeetingRepository.GetAllAsync(x => x.User.Id == participantMeetingId.UserId && x.Meeting.Id == participantMeetingId.MeetingId);
            if(res.Count > 1)
			{
                throw new Exception("Many res are found");
			}
            if (res.Count == 0)
            {
                throw new Exception("Result not found");
            }

            _participantMeetingRepository.Delete(res.First());
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> DeleteMeetingSubscriber(UserMeetingId participantMeetingId)
        {
			List<MeetingSubscriber> res = await _meetingSubscriberRepository.GetAllAsync(x => x.User.Id == participantMeetingId.UserId && x.Meeting.Id == participantMeetingId.MeetingId);
            if (res.Count > 1)
            {
                throw new Exception("Many res are found");
            }
            if (res.Count == 0)
            {
                throw new Exception("Result not found");
            }

            _meetingSubscriberRepository.Delete(res.First());
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Meeting> GetMeeting(int meetingId)
        {
            return await _meetingRepository.GetSingleAsync(meetingId);
        }

        public async Task<int> AddMeeting(Meeting meeting)
        {
            _meetingRepository.Insert(meeting);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> EditMeeting(Meeting meeting)
        {
            _meetingRepository.Update(meeting);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> DeleteMeeting(Meeting meeting)
        {
            _meetingRepository.Delete(meeting);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
