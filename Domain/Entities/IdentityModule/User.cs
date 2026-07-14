using Domain.Entities.RestaurantModule;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.IdentityModule
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public bool IsDeleted { get; set; } = false;
        public UserRole Role { get; set; } = UserRole.User;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Address? Address { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

      
        public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public ICollection<Favorite> Favorites { get; set; } = new HashSet<Favorite>();

       
        public ICollection<Payment> VerifiedPayments { get; set; } = new HashSet<Payment>();
    }
}

