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
		private readonly IRepository<Message> _messageRepository;
		private readonly IRepository<UnreadMessage> _unreadMessageRepository;

		public MessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _messageRepository = unitOfWork.GetRepository<Message>();
            _unreadMessageRepository = unitOfWork.GetRepository<UnreadMessage>();
        }

		//#region if disabled notification, won`t send the notification
		//public async Task<List<UnreadMessage>> GetUserUnreadMessagesByUserId(int chatId)
  //      {
  //          return await _unreadMessageRepository.GetAllAsync(x => x.Chat.Id == chatId);
  //      }

  //      public async Task<int> DeleteUnreadMessagesByChatId(List<UnreadMessage> unreadMessages)
		//{
		//	foreach (UnreadMessage unreadMessage in unreadMessages)
		//	{
  //              _unreadMessageRepository.Delete(unreadMessage);
  //          }
           
		//	return await _unitOfWork.SaveChangesAsync();
		//}
		//#endregion

		public async Task<int> CreateMessage(Message message)
        {
            await SendMassageToUser(message);
            _messageRepository.Insert(message);
            return await _unitOfWork.SaveChangesAsync();
        }

        private async Task SendMassageToUser(Message message) // move to correlation function, queue, microservice
		{
            bool signalRHubOpened = false; // TODO
            if (signalRHubOpened)
            {
                // TODO
            }
            else
            {
                // firebase push or _unreadMessageRepository.insert
            }
        }
    }
}
