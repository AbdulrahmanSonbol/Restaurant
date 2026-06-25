using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Identityodule
{
    public class Address
    {
        public int Id { get; set; } // PK
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string Country { get; set; } = default!;
       
    }
}
