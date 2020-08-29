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
    public class MessageService
    {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IRepository<UserChat> _userChatRepository;
		private readonly IRepository<Message> _messageRepository;
		private readonly IRepository<UnreadMessage> _unreadMessageRepository;

		public MessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userChatRepository = unitOfWork.GetRepository<UserChat>();
            _messageRepository = unitOfWork.GetRepository<Message>();
            _unreadMessageRepository = unitOfWork.GetRepository<UnreadMessage>();
        }

        public async Task<UserChat> GetUserChats(int userId, int count, int offset) // return users? // design
        {
			List<UserChat> chats = await _userChatRepository.GetAllAsync(count, offset, x => x.Owner.Id == userId);
            if (chats.Count > 1)
			{
                throw new Exception("Ambitiouns in chat");
			}

            return chats.FirstOrDefault();
        }

        public async Task<List<UnreadMessage>> GetUserUnreadMessagesByChatId(int chatId)
        {
            return await _unreadMessageRepository.GetAllAsync(x => x.Chat.Id == chatId);
        }

		public async Task<int> DeleteUnreadMessagesByChatId(List<UnreadMessage> unreadMessages)
		{
			foreach (UnreadMessage unreadMessage in unreadMessages)
			{
                _unreadMessageRepository.Delete(unreadMessage);
            }
           
			return await _unitOfWork.SaveChangesAsync();
		}

		public async Task<int> CreateMessage(MessageBase message)
        {
            _messageRepository.Insert(message as Message);
            _unreadMessageRepository.Insert(message as UnreadMessage);
            return await _unitOfWork.SaveChangesAsync();

            // firebase push
        }
    }
}
