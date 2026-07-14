using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.RestaurantModule
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public ICollection<RestaurantTag> RestaurantTags { get; set; } = new HashSet<RestaurantTag>();
    }
}
