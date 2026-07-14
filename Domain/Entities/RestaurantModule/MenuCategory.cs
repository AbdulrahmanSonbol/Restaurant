using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.RestaurantModule
{
    public class MenuCategory
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public int DisplayOrder { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = null!;

        public ICollection<MenuItem> MenuItems { get; set; } = new HashSet<MenuItem>();
    }
}
