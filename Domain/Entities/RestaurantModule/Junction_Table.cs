using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.RestaurantModule
{


    public class Junction_Table
    {

        public int RestaurantIDFK { get; set;  }

        public int USerIDFK { get; set;  }

        public DateTime CreatedAt { get;   set;   }


    }



}
