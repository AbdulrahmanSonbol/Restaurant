using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.DTOs.RestaurantDTO
{
    public class UpdateReservationDto
    {
        public DateOnly ReservationDate { get; set; }

        public TimeOnly ReservationTime { get; set; }

        public int GuestCount { get; set; }

        public int RestaurantTableId { get; set; }

        public string? SpecialRequest { get; set; }

        public string? FcmToken { get; set; }
    }
}
