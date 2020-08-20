using Core.Data;
using Core.Data.Repositories;
using WhoWithMe.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoWithMe.Core.Data;

namespace WhoWithMe.Services.Implementation
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserSubscriber> _userSubscriberRepository;
        private readonly IRepository<ParticipantMeeting> _participantMeetingRepository;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepository = unitOfWork.GetRepository<User>();
            _userSubscriberRepository = unitOfWork.GetRepository<UserSubscriber>();
            _participantMeetingRepository = unitOfWork.GetRepository<ParticipantMeeting>();
        }

        public async Task<List<UserSubscriber>> GetSubscribedUsers(int userId, int count, int offset) // return users? // design
        {
            return await _userSubscriberRepository.GetAllAsync(count, offset, x => x.TargetUserId == userId);
        }

        public async Task<List<ParticipantMeeting>> GetUserVisits(int userId, int count, int offset) // return meetings? // designs
        {
            return await _participantMeetingRepository.GetAllAsync(count, offset, x => x.User.Id == userId);
        }

        public async Task<User> GetUserInfo(int userId)
        {
            return await _userRepository.GetSingleAsync(userId);
        }

        public async Task<int> EditUserInfo(User user)
        {
            _userRepository.Update(user);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> Register(User user)
        {
            ValidateEmail(user.Email);
            ValidatePassword(user.Password);
            user.Password = Encrypt(user.Password);
            _userRepository.Insert(user);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<User> Login(string login, string password)
        {
            string enPassword = Encrypt(password);
			List<User> res = await _userRepository.GetAllAsync(x => x.Email == login && x.Password == enPassword);
            if(res.Count > 1)
			{
                throw new Exception("ambitions of user!");
            }
            if (res.Count == 0)
            {
                throw new Exception("user not found");
            }

            return res.First();
        }

        private void ValidatePassword(string password)
        {
            if(password.Length <= 6)
			{
                throw new Exception("password has to be > 6");
            }
        }

        private void ValidateEmail(string email)
        {
            bool isValid;
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                isValid = addr.Address == email;
            }
            catch
            {
                isValid = false;
            }

            if(!isValid)
			{
                throw new Exception("not valid email");
            }

            // check for uniq TODO
        }

        private string Encrypt(string inputString)
		{
            byte[] data = System.Text.Encoding.ASCII.GetBytes(inputString);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            String hash = System.Text.Encoding.ASCII.GetString(data);
            return hash;
        }

        // Forgot Password
        // hide profile
    }
}





// TODO
// kilometers distance-