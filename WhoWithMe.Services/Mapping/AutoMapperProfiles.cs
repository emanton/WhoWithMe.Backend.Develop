using AutoMapper;
using WhoWithMe.Core.Entities;
using WhoWithMe.DTO.UserDTOs;
using WhoWithMe.DTO.Meeting;
using WhoWithMe.DTO.Meeting.Abstract;
using WhoWithMe.DTO.Authorization;
using WhoWithMe.Core.Entities.dictionaries;

namespace WhoWithMe.Services.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Map User -> UserDTO using the DTO constructor
            CreateMap<User, UserDTO>().ConstructUsing(u => new UserDTO(u));
            CreateMap<UserDTO, User>();

            // Map User -> UserWithToken using constructor
            CreateMap<User, UserWithToken>().ConstructUsing(u => new UserWithToken(u));

            // Map Meeting DTOs to Meeting entity
            CreateMap<MeetingBaseDTO, Meeting>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<MeetingCreateDTO, Meeting>().IncludeBase<MeetingBaseDTO, Meeting>();
            CreateMap<MeetingEditDTO, Meeting>().IncludeBase<MeetingBaseDTO, Meeting>();

            // Map Meeting entity to MeetingView DTO
            CreateMap<Meeting, MeetingView>().ConstructUsing(m => new MeetingView(m));

            // Map MeetingType and City DTOs
            CreateMap<MeetingTypeDTO, MeetingType>();
            CreateMap<MeetingType, MeetingTypeDTO>();

            CreateMap<CityDTO, City>();
            CreateMap<City, CityDTO>();

            // Map MeetingSubscriber DTO
            CreateMap<MeetingSubscriberDTO, MeetingSubscriber>();

            // Additional mappings as needed
        }
    }
}
