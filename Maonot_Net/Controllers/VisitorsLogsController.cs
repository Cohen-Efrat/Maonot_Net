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
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var vistor = from s in _context.VisitorsLogs
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                vistor = vistor.Where(s => s.StudentFirstName.Contains(searchString)
                                       || s.StudentLasttName.Contains(searchString)
                                       ||s.VistorName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    vistor = vistor.OrderByDescending(s => s.StudentLasttName);
                    break;
                case "name":
                    vistor = vistor.OrderBy(s => s.StudentLasttName);
                    break;
                case "date_desc":
                    vistor = vistor.OrderByDescending(s => s.EnteryDate);
                    break;
                case "firts_name_desc":
                    vistor = vistor.OrderByDescending(s => s.StudentFirstName);
                    break;
                case "_first_name":
                    vistor = vistor.OrderBy(s => s.StudentFirstName);
                    break;
                default:
                    vistor = vistor.OrderBy(s => s.EnteryDate);
                    
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<VisitorsLog>.CreateAsync(vistor.AsNoTracking(), page ?? 1, pageSize));
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
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitorsLog = await _context.VisitorsLogs.AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);
            if (visitorsLog == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["EErrorMessage"] = "המחיקה נכשלה, נא נסה שנית במועד מאוחד יותר";
            }

            return View(visitorsLog);
        }

        // POST: VisitorsLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var visitorsLog = await _context.VisitorsLogs.AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);
            if (visitorsLog == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.VisitorsLogs.Remove(visitorsLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Index), new { id = id, saveCahngeError = true });
            }
        }

        private bool VisitorsLogExists(int id)
        {
            return _context.VisitorsLogs.Any(e => e.Id == id);
        }
    }
}
