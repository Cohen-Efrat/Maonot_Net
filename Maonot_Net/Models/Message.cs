using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Maonot_Net.Models
{
    public class Message
    {

        public int MessageID { set; get; }
        [Required]
        [Display(Name = "מאת")]
        public string From { set; get; }

        [Display(Name = "נמען")]
        public string Addressee { set; get; }
        [Required]
        [Display(Name = "נושא הודעה")]
        public string Subject { set; get; }
        [Required]
        [Display(Name = "תוכן הודעה")]
        public string Content { set; get; }
   





    }

}
