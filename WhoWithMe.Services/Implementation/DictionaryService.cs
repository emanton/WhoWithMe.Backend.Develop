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


namespace WhoWithMe.Services.Implementation
{
	public class DictionaryService : IDictionaryService
    {
        private readonly IContext _context;
        private readonly IRepository<MeetingType> _meetingTypeRepository;
        private readonly IRepository<City> _cityRepository;


        public DictionaryService(IContext context)
        {
            _context = context;
            _meetingTypeRepository = new EntityRepository<MeetingType>(context);
            _cityRepository = new EntityRepository<City>(context);
        }

        public async Task<List<MeetingType>> GetMeetingTypes()
        {
            var asd = await _meetingTypeRepository.GetAllAsync();
            return await _meetingTypeRepository.GetAllAsync();
        }

        public async Task<int> AddMeetingType(MeetingTypeDTO meetingType)
        {
            _meetingTypeRepository.Insert(meetingType.GetMeetingType());
            return await _context.SaveChangesAsync();
        }

        public async Task<List<City>> GetCities()
        {
            return await _cityRepository.GetAllAsync();
        }

        public async Task<int> AddCity(CityDTO meetingType)
        {
            _cityRepository.Insert(meetingType.GetCity());
            return await _context.SaveChangesAsync();
        }
    }
}
