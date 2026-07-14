using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities.RestaurantModule
{
    public class Restaurant
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Website { get; set; }

        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }

        public bool IsFeatured { get; set; }

        public string CoverImage { get; set; } = null!;

        public RestaurantStatus RestaurantStatus { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Address { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string PriceRange { get; set; }

        public int ReservationDurationMinutes { get; set; }

        public bool IsDepositRequired { get; set; }
        public decimal DepositAmount { get; set; }

        public ICollection<RestaurantImage> Images { get; set; } = new HashSet<RestaurantImage>();
        public ICollection<RestaurantCuisine> RestaurantCuisines { get; set; } = new HashSet<RestaurantCuisine>();
        public ICollection<RestaurantTag> RestaurantTags { get; set; } = new HashSet<RestaurantTag>();
        public ICollection<RestaurantWorkingHour> WorkingHours { get; set; } = new HashSet<RestaurantWorkingHour>();
        public ICollection<RestaurantTable> Tables { get; set; } = new HashSet<RestaurantTable>();
        public ICollection<MenuCategory> Categories { get; set; } = new HashSet<MenuCategory>();
        public ICollection<MenuItem> MenuItems { get; set; } = new HashSet<MenuItem>();
        public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public ICollection<Favorite> Favorites { get; set; } = new HashSet<Favorite>();
        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;
    }
}
