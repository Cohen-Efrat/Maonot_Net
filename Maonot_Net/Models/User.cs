using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Maonot_Net.Models
{
    public class User
    {

        public int ID { get; set; }
        [Required]
        [Display(Name = "תעודת זהות")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Please Enter Correct ID")]
        public int StundetId { get; set; }
        [Required]
        [Display(Name = "שם פרטי")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "שם משפחה")]
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return LastName + FirstName;
            }
        }
        [Required]
       [RegularExpression(@"(?=^.{8,}$)(?=.*\d)(?=.*[!@#$%^&*]+)(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$",
        ErrorMessage = "הסיסמה צריכה להיות 8 תווים לפחות, הסיסמה חייבת להכיל אותיות גדולות וקטנות, הסיסמה חייבת להכיל מספר, הסיסמה חייבת להכיל תו יחודי.")]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא")]
        public string Password { get; set; }

        
        [Display(Name = "דואר אלקטרוני")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
        ErrorMessage = "Please Enter Correct Email Address")]
        [Required]
        public string Email { get; set; }

        
        [Display(Name = "מספר דירה")]
        public int? ApartmentNum { get; set; }
        //
        [Display(Name = "מספר חדר")]
        public RoomNum? Room { get; set; }

        public Authorization Aut { get; set; }

        [Display(Name = "רמת הרשאה")]
        public int Authorization { get; set; }
    }
}
