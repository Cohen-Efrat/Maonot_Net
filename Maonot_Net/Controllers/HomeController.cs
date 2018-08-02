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

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult NotAut()
        {
            return View();
        }
        public async Task<IActionResult> Wellcome()
        {
            string ID = HttpContext.Session.GetString("User");
            var user = await _context.Users.AsNoTracking().SingleOrDefaultAsync(m => m.StundetId.ToString().Equals(ID));
            if (user != null)
            {
                ViewBag.username = user.FullName;
            }
            else
            {
                ViewBag.username = "haha not working";
            }
            
            return View();
        }
        public IActionResult NoMore()
        {
            return View();
        }

    }
}
