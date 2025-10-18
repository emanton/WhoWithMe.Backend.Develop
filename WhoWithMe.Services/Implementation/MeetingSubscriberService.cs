using Core.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhoWithMe.Core.Data;
using WhoWithMe.Core.Entities;
using WhoWithMe.Core.Entities.dictionaries;
using WhoWithMe.DTO.Meeting;
using WhoWithMe.DTO.Model.Meeting;
using WhoWithMe.Services.Exceptions;
using WhoWithMe.Services.Interfaces;
using WhoWithMe.Data.Repositories;
using WhoWithMe.Data;
using AutoMapper;

namespace WhoWithMe.Services.Implementation
{
	public class MeetingSubscriberService : IMeetingSubscriberService
    {
        private readonly IContext _context;
        private readonly IRepository<MeetingSubscriber> _meetingSubscriberRepository;
        private readonly IMapper _mapper;


        public MeetingSubscriberService(IContext context, IRepository<MeetingSubscriber> meetingSubscriberRepository, IMapper mapper)
        {
            _context = context;
            _meetingSubscriberRepository = meetingSubscriberRepository;
            _mapper = mapper;
        }

        public async Task<List<MeetingSubscriber>> GetMeetingAcceptedSubscribers(PaginationMeetingId paginationMeetingId)
        {
            return await _meetingSubscriberRepository.GetAllAsync(paginationMeetingId.Count, paginationMeetingId.Offset, x => x.MeetingId == paginationMeetingId.MeetingId && x.IsAccepted);
        }
        public async Task<List<MeetingSubscriber>> GetMeetingNotAcceptedSubscribers(PaginationMeetingId paginationMeetingId)
        {
            return await _meetingSubscriberRepository.GetAllAsync(paginationMeetingId.Count, paginationMeetingId.Offset, x => x.MeetingId == paginationMeetingId.MeetingId && !x.IsAccepted);
        }

        public async Task<int> AddMeetingSubscriber(MeetingSubscriberDTO meetingSubscriber)
        {
            MeetingSubscriber res = await _meetingSubscriberRepository.GetSingleAsync(x => x.UserId == meetingSubscriber.UserId && x.MeetingId == meetingSubscriber.MeetingId);
            if (res != null)
            {
                throw new BadRequestException("He is already subscriber");
            }
            var entity = _mapper.Map<MeetingSubscriber>(meetingSubscriber);
            _meetingSubscriberRepository.Insert(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateMeetingSubscriberAcceptStatus(MeetingSubscriberDTO meetingSubscriber)
        {
            MeetingSubscriber subscriber = await _meetingSubscriberRepository.GetSingleAsync(x => x.MeetingId == meetingSubscriber.MeetingId && x.UserId == meetingSubscriber.UserId);
            if (subscriber == null)
            {
                throw new BadRequestException("subscriber not fount");
            }
            subscriber.IsAccepted = meetingSubscriber.IsAccepted;
            _meetingSubscriberRepository.Update(subscriber);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteMeetingSubscriber(UserMeetingId participantMeetingId)
        {
            MeetingSubscriber res = await _meetingSubscriberRepository.GetSingleAsync(x => x.UserId == participantMeetingId.UserId && x.MeetingId == participantMeetingId.MeetingId);
            if (res == null)
            {
                throw new BadRequestException("Subscriber not found");
            }

            _meetingSubscriberRepository.Delete(res);
            return await _context.SaveChangesAsync();
        }
    }
}
