namespace GestionReclamation.Mappings;

using AutoMapper;
using GestionReclamation.Entities;
using GestionReclamation.Models.DTOs;
using GestionReclamation.Models.Requests;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Reclamation mappings
        CreateMap<Reclamation, ReclamationDto>()
            .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email));

        CreateMap<CreateReclamationRequest, Reclamation>()
            .ForMember(dest => dest.DateCreation, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Statut, opt => opt.MapFrom(src => "EnAttente"))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Reponse, opt => opt.Ignore())
            .ForMember(dest => dest.DateReponse, opt => opt.Ignore());

        // User mappings
        CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore());
    }
}