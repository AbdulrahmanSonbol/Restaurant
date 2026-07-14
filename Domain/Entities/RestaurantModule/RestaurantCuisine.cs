using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.RestaurantModule
{
    public class RestaurantCuisine
    {
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = null!;

        public int CuisineId { get; set; }
        public Cuisine Cuisine { get; set; } = null!;
    }
}
