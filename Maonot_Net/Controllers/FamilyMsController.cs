using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Maonot_Net.Data;
using Maonot_Net.Models;

namespace Maonot_Net.Controllers
{
    public class FamilyMsController : Controller
    {
        private readonly MaonotNetContext _context;

        public FamilyMsController(MaonotNetContext context)
        {
            _context = context;
        }

        // GET: FamilyMs
        public async Task<IActionResult> Index()
        {
            return View(await _context.FamilyM.ToListAsync());
        }

        // GET: FamilyMs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var familyM = await _context.FamilyM
                .SingleOrDefaultAsync(m => m.ID == id);
            if (familyM == null)
            {
                return NotFound();
            }

            return View(familyM);
        }

        // GET: FamilyMs/Create
        public IActionResult Create()
        {
            ViewData["StudentID"] = new SelectList(_context.Registrations, "ID", "StundetId");
            return View();
        }

        // POST: FamilyMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,StudentID,FullName,Age")] FamilyM familyM)
        {
            if (ModelState.IsValid)
            {
                _context.Add(familyM);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(familyM);
        }

        // GET: FamilyMs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var familyM = await _context.FamilyM.SingleOrDefaultAsync(m => m.ID == id);
            if (familyM == null)
            {
                return NotFound();
            }
            return View(familyM);
        }

        // POST: FamilyMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,StudentID,FullName,Age")] FamilyM familyM)
        {
            if (id != familyM.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(familyM);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FamilyMExists(familyM.ID))
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
            return View(familyM);
        }

        // GET: FamilyMs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var familyM = await _context.FamilyM
                .SingleOrDefaultAsync(m => m.ID == id);
            if (familyM == null)
            {
                return NotFound();
            }

            return View(familyM);
        }

        // POST: FamilyMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var familyM = await _context.FamilyM.SingleOrDefaultAsync(m => m.ID == id);
            _context.FamilyM.Remove(familyM);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FamilyMExists(int id)
        {
            return _context.FamilyM.Any(e => e.ID == id);
        }
    }
}
