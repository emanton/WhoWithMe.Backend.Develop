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
using WhoWithMe.DTO.UserDTOs;

namespace WhoWithMe.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserSubscriber> _userSubscriberRepository;
        private readonly IRepository<MeetingSubscriber> _subscriberMeetingRepository;
        private readonly IRepository<UserPhoneToConfirm> _userPhoneToConfirmRepository;
        
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepository = unitOfWork.GetRepository<User>();
            _userSubscriberRepository = unitOfWork.GetRepository<UserSubscriber>();
            _subscriberMeetingRepository = unitOfWork.GetRepository<MeetingSubscriber>();
            _userPhoneToConfirmRepository = unitOfWork.GetRepository<UserPhoneToConfirm>();
        }

        public async Task<List<UserSubscriber>> GetSubscribedUsers(int userId, int count, int offset) // return users? // design
        {
            return await _userSubscriberRepository.GetAllAsync(count, offset, x => x.TargetUserId == userId);
        }

        public async Task<List<MeetingSubscriber>> GetUserVisits(int userId, int count, int offset) // return meetings? // designs
        {
            return await _subscriberMeetingRepository.GetAllAsync(count, offset, x => x.UserId == userId);
        }

        public async Task<UserDTO> GetUserInfo(long userId)
        {
            User user = await _userRepository.GetSingleAsync(userId);
            return new UserDTO(user);
        }

        public async Task<int> EditUserInfo(UserDTO user)
        {
            _userRepository.Update(user.GetUser());
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<UserDTO>> GetUsers(FromToLong fromTo)
        {
            List<User> users = await _unitOfWork.GetRepository<User>().GetAllAsync(x => x.Id > fromTo.From && x.Id < fromTo.To);
            return users.Select(x => new UserDTO(x)).ToList();
        }

        public async Task<bool> DeleteUser(long id)
        {
			User user = await _unitOfWork.GetRepository<User>().GetSingleAsync(x => x.Id == id);
            if (user == null)
			{
                throw new BadRequestException("User not found");
			}
            _userRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        // hide profile

        public async Task<bool> UpdatePhoneNumber(UserIdPhone userIdPhone)
        {
            Random r = new Random();
            int confirmPassword = r.Next(1000, 9999);
            UserPhoneToConfirm userPhoneToConfirm = new UserPhoneToConfirm()
            {
                UserId = userIdPhone.UserId,
                PhoneNumber = userIdPhone.PhoneNumber,
                ConfirmPassword = confirmPassword,
                DateTimeCreated = DateTime.Now,
            };

            _userPhoneToConfirmRepository.Insert(userPhoneToConfirm);
            await _unitOfWork.SaveChangesAsync();
            // send confirmPassword to phone TODO REDO
            return true;
        }

        public async Task<bool> ConfirmPhoneNumberPassword(PhoneConfirmPassword phoneConfirmPassword)
        {
            Random r = new Random();
			List<UserPhoneToConfirm> phoneToConfirm = await _userPhoneToConfirmRepository.FindByAsync(x =>
            x.DateTimeCreated > DateTime.Now.AddDays(-1)
            && x.UserId == phoneConfirmPassword.UserId
            && x.PhoneNumber == phoneConfirmPassword.PhoneNumber
            && x.ConfirmPassword == phoneConfirmPassword.ConfirmPassword);
            if (phoneToConfirm?.Count > 0)
			{
                User user = await _userRepository.GetSingleAsync(phoneConfirmPassword.UserId);
                user.PhoneNumber = phoneConfirmPassword.PhoneNumber;
                user.PhoneNumberConfirmed = true;
                await _unitOfWork.SaveChangesAsync();
                return true;
            }

            return false;
        }

  //      private async Task<bool> AddBonusToInviter()
		//{

		//}
    }
}


// TODO
// kilometers distance-