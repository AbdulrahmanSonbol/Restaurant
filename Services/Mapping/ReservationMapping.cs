using AutoMapper;
using Contracts.DTOs.RestaurantDTO;
using Domain.Entities.RestaurantModule;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Mapping
{
    public class ReservationMapping : Profile
    {
        public ReservationMapping()
        {
            CreateMap<CreateReservationDto, Reservation>()
                .ForMember(dest => dest.ReservationStatus, opt => opt.MapFrom(src => ReservationStatus.Pending))

                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))

                .ForMember(dest => dest.ReservationNumber, opt => opt.MapFrom(src => $"RES-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 5).ToUpper()}"))

                .ForMember(dest => dest.IsNotificationSent, opt => opt.MapFrom(src => false))

                .ForMember(dest => dest.DepositAmount, opt => opt.MapFrom(src => 0m));

            CreateMap<Reservation, ReservationDetailsDto>();

            CreateMap<UpdateReservationDto, Reservation>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
