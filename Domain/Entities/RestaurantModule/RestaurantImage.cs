using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.RestaurantModule
{
    public class RestaurantImage
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; } = null!;
        public int DisplayOrder { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = null!;
    }
}
