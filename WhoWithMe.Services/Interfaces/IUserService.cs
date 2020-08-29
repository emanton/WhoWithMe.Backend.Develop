using Core.Data;
using Core.Data.Repositories;
using WhoWithMe.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhoWithMe.DTO;

namespace WhoWithMe.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserSubscriber>> GetSubscribedUsers(int userId, int count, int offset);
        Task<List<MeetingSubscriber>> GetUserVisits(int userId, int count, int offset);
        Task<User> GetUserInfo(long userId);
        Task<List<User>> GetUsers(FromToLong fromTo);
        Task<int> EditUserInfo(User user);
        Task<bool> DeleteUser(long id);
    }
}
