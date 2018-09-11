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
    public class AssigningsController : Controller
    {
        private readonly MaonotNetContext _context;

        public AssigningsController(MaonotNetContext context)
        {
            _context = context;
        }

        // GET: Assignings
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var a = from s in _context.Assigning
                    select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                a = a.Where(s=> s.User.FullName.ToString().Contains(searchString));
                                   
            }
            switch (sortOrder)
            {
                case "FullName_desc":
                    a = a.OrderByDescending(s => s.User.FullName);
                    break;
                case "FullName":
                    a = a.OrderBy(s => s.User.FullName);
                    break;
                case "apartment_desc":
                    a = a.OrderByDescending(s => s.ApartmentNum);
                    break;
                default:
                    a = a.OrderBy(s => s.ApartmentNum);
                    break;
            }

            ViewBag.c = a.Count();
            int pageSize = 3;

            return View(await PaginatedList<Assigning>.CreateAsync(a.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Assignings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assigning = await _context.Assigning
                .SingleOrDefaultAsync(m => m.ID == id);
            if (assigning == null)
            {
                return NotFound();
            }

            return View(assigning);
        }

        // GET: Assignings/Create
        public async Task<IActionResult> Create(int id)
        {
            
            var approvalKit = await _context.ApprovalKits.SingleOrDefaultAsync(m => m.ID == id);
            ViewBag.app = approvalKit;
            List<Apartments> apartment = new List<Apartments> { };
            if (approvalKit.RoomType.Equals(RoomType.חדר_ליחיד))
            {
                  var apartments = from s in _context.Apartments
                                 where (s.Gender.Equals(approvalKit.Gender) && (s.Type.Equals("Single") || s.Type.Equals("Accessible"))
                                 && s.LivingWithSmoker.Equals(approvalKit.LivingWithSmoker) && s.LivingWithReligious.Equals(approvalKit.LivingWithSmoker)
                                 && s.ReligiousType.Equals(approvalKit.ReligiousType) && s.capacity>0) || s.capacity==4
                                 select s;
                foreach (Apartments a in apartments)
                {
                    apartment.Add(a);

                }
                ViewBag.apartment = apartment;
            }
            else
            {
                var apartments = from s in _context.Apartments
                                 where s.capacity>0 && s.Type.Equals("Couples")
                                 select s;
                foreach (Apartments a in apartments)
                {
                    apartment.Add(a);
                }

            }

        

            
            ViewData["ap"] = new SelectList(apartment, "ApartmentNum");
            return View();
        }

        // POST: Assignings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id,[Bind("StundetId,ApartmentNum,Room")] Assigning assigning)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assigning);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(assigning);
        }

        // GET: Assignings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assigning = await _context.Assigning.SingleOrDefaultAsync(m => m.ID == id);
            if (assigning == null)
            {
                return NotFound();
            }
            return View(assigning);
        }

        // POST: Assignings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,StundetId,ApartmentNum,Room")] Assigning assigning)
        {
            if (id != assigning.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assigning);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssigningExists(assigning.ID))
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
            return View(assigning);
        }

        // GET: Assignings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assigning = await _context.Assigning
                .SingleOrDefaultAsync(m => m.ID == id);
            if (assigning == null)
            {
                return NotFound();
            }

            return View(assigning);
        }

        // POST: Assignings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assigning = await _context.Assigning.SingleOrDefaultAsync(m => m.ID == id);
            _context.Assigning.Remove(assigning);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssigningExists(int id)
        {
            return _context.Assigning.Any(e => e.ID == id);
        }
    }
}
