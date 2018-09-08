using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Maonot_Net.Models;
using Microsoft.AspNetCore.Http;
using Maonot_Net.Data;
using Microsoft.EntityFrameworkCore;

namespace Maonot_Net.Controllers
{
    public class HomeController : Controller
    {
        private readonly MaonotNetContext _context;

        public HomeController(MaonotNetContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult NotAut()
        {
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            return View();
        }
        public async Task<IActionResult> Wellcome()
        {
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            string ID = HttpContext.Session.GetString("User");

            //Beging Personl Info
            var user = await _context.Users.AsNoTracking().SingleOrDefaultAsync(m => m.StundetId.ToString().Equals(ID));
            ViewBag.user = user;
            //End Personl Info

            if (Aut.Equals("8"))
            {
                //edit registration form
                var u = await _context.Registrations.AsNoTracking().SingleOrDefaultAsync(m => m.StundetId.ToString().Equals(ID));
                ViewBag.RegID = u.ID;
            }
            if (Aut.Equals("9"))
            {
                //edit ApprovlalKit form

                var u = await _context.ApprovalKits.AsNoTracking().SingleOrDefaultAsync(m => m.StundetId.ToString().Equals(ID));
                if (u == null)
                {
                    ViewBag.flage = 0;
                }
                else
                {
                    ViewBag.AppID = u.ID;
                    ViewBag.flage = 1;
                }
                
                
                //Beging falutForm
                var faults = from s in _context.FaultForms
                             where s.StundetId.ToString().Equals(ID)
                             select s;
                List<FaultForm> listFaults = new List<FaultForm> { };
                foreach (var fault in faults)
                {
                    listFaults.Add(fault);
                }
                ViewBag.listFaults = listFaults;
                //End falutForm

                //Beging Warning

                var warning = from s in _context.Warnings
                              where s.StudentId.ToString().Equals(ID)
                              select s;

                List<Warning> Warninglist = new List<Warning> { };
                foreach (var w in warning)
                {
                    Warninglist.Add(w);
                }
                ViewBag.Warninglist = Warninglist;

                //End Warning

                //Beging visitorLog

                var visitors = from s in _context.VisitorsLogs
                               where s.StudentId.ToString().Equals(ID)
                               select s;

                List<VisitorsLog> Visitorslist = new List<VisitorsLog> { };
                foreach (var v in visitors)
                {
                    Visitorslist.Add(v);
                }
                ViewBag.Visitorslist = Visitorslist;

                //End visitorLog
            }






            if (user != null)
            {
                ViewBag.username = user.FullName;
            }

            
            return View();
        }
        public IActionResult NoMore()
        {
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            return View();
        }

        public IActionResult ExistsForm()
        {
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            return View();
        }

    }
}
