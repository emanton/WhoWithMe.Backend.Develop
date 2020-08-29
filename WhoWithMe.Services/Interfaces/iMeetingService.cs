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
using WhoWithMe.DTO.Model.User;

namespace WhoWithMe.Services.Interfaces
{
    public interface IMeetingService
    {
        Task<List<Meeting>> GetMeetingsByTypeAndTitleAndSortType(MeetingSearchDTO meetingSearchDTO);
        Task<List<Meeting>> GetMeetingsByOwner(PaginationUserId paginationUserId);
        Task<MeetingView> GetMeeting(CurrentUserIdMeetingId meetingId);
        Task<int> AddMeeting(MeetingDTO meeting);
        Task<int> EditMeeting(Meeting meeting);
        Task<int> DeleteMeeting(CurrentUserIdMeetingId meeting);
    }
}
