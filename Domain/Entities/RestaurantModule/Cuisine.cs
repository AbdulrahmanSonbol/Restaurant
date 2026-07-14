using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.RestaurantModule
{
    public class Cuisine
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Icon { get; set; }

        public ICollection<RestaurantCuisine> RestaurantCuisines { get; set; } = new HashSet<RestaurantCuisine>();
    }
}
