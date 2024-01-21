using Application.DisabilityHandlers;
using Application.Handlers.PanelMemberHandlers;
using Application.Handlers.ResearchHandlers;
using Application.Handlers.UserHandlers;
using AutoMapper;
using Domain;
using Domain.Models.ChatModels;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Domain.Models.Disabilities;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // With this we can map an object's properties to another object
            CreateMap<User, User>();
            CreateMap<User, UserDTO>()
                .ForMember(x => x.Type, o => o.MapFrom(src => src.GetType().ToString()))
                .ForMember(x => x.HashedPassword, o => o.MapFrom(src => src.PasswordHash))
                .ForMember(x => x.UserName, o => o.MapFrom(src => src.UserName));
            CreateMap<Company, Company>();
            CreateMap<PanelMember, PanelMember>();
            CreateMap<Chat, Chat>();
            CreateMap<Message, Message>();
            CreateMap<PanelMember, PanelMemberDTO>()
                .ForMember(x => x.ParticipationsId, o => o.MapFrom(src => src.Participations.Select(p => p.ResearchId)))
                .ForMember(x => x.DisabilitiesId, o => o.MapFrom(src => src.Disabilities.Select(d => d.DisabilityId)))
                .ForMember(x => x.DisabilitiesName, o => o.MapFrom(src => src.Disabilities.Select(d => d.Disability.Name)));
            CreateMap<Research, Research>();
            CreateMap<Research, ResearchDTO>()
                .ForMember(x => x.OrganizerId, o => o.MapFrom(src => src.Organizer!.Id))
                .ForMember(x => x.OrganizerName, o => o.MapFrom(src => src.Organizer!.CompanyName));
            CreateMap<Disability, Disability>();
            CreateMap<Disability, DisabilityDTO>()
                .ForMember(x => x.PanelMemberId, o => o.MapFrom(src => src.PanelMembers.Select(e => e.PanelMemberId)));
            CreateMap<ResearchParticipant, PanelMemberDTO>()
                .ForMember(x => x.Id, o => o.MapFrom(src => src.PanelMember.Id))
                .ForMember(x => x.Guardian, o => o.MapFrom(src => src.PanelMember.Guardian))
                .ForMember(x => x.FirstName, o => o.MapFrom(src => src.PanelMember.FirstName))
                .ForMember(x => x.LastName, o => o.MapFrom(src => src.PanelMember.LastName))
                .ForMember(x => x.DateOfBirth, o => o.MapFrom(src => src.PanelMember.DateOfBirth))
                .ForMember(x => x.Address, o => o.MapFrom(src => src.PanelMember.Address))
                .ForMember(x => x.PostalCode, o => o.MapFrom(src => src.PanelMember.PostalCode))
                .ForMember(x => x.City, o => o.MapFrom(src => src.PanelMember.City))
                .ForMember(x => x.Country, o => o.MapFrom(src => src.PanelMember.Country))
                .ForMember(x => x.LastName, o => o.MapFrom(src => src.PanelMember.LastName));
        }
    }
}