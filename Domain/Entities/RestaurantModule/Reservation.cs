
using Domain.Entities.IdentityModule;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.RestaurantModule
{
    public class Reservation
    {


		public int Id { get; set;  }

        public string ReservationNumber { get; set; } = string.Empty;
        public string UserId { get;  set; }


		public int RestaurantId { get; set;  }


		public DateTime BookingTime { get ; set;  }
		

		public int GuestCount { get; set; }


        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;



        public DateTime TimeStamp { get; set; } 


		public DateTime CreatedAt { get; set; }


		public string? SpecialRequests { get;  set; }
        public User User { get; set; } = null!;

        public Restaurant Restaurant { get; set; } = null!;
     


    }
}
