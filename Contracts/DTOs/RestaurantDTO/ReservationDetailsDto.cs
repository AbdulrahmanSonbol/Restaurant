using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.DTOs.RestaurantDTO
{
    public class ReservationDetailsDto
    {
        public int Id { get; set; }

        public string ReservationNumber { get; set; } = null!;

        public DateOnly ReservationDate { get; set; }

        public TimeOnly ReservationTime { get; set; }

        public int GuestCount { get; set; }

        public string? SpecialRequest { get; set; }

        public decimal DepositAmount { get; set; }

        public ReservationStatus ReservationStatus { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int RestaurantId { get; set; }

        public int RestaurantTableId { get; set; }

        public Guid UserId { get; set; }

        public bool IsNotificationSent { get; set; }

        public string? FcmToken { get; set; }
    }
}
