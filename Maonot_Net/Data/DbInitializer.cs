using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maonot_Net.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Maonot_Net.Data
{
    public class DbInitializer
    {
        public static void Initialize(MaonotNetContext context)
        {
            context.Database.EnsureCreated();
            if (context.Users.Any())
            {
                return;
            }
            var users = new User[]
            {
                new User{StundetId= 302875125, FirstName= "Miki", LastName= "Rotenstain", Password= "M2i1k2i1", Email= "mikir2127@gmail.com"},
                new User{StundetId= 308242122, FirstName= "Efrat", LastName= "Cohen", Password= "M2i1k2i1", Email= "efratc66@gmail.com"}
            };
            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();

            var reg = new Registration[]
            {
                new Registration{StundetId= users.Single(u=> u.StundetId == 302875125).StundetId, FirstName= users.Single(u=> u.StundetId == 302875125).FirstName , LastName= users.Single(u=> u.StundetId == 302875125).LastName,gender=Gender.זכר, Bday= DateTime.Parse("1989-11-04"), City= "נשר", Adress= "עמוס 18",PhoneNumber= "050-2480441",
                    FieldOfStudy=FieldStudy.מערכות_מידע,SteadyYear=Year.ג, TypeOfService=Service.צבאי, HealthCondition=HealthCondition.ללא_מגבלה, Seniority=Year.ג, ApertmantType= ApertmantType.יחיד},
                new Registration{StundetId= users.Single(u=> u.StundetId == 308242122).StundetId, FirstName= users.Single(u=> u.StundetId == 308242122).FirstName , LastName= users.Single(u=> u.StundetId == 308242122).LastName,gender=Gender.נקבה, Bday= DateTime.Parse("1992-02-14"), City= "להבים", Adress= "גומא 2",PhoneNumber= "054-6814427",
                    FieldOfStudy=FieldStudy.מערכות_מידע,SteadyYear=Year.ג, TypeOfService=Service.צבאי, HealthCondition=HealthCondition.ללא_מגבלה, Seniority=Year.ג, ApertmantType= ApertmantType.יחיד},
            };
            foreach (Registration u in reg)
            {
                context.Registrations.Add(u);
            }
            context.SaveChanges();

            var App = new ApprovalKit[]
            {
                new ApprovalKit{StundetId= users.Single(u=> u.StundetId == 302875125).StundetId, FirstName= users.Single(u=> u.StundetId == 302875125).FirstName , LastName= users.Single(u=> u.StundetId == 302875125).LastName, RoomType=RoomType.חדר_ליחיד,
                    LivingWithSmoker = Choose.אפשרי,LivingWithReligious=Choose.אפשרי, ReligiousType=Religious.דרוזי, HealthCondition=HealthCondition.ללא_מגבלה},
                new ApprovalKit{StundetId= users.Single(u=> u.StundetId == 308242122).StundetId, FirstName= users.Single(u=> u.StundetId == 308242122).FirstName , LastName= users.Single(u=> u.StundetId == 308242122).LastName, RoomType=RoomType.חדר_ליחיד,
                    LivingWithSmoker = Choose.לא_מעוניין,LivingWithReligious=Choose.מעוניין, ReligiousType=Religious.יהודי, HealthCondition=HealthCondition.ללא_מגבלה}
            };
            foreach (ApprovalKit u in App)
            {
                context.ApprovalKits.Add(u);
            }
            context.SaveChanges();

            var Fault = new FaultForm[]
            {
                new FaultForm{Apartment= 927, RoomNum= 1, FullName= users.Single(u=> u.StundetId==308242122).FirstName+""+users.Single(u=> u.StundetId==308242122).LastName, PhoneNumber="050-2480441",Description="הבוילר לא עובד לי אשמח לעזרתכם" }
            };
            foreach (FaultForm u in Fault)
            {
                context.FaultForms.Add(u);
            }
            context.SaveChanges();

            var Messg = new Message[]
            {
                new Message{Addressee="efratc66@gmail.com", Subject= "ענת השותפות של מציקות לי", Content= "לענת שלום, השותפות שלי מטריפות אותי מה אפשר לעשות" }
            };
            foreach (Message u in Messg)
            {
                context.Messages.Add(u);
            }
            context.SaveChanges();

            var Vlog = new VisitorsLog[]
            {
                new VisitorsLog{VistorName= "דני שובבני", StudentFullName= users.Single(u=> u.StundetId== 302875125).FullName,
                     ApartmentNum=927,Room=RoomNum.OneA, EnteryDate= DateTime.Parse("2018-05-22"),VisitorID= 123456789}
            };
            foreach (VisitorsLog u in Vlog)
            {
                context.VisitorsLogs.Add(u);
            }
            context.SaveChanges();

            var War = new Warning[]
            {
                new Warning{WarningNumber=WarningNumber.ראשונה, StudentId=users.Single(u=> u.StundetId==302875125).StundetId, Date=DateTime.Parse("2018-05-20")}
            };
            foreach (Warning u in War)
            {
                context.Warnings.Add(u);
            }
            context.SaveChanges();
            var Aut = new Authorization[]
           {
                new Authorization{ AutName = "מנהל מערכת"},
                new Authorization{ AutName = "מנהל"},
                new Authorization{ AutName = "אבות בית"},
                new Authorization{ AutName = "ועדת משמעת"},
                new Authorization{ AutName = "ועדת תרבות"},
                new Authorization{ AutName = "עובד אבטחה"},
                new Authorization{ AutName = "אורח"},
                new Authorization{ AutName = "מועמד"},
                new Authorization{ AutName = "דייר"}
           };
            foreach (Authorization u in Aut)
            {
                context.Authorizations.Add(u);
            }
            context.SaveChanges();



        }

    }
}
