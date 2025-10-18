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
	public class DictionaryService : IDictionaryService
    {
        private readonly IContext _context;
        private readonly IRepository<MeetingType> _meetingTypeRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IMapper _mapper;


        public DictionaryService(IContext context, IRepository<MeetingType> meetingTypeRepository, IRepository<City> cityRepository, IMapper mapper)
        {
            _context = context;
            _meetingTypeRepository = meetingTypeRepository;
            _cityRepository = cityRepository; // typo fix
            _mapper = mapper;
        }

        public async Task<List<MeetingType>> GetMeetingTypes()
        {
            var asd = await _meetingTypeRepository.GetAllAsync();
            return await _meetingTypeRepository.GetAllAsync();
        }

        public async Task<int> AddMeetingType(MeetingTypeDTO meetingType)
        {
            var entity = _mapper.Map<MeetingType>(meetingType);
            _meetingTypeRepository.Insert(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<City>> GetCities()
        {
            return await _cityRepository.GetAllAsync();
        }

        public async Task<int> AddCity(CityDTO meetingType)
        {
            var entity = _mapper.Map<City>(meetingType);
            _cityRepository.Insert(entity);
            return await _context.SaveChangesAsync();
        }
    }
}
