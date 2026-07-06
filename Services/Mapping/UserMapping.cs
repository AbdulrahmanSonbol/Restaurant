using AutoMapper;
using Contracts.DTOs.UserDTO;
using Domain.Entities.IdentitMyodule;
using Domain.Entities.IdentityModule;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<RegisterDTO, User>()
                  .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))

                  .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))

                  .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))

                  .ForMember(dest => dest.Role, opt => opt.MapFrom(src => UserRole.User));


            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        }
    }
}
