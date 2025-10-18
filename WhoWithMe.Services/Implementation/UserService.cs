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
using WhoWithMe.Data.Repositories;
using WhoWithMe.Data;
using AutoMapper;

namespace WhoWithMe.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IContext _context;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserSubscriber> _userSubscriberRepository;
        private readonly IRepository<MeetingSubscriber> _subscriberMeeting_repository;
        private readonly IRepository<UserPhoneToConfirm> _userPhoneToConfirm_repository;
        private readonly IMapper _mapper;

        public UserService(IContext context,
            IRepository<User> userRepository,
            IRepository<UserSubscriber> userSubscriberRepository,
            IRepository<MeetingSubscriber> subscriberMeetingRepository,
            IRepository<UserPhoneToConfirm> userPhoneToConfirmRepository,
            IMapper mapper)
        {
            _context = context;
            _userRepository = userRepository;
            _userSubscriberRepository = userSubscriberRepository;
            _subscriberMeeting_repository = subscriberMeetingRepository;
            _userPhoneToConfirm_repository = userPhoneToConfirmRepository;
            _mapper = mapper;
        }

        public async Task<List<UserSubscriber>> GetSubscribedUsers(int userId, int count, int offset) // return users? // design
        {
            return await _userSubscriberRepository.GetAllAsync(count, offset, x => x.TargetUserId == userId);
        }

        public async Task<List<MeetingSubscriber>> GetUserVisits(int userId, int count, int offset) // return meetings? // designs
        {
            return await _subscriberMeeting_repository.GetAllAsync(count, offset, x => x.UserId == userId);
        }

        public async Task<UserDTO> GetUserInfo(long userId)
        {
            User user = await _userRepository.GetSingleAsync(userId);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<int> EditUserInfo(UserDTO user)
        {
            var entity = _mapper.Map<User>(user);
            _userRepository.Update(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<UserDTO>> GetUsers(FromToLong fromTo)
        {
            List<User> users = await _userRepository.GetAllAsync(x => x.Id > fromTo.From && x.Id < fromTo.To);
            return users.Select(x => _mapper.Map<UserDTO>(x)).ToList();
        }

        public async Task<bool> DeleteUser(long id)
        {
            User user = await _userRepository.GetSingleAsync(x => x.Id == id);
            if (user == null)
            {
                throw new BadRequestException("User not found");
            }
            _userRepository.Delete(user);
            await _context.SaveChangesAsync();
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

            _userPhoneToConfirm_repository.Insert(userPhoneToConfirm);
            await _context.SaveChangesAsync();
            // send confirmPassword to phone TODO REDO
            return true;
        }

        public async Task<bool> ConfirmPhoneNumberPassword(PhoneConfirmPassword phoneConfirmPassword)
        {
            Random r = new Random();
            List<UserPhoneToConfirm> phoneToConfirm = await _userPhoneToConfirm_repository.FindByAsync(x =>
            x.DateTimeCreated > DateTime.Now.AddDays(-1)
            && x.UserId == phoneConfirmPassword.UserId
            && x.PhoneNumber == phoneConfirmPassword.PhoneNumber
            && x.ConfirmPassword == phoneConfirmPassword.ConfirmPassword);
            if (phoneToConfirm?.Count > 0)
            {
                User user = await _userRepository.GetSingleAsync(phoneConfirmPassword.UserId);
                user.PhoneNumber = phoneConfirmPassword.PhoneNumber;
                user.PhoneNumberConfirmed = true;
                await _context.SaveChangesAsync();
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