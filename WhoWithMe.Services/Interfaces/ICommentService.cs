using Core.Data;
using Core.Data.Repositories;
using WhoWithMe.Core.Entities;
using WhoWithMe.DTO.Model.Meeting;
using WhoWithMe.DTO.UserDTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace WhoWithMe.Services.Interfaces
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
