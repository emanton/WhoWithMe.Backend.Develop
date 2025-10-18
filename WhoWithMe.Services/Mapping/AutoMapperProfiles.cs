using AutoMapper;
using WhoWithMe.Core.Entities;
using WhoWithMe.DTO.UserDTOs;

namespace WhoWithMe.Services.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Map User -> UserDTO using the DTO constructor
            CreateMap<User, UserDTO>().ConstructUsing(u => new UserDTO(u));

            // Add other mappings as needed
        }
    }
}
