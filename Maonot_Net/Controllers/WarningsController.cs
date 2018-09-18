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
    public class WarningsController : Controller
    {
        private readonly MaonotNetContext _context;

        public WarningsController(MaonotNetContext context)
        {
            _context = context;
        }

        //return a list of warinigs
        // GET: Warnings
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {

            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            string Id = HttpContext.Session.GetString("User");
            var u = await _context.Users.SingleOrDefaultAsync(m => m.StundetId.ToString().Equals(Id));
            if (Aut.Equals("4")|| Aut.Equals("2")|| Aut.Equals("9"))
            {
                ViewBag.Aut = Aut;
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
                var warning = from s in _context.Warnings
                              select s;
                if (Aut.Equals("9"))
                {
                     warning = from s in _context.Warnings
                                  where s.StudentId.Equals(Id)
                                  select s;
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    warning = warning.Where(s => s.StudentId.ToString().Contains(searchString));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        warning = warning.OrderByDescending(s => s.WarningNumber);
                        break;

                    default:
                        warning = warning.OrderBy(s => s.WarningNumber);
                        break;
                }

                int pageSize = 3;
                return View(await PaginatedList<Warning>.CreateAsync(warning.AsNoTracking(), page ?? 1, pageSize));
            }
            return RedirectToAction("NotAut", "Home");
        }

        // GET: Warnings/Details/5
        //return the details of a record by id
        public async Task<IActionResult> Details(int? id)
        {

            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            string Id = HttpContext.Session.GetString("User");

            if (id == null)
            {
                return NotFound();
            }
            var warning = await _context.Warnings
                .SingleOrDefaultAsync(m => m.WarningId == id);
            if (warning == null)
            {
                return NotFound();
            }
            if(warning.StudentId.Equals(Id)||Aut.Equals("2")|| Aut.Equals("3"))
            {
                return View(warning);
            }
            return RedirectToAction("NotAut", "Home");


        }

        // GET: Warnings/Create
        //return the warning form
        public IActionResult Create()
        {

            ViewData["StudentId"] = new SelectList(_context.ApprovalKits, "StudentId", "FullName");
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            if (Aut.Equals("2") || Aut.Equals("4"))
            {
                return View();
            }
            return RedirectToAction("NotAut", "Home");
        }

        // POST: Warnings/Create
// validate the warining form and send a messege to the user how gut the warinig
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WarningNumber,StudentId,Date,BlaBla")] Warning warning)
        {
            string Id = HttpContext.Session.GetString("User");

            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            
            try
            { 

                if (ModelState.IsValid)
                {
                    _context.Add(warning);
                    Message msg = new Message
                    {
                        From = "ועדת משמעת",
                        Addressee = Id,
                        MsgTime=DateTime.Now,
                        Subject = "אזהרת משמעת",
                        Content = "קיבלת מכתב אזהרה מועדת המשמעת בעקבות אורח שלא חתמת עליו ביומן המבקרים" +
                        "במידה ואת/ה חושב/ת שהייתה טעות נא לפנות בהודעה לועדת המשמעת" +
                        "במידה וזו אזהרה שלישית חל עלייך איסור לארח למשך שבוע החל מרגע זה."
                    };
                    _context.Add(msg);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "לא היה ניתן לשמור את השינויים, נא נסה שנית במועד מאוחר יותר");

            }
            return View(warning);
        }

        // GET: Warnings/Delete/5
        // get the details of the recoerd and ask the user if he is sure
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {

            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            if (Aut.Equals("2"))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var warning = await _context.Warnings.AsNoTracking()
                    .SingleOrDefaultAsync(m => m.WarningId == id);
                if (warning == null)
                {
                    return NotFound();
                }
                if (saveChangesError.GetValueOrDefault())
                {
                    ViewData["EErrorMessage"] = "המחיקה נכשלה, נא נסה שנית במועד מאוחד יותר";
                }

                return View(warning);
            }
            return RedirectToAction("NotAut", "Home");

        }

        // POST: Warnings/Delete/5
        // if the user confirem the delete this function start and delete the recore from the DB
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            var warning = await _context.Warnings.AsNoTracking().SingleOrDefaultAsync(m => m.WarningId == id);
            if (warning == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Warnings.Remove(warning);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Index), new { id = id, saveCahngeError = true });
            }
        }

        private bool WarningExists(int id)
        {
            return _context.Warnings.Any(e => e.WarningId == id);
        }
    }
}
