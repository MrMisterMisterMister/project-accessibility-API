using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // With this we can map an object's properties to another object
            // mainly used for put request
            CreateMap<User, User>();
            CreateMap<Company, Company>();
            CreateMap<PanelMember, PanelMember>();
        }
    }
}