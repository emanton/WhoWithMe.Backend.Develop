using Core.Data;
using Core.Data.Repositories;
using Core.Entities;
using Core.Entities.dictionaries;
using Services.Model.Meeting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IMeetingService
    {
        Task<List<MeetingType>> GetMeetingTypes();

        Task<List<Meeting>> GetMeetingsByTypeAndTitleAndSortType(int count, int offset, MeetingType meetingType, MeetingSortType meetingSortType);
        Task<List<Meeting>> GetMeetingsByTypeAndTitleAndSortType(int count, int offset, int userId);

        Task<List<ParticipantMeeting>> GetMeetingParticipants(int count, int offset, int meetingId);

        Task<List<MeetingSubscriber>> GetMeetingSubscribers(int count, int offset, int meetingId);

        Task<int> DeleteMeetingParticipant(UserMeetingId participantMeetingId);
        Task<int> DeleteMeetingSubscriber(UserMeetingId participantMeetingId);
        Task<Meeting> GetMeeting(int meetingId);
        Task<int> AddMeeting(Meeting meeting);
        Task<int> EditMeeting(Meeting meeting);
        Task<int> DeleteMeeting(Meeting meeting);
    }
}
