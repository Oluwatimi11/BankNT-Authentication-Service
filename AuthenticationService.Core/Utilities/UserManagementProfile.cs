using System;
using AuthenticationService.Core.DTOs;
using AuthenticationService.Core.DTOs.AuthDTOs;
using AuthenticationService.Domain.Models;
using AutoMapper;

namespace AuthenticationService.Core.Utilities
{
    public class UserManagementProfile : Profile
    {
        public UserManagementProfile()
        {
            CreateMap<RegistrationDTO, AppUser>()
                 .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Email.ToLower()))
                 .ForMember(dest => dest.UserName, act => act.MapFrom(src => src.Email.ToLower()));
            CreateMap<GetProfileDTO, AppUser>().ReverseMap();
            CreateMap<AppUser, GetUserDTO>().ReverseMap();
        }
    }
}

