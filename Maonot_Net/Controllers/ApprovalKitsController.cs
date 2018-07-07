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
    public class ApprovalKitsController : Controller
    {
        private readonly MaonotNetContext _context;

        public ApprovalKitsController(MaonotNetContext context)
        {
            _context = context;
        }

        // GET: ApprovalKits
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

            var app = from s in _context.ApprovalKits
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                app = app.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    app = app.OrderByDescending(s => s.LastName);
                    break;
                case "first_name_desc":
                    app = app.OrderByDescending(s => s.FirstName);
                    break;
                case "first_name":
                    app = app.OrderBy(s => s.FirstName);
                    break;
                case "room_type_desc":
                    app = app.OrderByDescending(s => s.RoomType);
                    break;
                case "room_type":
                    app = app.OrderBy(s => s.RoomType);
                    break;

                default:
                    app = app.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<ApprovalKit>.CreateAsync(app.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: ApprovalKits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalKit = await _context.ApprovalKits
                .SingleOrDefaultAsync(m => m.ID == id);
            if (approvalKit == null)
            {
                return NotFound();
            }

            return View(approvalKit);
        }

        // GET: ApprovalKits/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: ApprovalKits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StundetId,LastName,FirstName,RoomType,LivingWithReligious,LivingWithSmoker,ReligiousType,HealthCondition,PartnerId1,PartnerId2,PartnerId3,PartnerId4")] ApprovalKit approvalKit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(approvalKit);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "לא היה ניתן לשמור את השינויים, נא נסה שנית במועד מאוחר יותר");

            }
            return View(approvalKit);
        }

        // GET: ApprovalKits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalKit = await _context.ApprovalKits.SingleOrDefaultAsync(m => m.ID == id);
            if (approvalKit == null)
            {
                return NotFound();
            }
            return View(approvalKit);
        }

        // POST: ApprovalKits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,StundetId,LastName,FirstName,RoomType,LivingWithReligious,LivingWithSmoker,ReligiousType,HealthCondition,PartnerId1,PartnerId2,PartnerId3,PartnerId4")] ApprovalKit approvalKit)
        {
            if (id != approvalKit.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(approvalKit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApprovalKitExists(approvalKit.ID))
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
            return View(approvalKit);
        }

        // GET: ApprovalKits/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalKit = await _context.ApprovalKits.AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (approvalKit == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["EErrorMessage"] = "המחיקה נכשלה, נא נסה שנית במועד מאוחד יותר";
            }

            return View(approvalKit);
        }

        // POST: ApprovalKits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var approvalKit = await _context.ApprovalKits.AsNoTracking().SingleOrDefaultAsync(m => m.ID == id);
            if (approvalKit == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.ApprovalKits.Remove(approvalKit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Index), new { id = id, saveCahngeError = true });
            }

        }

        private bool ApprovalKitExists(int id)
        {
            return _context.ApprovalKits.Any(e => e.ID == id);
        }
    }
}
