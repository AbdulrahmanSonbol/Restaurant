using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.RestaurantModule
{
    public class MenuItem
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public decimal Price { get; set; }
        public int PreparationTime { get; set; }

        public bool IsAvailable { get; set; }
        public bool IsPopular { get; set; }
        public bool IsNew { get; set; }

        public string ImageUrl { get; set; } = null!;

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = null!;

        public int CategoryId { get; set; }
        public MenuCategory Category { get; set; } = null!;
        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;
    }
}
