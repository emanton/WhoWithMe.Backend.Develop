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


namespace WhoWithMe.Services.Implementation
{
	public class DictionaryService : IDictionaryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<MeetingType> _meetingTypeRepository;
        private readonly IRepository<City> _cityRepository;


        public DictionaryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _meetingTypeRepository = unitOfWork.GetRepository<MeetingType>();
            _cityRepository = unitOfWork.GetRepository<City>();
        }

        public async Task<List<MeetingType>> GetMeetingTypes()
        {
            return await _meetingTypeRepository.GetAllAsync();
        }

        public async Task<int> AddMeetingType(MeetingTypeDTO meetingType)
        {
            _meetingTypeRepository.Insert(meetingType.GetMeetingType());
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<City>> GetCities()
        {
            return await _cityRepository.GetAllAsync();
        }

        public async Task<int> AddCity(CityDTO meetingType)
        {
            _cityRepository.Insert(meetingType.GetCity());
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
