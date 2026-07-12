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
		
		public int Id { get; set;  }


        public string Name { get; set; } = string.Empty;
        public int RestaurantId { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

       
        public Restaurant Restaurant { get; set; } = null!;
        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;


        //  public string Category { get; set;  }



    }
}
