using Core.Data;
using Core.Data.Repositories;
using WhoWithMe.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoWithMe.Core.Data;
using WhoWithMe.Services.Interfaces;
using WhoWithMe.DTO;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.IIS;
using WhoWithMe.Services.Exceptions;

namespace WhoWithMe.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserSubscriber> _userSubscriberRepository;
        private readonly IRepository<MeetingSubscriber> _subscriberMeetingRepository;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepository = unitOfWork.GetRepository<User>();
            _userSubscriberRepository = unitOfWork.GetRepository<UserSubscriber>();
            _subscriberMeetingRepository = unitOfWork.GetRepository<MeetingSubscriber>();
        }

        public async Task<List<UserSubscriber>> GetSubscribedUsers(int userId, int count, int offset) // return users? // design
        {
            return await _userSubscriberRepository.GetAllAsync(count, offset, x => x.TargetUserId == userId);
        }

        public async Task<List<MeetingSubscriber>> GetUserVisits(int userId, int count, int offset) // return meetings? // designs
        {
            return await _subscriberMeetingRepository.GetAllAsync(count, offset, x => x.UserId == userId);
        }

        public async Task<User> GetUserInfo(long userId)
        {
            return await _userRepository.GetSingleAsync(userId);
        }

        public async Task<int> EditUserInfo(User user)
        {
            _userRepository.Update(user);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<User>> GetUsers(FromToLong fromTo)
        {
            return await _unitOfWork.GetRepository<User>().GetAllAsync(x => x.Id > fromTo.From && x.Id < fromTo.To);
        }

        public async Task<bool> DeleteUser(long id)
        {
			User user = await GetUserInfo(id);
            if (user == null)
			{
                throw new BadRequestException("User not found");
			}
            _unitOfWork.GetRepository<User>().Delete(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        // hide profile
    }
}





// TODO
// kilometers distance-