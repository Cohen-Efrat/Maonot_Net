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
    public class FaultFormsController : Controller
    {
        private readonly MaonotNetContext _context;

        public FaultFormsController(MaonotNetContext context)
        {
            _context = context;
        }

        // GET: FaultForms
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

            var faults = from s in _context.FaultForms
                         select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                faults = faults.Where(s => s.FullName.Contains(searchString));

            }
            switch (sortOrder)
            {
                case "name_desc":
                    faults = faults.OrderByDescending(s => s.FullName);
                    break;
                case "Date":
                    faults = faults.OrderBy(s => s.Apartment);
                    break;
                case "date_desc":
                    faults = faults.OrderByDescending(s => s.Apartment);
                    break;
                default:
                    faults = faults.OrderBy(s => s.FullName);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<FaultForm>.CreateAsync(faults.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: FaultForms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faultForm = await _context.FaultForms
                .SingleOrDefaultAsync(m => m.ID == id);
            if (faultForm == null)
            {
                return NotFound();
            }

            return View(faultForm);
        }

        // GET: FaultForms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FaultForms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Apartment,RoomNum,FullName,PhoneNumber,Description")] FaultForm faultForm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(faultForm);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "לא היה ניתן לשמור את השינויים, נא נסה שנית במועד מאוחר יותר");

            }
            return View(faultForm);
        }

        // GET: FaultForms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faultForm = await _context.FaultForms.SingleOrDefaultAsync(m => m.ID == id);
            if (faultForm == null)
            {
                return NotFound();
            }
            return View(faultForm);
        }

        // POST: FaultForms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Apartment,RoomNum,FullName,PhoneNumber,Description")] FaultForm faultForm)
        {
            if (id != faultForm.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faultForm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaultFormExists(faultForm.ID))
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
            return View(faultForm);
        }

        // GET: FaultForms/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faultForm = await _context.FaultForms.AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (faultForm == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["EErrorMessage"] = "המחיקה נכשלה, נא נסה שנית במועד מאוחד יותר";
            }

            return View(faultForm);
        }

        // POST: FaultForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faultForm = await _context.FaultForms.AsNoTracking().SingleOrDefaultAsync(m => m.ID == id);
            if (faultForm == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.FaultForms.Remove(faultForm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Index), new { id = id, saveCahngeError = true });
            }

        }

        private bool FaultFormExists(int id)
        {
            return _context.FaultForms.Any(e => e.ID == id);
        }



    }
}
