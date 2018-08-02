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
            if (Aut.Equals(1)) { return View(await _context.Authorizations.ToListAsync()); }
            return RedirectToAction("NotAut", "Home");

        }

        // GET: Authorizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string Aut = HttpContext.Session.GetString("Aut");
            if (Aut.Equals(1))
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

        // GET: Authorizations/Create
        public IActionResult Create()
        {
            string Aut = HttpContext.Session.GetString("Aut");
            if (Aut.Equals(1))
            {
                return View();
            }
            return RedirectToAction("NotAut", "Home");
        }

        // POST: Authorizations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AutName")] Authorization authorization)
        {
            string Aut = HttpContext.Session.GetString("Aut");
            if (Aut.Equals(1))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(authorization);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(authorization);
            }
            return RedirectToAction("NotAut", "Home");
        }

        // GET: Authorizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            string Aut = HttpContext.Session.GetString("Aut");
            if (Aut.Equals(1))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var authorization = await _context.Authorizations.SingleOrDefaultAsync(m => m.Id == id);
                if (authorization == null)
                {
                    return NotFound();
                }
                return View(authorization);
            }
            return RedirectToAction("NotAut", "Home");
        }

        // POST: Authorizations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AutName")] Authorization authorization)
        {
            string Aut = HttpContext.Session.GetString("Aut");
            if (Aut.Equals(1))
            {
                if (id != authorization.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(authorization);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AuthorizationExists(authorization.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(authorization);
            }
            return RedirectToAction("NotAut", "Home");
        }

        // GET: Authorizations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            string Aut = HttpContext.Session.GetString("Aut");
            if (Aut.Equals(1))
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

        // POST: Authorizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string Aut = HttpContext.Session.GetString("Aut");
            if (Aut.Equals(1))
            {
                var authorization = await _context.Authorizations.SingleOrDefaultAsync(m => m.Id == id);
                _context.Authorizations.Remove(authorization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("NotAut", "Home");
        }

        private bool AuthorizationExists(int id)
        {
            return _context.Authorizations.Any(e => e.Id == id);
        }
    }
}
