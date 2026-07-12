using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities.IdentityModule
{
    public class Address
    {
        public int Id { get; set; } // PK
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string Country { get; set; } = default!;

        public Guid UserId { get; set; } // Foreign Key

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = default!;
    }
}
