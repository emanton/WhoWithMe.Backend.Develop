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
using WhoWithMe.DTO.Meeting;
using WhoWithMe.DTO.UserDTOs;

namespace WhoWithMe.Services.Interfaces
{
    public interface IMeetingSubscriberService
    {
        Task<int> UpdateMeetingSubscriberAcceptStatus(MeetingSubscriberDTO meetingSubscriber);
        Task<List<MeetingSubscriber>> GetMeetingAcceptedSubscribers(PaginationMeetingId paginationMeetingId);
        Task<List<MeetingSubscriber>> GetMeetingNotAcceptedSubscribers(PaginationMeetingId paginationMeetingId);
        Task<int> DeleteMeetingSubscriber(UserMeetingId participantMeetingId);
        Task<int> AddMeetingSubscriber(MeetingSubscriberDTO meetingSubscriber);
    }
}
