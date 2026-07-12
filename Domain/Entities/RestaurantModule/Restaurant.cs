using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.RestaurantModule
{
    public class Restaurant
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty; 
        public string Category { get; set; } = string.Empty; 
        public string Price { get; set; } = string.Empty; 

        public double Rating { get; set; } 
        public string Time { get; set; } = string.Empty; 
        public bool IsOpen { get; set; } 

        public double Latitude { get; set; }  
        public double Longitude { get; set; }
    }
}
