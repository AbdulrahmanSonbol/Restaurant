using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.RestaurantModule
{
    public class RestaurantTag
    {
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = null!;

        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!;
    }
}
