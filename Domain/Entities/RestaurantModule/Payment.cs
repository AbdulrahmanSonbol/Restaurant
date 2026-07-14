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
    public class Payment
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        public string ReceiptImage { get; set; } = null!;

        public Guid? VerifiedByUserId { get; set; }
        public User? VerifiedByUser { get; set; }

        public DateTime CreatedAt { get; set; }

        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; } = null!;
        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;
    }

}
