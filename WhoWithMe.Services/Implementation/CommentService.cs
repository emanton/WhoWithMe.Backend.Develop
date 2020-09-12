using Core.Data;
using Core.Data.Repositories;
using WhoWithMe.Core.Entities;
using WhoWithMe.Services.Interfaces;
using WhoWithMe.DTO.Model.Meeting;
using WhoWithMe.DTO.UserDTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using WhoWithMe.Core.Data;

namespace WhoWithMe.Services.Implementation
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<CommentUser> _commentUserRepository;
        private readonly IRepository<CommentMeeting> _commentMeetingRepository;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _commentUserRepository = unitOfWork.GetRepository<CommentUser>();
            _commentMeetingRepository = unitOfWork.GetRepository<CommentMeeting>();
        }
        // user
        public async Task<List<CommentUser>> GetUserComments(PaginationUserId paginationUserId)
		{
            return await _commentUserRepository.GetAllAsync(paginationUserId.Count, paginationUserId.Offset, x => x.UserToId == paginationUserId.UserId);
        }

        public async Task<int> AddUserComments(CommentUser commentUser)
        {
            _commentUserRepository.Insert(commentUser);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> DeleteUserComments(CommentUser commentUser)
        {
            _commentUserRepository.Delete(commentUser);
            return await _unitOfWork.SaveChangesAsync();
        }

        // meeting
        public async Task<List<CommentMeeting>> GetMeetingComments(PaginationMeetingId paginationMeetingId)
        {
            return await _commentMeetingRepository.GetAllAsync(paginationMeetingId.Count, paginationMeetingId.Offset, x => x.Meeting.Id == paginationMeetingId.MeetingId);
        }
        public async Task<int> AddMeetingComments(CommentMeeting commentMeeting)
        {
            _commentMeetingRepository.Insert(commentMeeting);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> DeleteMeetingComments(CommentMeeting commentMeeting)
        {
            _commentMeetingRepository.Delete(commentMeeting);
            return await _unitOfWork.SaveChangesAsync();
        }

		
	}
}
