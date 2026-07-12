using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.RestaurantModule
{
    public class RestaurantTable
    {

		public int Id { get; set; }

		public int RestaurantId { get; set; }


        public string TableNumber { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public string? LocationDescription { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public Restaurant Restaurant { get; set; } = null!;

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    



    }
}
