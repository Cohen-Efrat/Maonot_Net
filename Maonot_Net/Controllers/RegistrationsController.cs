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
using Microsoft.AspNetCore.Http;

namespace Maonot_Net.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly MaonotNetContext _context;

        public RegistrationsController(MaonotNetContext context)
        {

            _context = context;
        }

        public async Task<int> GetAut()
        {
            string id = HttpContext.Session.GetString("User");
            var user = await _context.Users.SingleOrDefaultAsync(m => m.StundetId.ToString().Equals(id));
            if (user == null) { return 0; }
            else
            {
                return user.Authorization;
            }

        }

        // GET: Registrations
        public async Task<IActionResult> Index(){

            ViewBag.cF = queryFemale().Count(); 

            ViewBag.cM = queryMale().Count();

            ViewBag.cC = queryCouples().Count();


            return View();
        }

        public async Task<IActionResult> Index_Couples(
    string currentFilter,
    string searchString,
    int? page
    )
        {
            ViewBag.type = "Index_Couples";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
        
            ViewData["CurrentFilter"] = searchString;

            var reg = queryCouples();
            ViewBag.c = reg.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                reg = reg.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }

            int pageSize = 3;
            return View("viewReg",await PaginatedList<Registration>.CreateAsync(reg.AsNoTracking(), page ?? 1, pageSize));
        }

        public async Task<IActionResult> Index_Single_Female(

            string currentFilter,
            string searchString,
            int? page
            )
        {
            ViewBag.type = "Index_Single_Female";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var reg = queryFemale();
            ViewBag.c = reg.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                reg = reg.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }

            int pageSize = 3;
            return View("viewReg", await PaginatedList<Registration>.CreateAsync(reg.AsNoTracking(), page ?? 1, pageSize));
        }

        public async Task<IActionResult> Index_Single_Male(
            string currentFilter,
            string searchString,
            int? page
            )
        {
            ViewBag.type = "Index_Single_Male";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var reg = queryMale();
            ViewBag.c = reg.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                reg = reg.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }
          
            int pageSize = 3;
            return View("viewReg",await PaginatedList<Registration>.CreateAsync(reg.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Registrations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations
             .SingleOrDefaultAsync(m => m.ID == id);
           
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // GET: Registrations/Create
        public IActionResult Create()
        {
            ViewData["StudentID"] = new SelectList(_context.Registrations, "ID", "StundetId");

            return View();
        }

        // POST: Registrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,StundetId,LastName,FirstName,Bday,gender,City,Adress,PostalCode,PhoneNumber,FieldOfStudy,SteadyYear,TypeOfService,HealthCondition,Seniority,ApertmantType,ParentID1,ParentfullName1,ParentAge1,ParentID2,ParentfullName2,ParentAge2,Total,Approved")] Registration registration)
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,StundetId,LastName,FirstName,Bday,gender,City,Adress,PostalCode,PhoneNumber,FieldOfStudy,SteadyYear,TypeOfService,HealthCondition,Seniority,ApertmantType,ParentID1,ParentfullName1,ParentAge1,ParentID2,ParentfullName2,ParentAge2,Total,Approved")] Registration registration)
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

        public async Task<IActionResult> App(int? id)

        {
            if (id == null)
            {
                return NotFound();
            }

            var reg = await _context.Registrations.SingleOrDefaultAsync(m => m.ID == id);
            if (reg == null)
            {
                return NotFound();
            }
            return View(reg);
        }
        // צריך להוסיף שדייר יוכל לחתום רק על האורח שלו
        public async Task<ActionResult> Yes(int id)
        {

            var reg = await _context.Registrations.SingleOrDefaultAsync(m => m.ID == id);
            if (reg != null)
            {

                reg.Approved = true;
                _context.Update(reg);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Registrations");



            }
            return RedirectToAction("Index", "Home");



        }
            private bool RegistrationExists(int id)
        {
            return _context.Registrations.Any(e => e.ID == id);
        }
        public System.Linq.IQueryable<Maonot_Net.Models.Registration> queryFemale()
        {
            var reg = from s in _context.Registrations
                      orderby s.Total descending
                      where s.gender.Equals(Gender.נקבה) && s.ApertmantType.Equals(ApertmantType.יחיד)
                      select s;
            return reg;
        }
        public System.Linq.IQueryable<Maonot_Net.Models.Registration> queryMale()
        {
            var reg = from s in _context.Registrations
                      orderby s.Total descending
                      where s.gender.Equals(Gender.זכר) && s.ApertmantType.Equals(ApertmantType.יחיד)
                      select s;
            return reg;
        }
        public System.Linq.IQueryable<Maonot_Net.Models.Registration> queryCouples()
        {
            var reg = from s in _context.Registrations
                      orderby s.Total descending
                      where s.ApertmantType.Equals(ApertmantType.זוגי)
                      select s;
            return reg;
        }


    }
}
