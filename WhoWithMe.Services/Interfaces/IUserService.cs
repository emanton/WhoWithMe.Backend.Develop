using Core.Data;
using Core.Data.Repositories;
using WhoWithMe.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WhoWithMe.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserSubscriber>> GetSubscribedUsers(int userId, int count, int offset);
        Task<List<ParticipantMeeting>> GetUserVisits(int userId, int count, int offset);
        Task<User> GetUserInfo(int userId);
        Task<int> EditUserInfo(User user);
    }
}
