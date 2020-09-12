using Core.Data;
using Core.Data.Repositories;
using WhoWithMe.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhoWithMe.DTO;
using WhoWithMe.DTO.UserDTOs;

namespace WhoWithMe.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserSubscriber>> GetSubscribedUsers(int userId, int count, int offset);
        Task<List<MeetingSubscriber>> GetUserVisits(int userId, int count, int offset);
        Task<UserDTO> GetUserInfo(long userId);
        Task<List<UserDTO>> GetUsers(FromToLong fromTo);
        Task<int> EditUserInfo(UserDTO user);
        Task<bool> DeleteUser(long id);
        Task<bool> ConfirmPhoneNumberPassword(PhoneConfirmPassword phoneConfirmPassword);
        Task<bool> UpdatePhoneNumber(UserIdPhone userIdPhone);
    }
}
