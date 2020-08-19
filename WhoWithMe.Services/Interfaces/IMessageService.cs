using Core.Data;
using Core.Data.Repositories;
using WhoWithMe.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoWithMe.Services.Interfaces
{
    public interface IMessageService
    {
        Task<UserChat> GetUserChats(int userId, int count, int offset);

        Task<List<UnreadMessage>> GetUserUnreadMessagesByChatId(int chatId, int count, int offset);

        Task<int> DeleteUnreadMessagesByChatId(List<UnreadMessage> unreadMessages);

        Task<int> CreateMessage(Message message);
    }
}
