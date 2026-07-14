
using Domain.Entities.IdentityModule;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.RestaurantModule
{
    public class Reservation
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
        public Restaurant Restaurant { get; set; } = null!;

        public int RestaurantTableId { get; set; }
        public RestaurantTable RestaurantTable { get; set; } = null!;

        public Guid UserId { get; set; } 
        public User User { get; set; } = null!;

        public Payment? Payment { get; set; }
        public Review? Review { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;
    }
}
