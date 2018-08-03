using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Maonot_Net.Models
{
    public enum WarningNumber
    {
        ראשונה,
        שנייה,
        שלישית
    };
    public class Warning

    {
        public int WarningId { get; set; }
        [Required]
        [Display(Name = "מספר אזהרה")]
        public WarningNumber? WarningNumber { get; set; }
        public User User { get; set; }
        [Required]
        [Display(Name = "ת.ז. סטודנט")]
        public int StudentId { get; set; }
        [Required]
        [Display(Name = "תאריך הפרת נוהל אירוח")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public string BlaBla { get; set; }




    }
}
