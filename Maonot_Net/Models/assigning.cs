using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Maonot_Net.Models
{
    public class Assigning
    {
        public int ID { get; set; }

        public int? StundetId { get; set; }

        public User User { get; set; }

        public int? ApartmentNum { get; set; }

        public int? Room { get; set; }

        


    }
}
