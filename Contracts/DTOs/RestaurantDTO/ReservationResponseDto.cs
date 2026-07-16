using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.DTOs.RestaurantDTO
{
    public class ReservationResponseDto
    {
        public string Message { get; set; } = "Reservation created successfully";
        public int ReservationId { get; set; }
        public string ReservationNumber { get; set; } = null!;
    }
}
