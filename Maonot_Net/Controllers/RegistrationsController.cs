using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Maonot_Net.Data;
using Maonot_Net.Models;
using System.IO;


namespace Maonot_Net.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly MaonotNetContext _context;

        public RegistrationsController(MaonotNetContext context)
        {
            _context = context;
        }

        public System.Linq.IQueryable<Maonot_Net.Models.Registration> display()
        {
            var reg = from s in _context.Registrations
                      where s.gender.Equals(Gender.זכר) && s.ApertmantType.Equals(ApertmantType.יחיד)
                      select s;
            return reg;
        }


        // GET: Registrations
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var reg = from s in _context.Registrations where s.gender.Equals(Gender.זכר) &&  s.ApertmantType.Equals(ApertmantType.יחיד)
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                reg = reg.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    reg = reg.OrderByDescending(s => s.LastName);
                    break;
                case "first_name_desc":
                    reg = reg.OrderByDescending(s => s.FirstName);
                    break;
                case "first_name":
                    reg = reg.OrderBy(s => s.FirstName);
                    break;

                default:
                    reg = reg.OrderBy(s => s.LastName);
                    break;
            }
          
            int pageSize = 3;
            return View(await PaginatedList<Registration>.CreateAsync(reg.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Registrations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var registration = await _context.Registrations
            // .SingleOrDefaultAsync(m => m.ID == id);
            var registration = display();
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // GET: Registrations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Registrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,StundetId,LastName,FirstName,Bday,gender,City,Adress,PostalCode,PhoneNumber,FieldOfStudy,SteadyYear,TypeOfService,HealthCondition,Seniority,ApertmantType,ParentID,ParentLastName,PartnerFirstName,ParentAge")] Registration registration)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(registration);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "לא היה ניתן לשמור את השינויים, נא נסה שנית במועד מאוחר יותר");

            }
            return View(registration);
        }

        // GET: Registrations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations.SingleOrDefaultAsync(m => m.ID == id);
            if (registration == null)
            {
                return NotFound();
            }
            return View(registration);
        }

        // POST: Registrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,StundetId,LastName,FirstName,Bday,gender,City,Adress,PostalCode,PhoneNumber,FieldOfStudy,SteadyYear,TypeOfService,HealthCondition,Seniority,ApertmantType,ParentID,ParentLastName,PartnerFirstName,ParentAge")] Registration registration)
        {
            if (id != registration.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationExists(registration.ID))
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
            return View(registration);
        }

        // GET: Registrations/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations.AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (registration == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["EErrorMessage"] = "המחיקה נכשלה, נא נסה שנית במועד מאוחד יותר";
            }

            return View(registration);
        }

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registration = await _context.Registrations.AsNoTracking().SingleOrDefaultAsync(m => m.ID == id);
            if (registration == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Registrations.Remove(registration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Index), new { id = id, saveCahngeError = true });
            }
        }

        private bool RegistrationExists(int id)
        {
            return _context.Registrations.Any(e => e.ID == id);
        }


    }
}
