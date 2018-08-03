using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Maonot_Net.Models
{
    public class FaultForm
    {
        public int ID { get; set; }

        [Display(Name = "תעודת זהות")]
        public int? StundetId { get; set; }
        
        [Display(Name = "מספר דירה")]

        public int? Apartment { get; set; }
        [Display(Name = "מספר חדר")]
        public RoomNum? RoomNum { get; set; }
       
        [Display(Name = "שם מלא")]
        public string FullName { get; set; }

        [Display(Name = "מספר פלאפון")]
        [RegularExpression(@"^0\d([\d]{0,1})([-]{0,1})\d{7}$", ErrorMessage = "Please Enter Correct Phone Number(9/10 digits")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "תאור התקלה")]
        public string Description { get; set; }

        [Display(Name = "תוקן")]
        public bool Fix { get; set; }

    }
}
