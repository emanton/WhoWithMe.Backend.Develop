using Core.Data;
using Core.Data.Repositories;
using Core.Entities;
using Services.Model.Meeting;
using Services.Model.User;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICommentService
    {

        Task<List<CommentUser>> GetUserComments(PaginationUserId paginationUserId);
        Task<int> AddUserComments(CommentUser commentUser);
        Task<int> DeleteUserComments(CommentUser commentUser);
        Task<List<CommentMeeting>> GetMeetingComments(PaginationMeetingId paginationMeetingId);
        Task<int> AddMeetingComments(CommentMeeting commentMeeting);
        Task<int> DeleteMeetingComments(CommentMeeting commentMeeting);
    }
}
