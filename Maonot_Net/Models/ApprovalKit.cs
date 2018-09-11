using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Maonot_Net.Models
{
    public enum RoomType
    {
        חדר_ליחיד,
        דירה_זוגית
    }
    public enum Choose
    {
        מעוניין,
        לא_מעוניין,
       
    }
    public enum Religious
    {
        יהודי,
        מוסלמי,
        נוצרי,
        דרוזי,
      
    }
    public class ApprovalKit
    {

        public int ID { get; set; }
        
        [Display(Name = "תעודת זהות")]
        public int? StundetId { get; set; }
        
        [Display(Name = "שם משפחה")]
        public string LastName { get; set; }
        
        [Display(Name = "שם פרטי")]
        public string FirstName { get; set; }

        [Display(Name = "שם מלא")]
        public string FullName
        {
            get
            {
                return LastName + " " + FirstName;
            }
        }
        public Registration Reg { get; set; }
        //public User User { get; set; }
        [Required]
        [Display(Name = "סוג חדר")]
        public RoomType? RoomType { get; set; }
        [Required]
        [Display(Name = "מגורים עם סטודנט/ית דתי/ה")]
        public Choose? LivingWithReligious { get; set; }
        [Required]
        [Display(Name = "מגורים עם סטודנט/ית מעשנ/ת")]
        public Choose? LivingWithSmoker { get; set; }
        [Required]
        [Display(Name = "מעוניין במגורים עם סטודנט/ית")]
        public Religious? ReligiousType { get; set; }
        [Display(Name = "מגבלות רפואיות")]
        public HealthCondition? HealthCondition { get; set; }
        [Display(Name = "העדפה לשותף 1")]
        public int? PartnerId1 { get; set; }
        [Display(Name = "העדפה לשותף 2")]
        public int? PartnerId2 { get; set; }
        [Display(Name = "העדפה לשותף 3")]
        public int? PartnerId3 { get; set; }

        public Gender? Gender { get; set; }


    }
}
