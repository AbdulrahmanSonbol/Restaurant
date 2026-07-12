using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.DTOs.RestaurantDTO
{
    public class RestaurantDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Rating { get; set; } 
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
