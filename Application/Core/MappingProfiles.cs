using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<User, User>(); // With this we can map a user's properties to another user, mainly used for put request
        }
    }
}