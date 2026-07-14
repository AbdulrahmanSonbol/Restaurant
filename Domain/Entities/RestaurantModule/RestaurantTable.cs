using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.RestaurantModule
{
    public class RestaurantTable
    {
        public int Id { get; set; }

        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public int Floor { get; set; }

        public bool NearWindow { get; set; }
        public bool Outdoor { get; set; }
        public bool VIP { get; set; }

        public TableStatus TableStatus { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = null!;

        public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;
    }
}
