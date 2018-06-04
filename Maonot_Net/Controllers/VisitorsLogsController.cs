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
    public class VisitorsLogsController : Controller
    {
        private readonly MaonotNetContext _context;

        public VisitorsLogsController(MaonotNetContext context)
        {
            _context = context;
        }

        // GET: VisitorsLogs
        public async Task<IActionResult> Index()
        {
            return View(await _context.VisitorsLogs.ToListAsync());
        }

        // GET: VisitorsLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitorsLog = await _context.VisitorsLogs
                .SingleOrDefaultAsync(m => m.Id == id);
            if (visitorsLog == null)
            {
                return NotFound();
            }

            return View(visitorsLog);
        }

        // GET: VisitorsLogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VisitorsLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnteryDate,VistorName,VisitorID,StudentFirstName,StudentLasttName,ExitDate,ApartmentNum,Room,Signature")] VisitorsLog visitorsLog)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(visitorsLog);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "לא היה ניתן לשמור את השינויים, נא נסה שנית במועד מאוחר יותר");

            }
            return View(visitorsLog);
        }

        // GET: VisitorsLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitorsLog = await _context.VisitorsLogs.SingleOrDefaultAsync(m => m.Id == id);
            if (visitorsLog == null)
            {
                return NotFound();
            }
            return View(visitorsLog);
        }

        // POST: VisitorsLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EnteryDate,VistorName,VisitorID,StudentFirstName,StudentLasttName,ExitDate,ApartmentNum,Room,Signature")] VisitorsLog visitorsLog)
        {
            if (id != visitorsLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visitorsLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitorsLogExists(visitorsLog.Id))
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
            return View(visitorsLog);
        }

        // GET: VisitorsLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitorsLog = await _context.VisitorsLogs
                .SingleOrDefaultAsync(m => m.Id == id);
            if (visitorsLog == null)
            {
                return NotFound();
            }

            return View(visitorsLog);
        }

        // POST: VisitorsLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var visitorsLog = await _context.VisitorsLogs.SingleOrDefaultAsync(m => m.Id == id);
            _context.VisitorsLogs.Remove(visitorsLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisitorsLogExists(int id)
        {
            return _context.VisitorsLogs.Any(e => e.Id == id);
        }
    }
}
