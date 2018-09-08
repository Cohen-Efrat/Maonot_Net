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
            string Aut = HttpContext.Session.GetString("Aut");
            string Id = HttpContext.Session.GetString("User");
            ViewBag.Aut = Aut;
            // var faultForm = await _context.FaultForms.SingleOrDefaultAsync(m => m.StundetId.Equals("Id"));

            if (Aut.Equals("3") || Aut.Equals("2") || Aut.Equals("9"))
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

                if (Aut.Equals("9")) {
                        faults = from s in _context.FaultForms
                        where s.StundetId.ToString().Equals(Id)
                        select s;
                }
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
           return RedirectToAction("NotAut", "Home");
        }

        // GET: FaultForms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            string Id = HttpContext.Session.GetString("User");
            var faultForm = await _context.FaultForms.SingleOrDefaultAsync(m => m.ID == id);

            if (id == null)
            {
                return NotFound();
            }
            if (faultForm == null)
            {
                return NotFound();
            }
            if (Aut.Equals("3")|| Aut.Equals("2")|| id.Equals(faultForm.StundetId.ToString()))
            {
                return View(faultForm);
            }
            return RedirectToAction("NotAut", "Home");
        }

        // GET: FaultForms/Create
        public IActionResult Create()
        {
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            if (Aut.Equals("9"))
            {
                return View();
            }
            return RedirectToAction("NotAut", "Home");
        }

        // POST: FaultForms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Apartment,RoomNum,FullName,PhoneNumber,Description")] FaultForm faultForm)
        {
            string Id = HttpContext.Session.GetString("User");
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            var u = await _context.Users.SingleOrDefaultAsync(m => m.StundetId.ToString().Equals(Id));
            var r = await _context.Registrations.SingleOrDefaultAsync(m => m.StundetId.ToString().Equals(Id));
            try
            {
                if (ModelState.IsValid)
                {
                    
                    faultForm.StundetId = u.StundetId;
                    faultForm.RoomNum =u.Room;
                    faultForm.Apartment = u.ApartmentNum;
                    faultForm.FullName = u.FullName;
                    faultForm.PhoneNumber = r.PhoneNumber;

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
          
            string Id = HttpContext.Session.GetString("User");
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            var faultForm = await _context.FaultForms.SingleOrDefaultAsync(m => m.ID == id);
            if (faultForm.StundetId.Equals(Id))
            {
               
                if (faultForm.Fix)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }
                    if (faultForm == null)
                    {
                        return NotFound();
                    }
                    return View(faultForm);
                }
                return RedirectToAction("NoMore", "Home");
            }
            return RedirectToAction("NotAut", "Home");

        }

        // POST: FaultForms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Apartment,RoomNum,FullName,PhoneNumber,Description")] FaultForm faultForm)
        {
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
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
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            string Id = HttpContext.Session.GetString("User");
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
            if (Aut.Equals("3") || Aut.Equals("2") || id.Equals(faultForm.StundetId.ToString()))
            {

                return View(faultForm);
            }
            return RedirectToAction("NotAut", "Home");
        }

        // POST: FaultForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            var faultForm = await _context.FaultForms.AsNoTracking().SingleOrDefaultAsync(m => m.ID == id);
            if (faultForm == null)
            {
                return RedirectToAction(nameof(Index));
            }
            if (faultForm.Fix)
            {
                return RedirectToAction("NoMore", "Home");
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
        public async Task<IActionResult> Fix(int? id)

        {
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            if (Aut.Equals("3"))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var fault = await _context.FaultForms.SingleOrDefaultAsync(m => m.ID == id);
                if (fault == null)
                {
                    return NotFound();
                }
                return View(fault);
            }
            return RedirectToAction("NotAut", "Home");

        }

        public async Task<ActionResult> Yes(int id)
        {
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            var fault = await _context.FaultForms.SingleOrDefaultAsync(m => m.ID == id);


            if (fault != null)
            {
                fault.Fix = true;
                _context.Update(fault);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "FaultForms");



            }
            return RedirectToAction("Index", "Home");



        }



    }
}
