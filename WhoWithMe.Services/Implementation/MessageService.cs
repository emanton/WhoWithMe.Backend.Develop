using Core.Data;
using Core.Data.Repositories;
using WhoWithMe.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoWithMe.Core.Data;
using WhoWithMe.Data.Repositories;
using WhoWithMe.Data;

namespace WhoWithMe.Services.Implementation
{
    public class MessageService
    {
        private readonly IContext _context;
        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<UnreadMessage> _unreadMessageRepository;

        public MessageService(IContext context)
        {
            _context = context;
            _messageRepository = new EntityRepository<Message>(context);
            _unreadMessageRepository = new EntityRepository<UnreadMessage>(context);
        }

        public async Task<int> CreateMessage(Message message)
        {
            await SendMassageToUser(message);
            _messageRepository.Insert(message);
            return await _context.SaveChangesAsync();
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
