using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.DTOs.RestaurantDTO
{
    public class CreateReservationDto
    {
        public DateOnly ReservationDate { get; set; }
        public TimeOnly ReservationTime { get; set; }
        public int GuestCount { get; set; }
        public string? SpecialRequest { get; set; }
        public int RestaurantId { get; set; }
        public int RestaurantTableId { get; set; }
        public Guid UserId { get; set; }
        public string? FcmToken { get; set; }

    }
}
