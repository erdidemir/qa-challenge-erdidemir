using AutoMapper;
using CodingChallenge.Data.DataModels;
using CodingChallenge.Dtos;

namespace CodingChallenge.Service.MappingProfiles
{
    /// <summary>
    /// The user mapping profile.
    /// </summary>
    public class UserMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserMappingProfile"/> class.
        /// </summary>
        public UserMappingProfile()
        {
            CreateMap<UserDataModel, UserDto>();
            CreateMap<AddOrUpdateUserDto, UserDataModel>();
        }
    }
}
