using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Maonot_Net.Data;
using Maonot_Net.Models;
using Microsoft.AspNetCore.Http;

namespace Maonot_Net.Controllers
{
    public class AuthorizationsController : Controller
    {
        private readonly MaonotNetContext _context;

        public AuthorizationsController(MaonotNetContext context)
        {
            _context = context;
        }

        // GET: Authorizations
        public async Task<IActionResult> Index()
        {
            string Aut = HttpContext.Session.GetString("Aut");
            if (Aut.Equals("1")) { return View(await _context.Authorizations.ToListAsync()); }
            return RedirectToAction("NotAut", "Home");

        }

        // GET: Authorizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string Aut = HttpContext.Session.GetString("Aut");
            if (Aut.Equals("1"))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var authorization = await _context.Authorizations
                    .SingleOrDefaultAsync(m => m.Id == id);
                if (authorization == null)
                {
                    return NotFound();
                }

                return View(authorization);
            }
            return RedirectToAction("NotAut", "Home");
        }

    }
}
