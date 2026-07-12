using Domain.Entities.IdentityModule;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.RestaurantModule
{
    public class Review
    {


		public int Id { get; set; }


		public int  Rating { get; set;  }


		public int RestaurantId { get; set ; }


		public string  UserId { get;  set;  }


        public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
        public string? Comment { get;  set;  }


		


		public DateTime CreatedAt { get ; 	set;  }

        public User User { get; set; } = null!;

        public Restaurant Restaurant { get; set; } = null!;

    }
}
