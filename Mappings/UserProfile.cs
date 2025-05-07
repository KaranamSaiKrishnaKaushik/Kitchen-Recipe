using AutoMapper;
using DataModels;
using DTOs;

namespace Mappings;

public class UserProfile: Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, UserDetails>().ReverseMap();
    }
}