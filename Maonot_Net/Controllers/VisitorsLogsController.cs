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
//blabla
//string Aut = HttpContext.Session.GetString("Aut");
//string Id = HttpContext.Session.GetString("User");
// u = await _context.Users.SingleOrDefaultAsync(m => m.StundetId.Equals("Id"));
//return RedirectToAction("NotAut", "Home");
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
            ViewBag.LastDate = Globals.LastDate;
            string Aut = HttpContext.Session.GetString("Aut");
            if (Aut.Equals("2") || Aut.Equals("9") || Aut.Equals("4"))
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
                    vistor = vistor.Where(s => s.StudentFullName.Contains(searchString)
                                           || s.VistorName.Contains(searchString));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        vistor = vistor.OrderByDescending(s => s.StudentFullName);
                        break;
                    case "name":
                        vistor = vistor.OrderBy(s => s.StudentFullName);
                        break;
                    case "date_desc":
                        vistor = vistor.OrderByDescending(s => s.EnteryDate);
                        break;
                    default:
                        vistor = vistor.OrderBy(s => s.EnteryDate);

                        break;
                }

                int pageSize = 3;
                return View(await PaginatedList<VisitorsLog>.CreateAsync(vistor.AsNoTracking(), page ?? 1, pageSize));
            }
            return RedirectToAction("NotAut", "Home");
        }

        // GET: VisitorsLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string Aut = HttpContext.Session.GetString("Aut");
            if (Aut.Equals("2") || Aut.Equals("9") || Aut.Equals("4"))
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
            return RedirectToAction("NotAut", "Home");
        }

        // GET: VisitorsLogs/Create
        public IActionResult Create()
        {
            string Aut = HttpContext.Session.GetString("Aut");
            if (Aut.Equals("2") || Aut.Equals("9") || Aut.Equals("4"))
            {
                ViewData["FullName"] = new SelectList(_context.ApprovalKits, "ID", "FullName");
                //make a list of appartments
                return View();
            }
            return RedirectToAction("NotAut", "Home");
        }

        // POST: VisitorsLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnteryDate,VistorName,VisitorID,StudentFullName,ExitDate,ApartmentNum,Room,Signature")] VisitorsLog visitorsLog)
        {
            VisitorsLog v = await _context.VisitorsLogs.AsNoTracking().SingleOrDefaultAsync(m => m.ApartmentNum == visitorsLog.ApartmentNum && m.Room == visitorsLog.Room);

            try
                {
                //visitorsLog.EnteryDate = DateTime.Now;
                visitorsLog.EnteryDate = DateTime.Parse("2018-02-14");
                   // visitorsLog.StudentId = v.StudentId;
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
            string Aut = HttpContext.Session.GetString("Aut");
            if (Aut.Equals("2") || Aut.Equals("9") || Aut.Equals("4"))
            {
                ViewData["FullName"] = new SelectList(_context.ApprovalKits, "ID", "FullName");
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
            return RedirectToAction("NotAut", "Home");
        }

        // POST: VisitorsLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EnteryDate,VistorName,VisitorID,StudentFullName,ExitDate,ApartmentNum,Room,Signature")] VisitorsLog visitorsLog)
        {
            if (id != visitorsLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    visitorsLog.ExitDate = DateTime.Now;
                    if (visitorsLog.ExitDate.Value.Date > visitorsLog.EnteryDate.Date)
                    {
                        SendMsg(visitorsLog.StudentId, visitorsLog.EnteryDate, visitorsLog.VistorName);
                    }
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

        private void SendMsg(int? studentId, DateTime enteryDate, string vistorName)
        {
            Message msg = new Message
            {
                Addressee = studentId.ToString(),
                From = "אבטחה",
                Subject = "חתימה על אורח/ת",
                Content = "התארח/ה אצלך אורח/ת בשם" +
                vistorName + "בתאריך" +
                enteryDate +
                "מכוון שעברה השעה 00:00 נדרש לחתום על האורח/ת " +
                "לטובת כך נא לעבור לאיזור האישי וללחוץ על חתימה"
            };

            _context.Messages.Add(msg);
            _context.SaveChanges();

        }

        // GET: VisitorsLogs/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            string Aut = HttpContext.Session.GetString("Aut");
            if (Aut.Equals("2"))
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
            return RedirectToAction("NotAut", "Home");
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

        public async Task<IActionResult> Signature(int? id)
     
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
        // צריך להוסיף שדייר יוכל לחתום רק על האורח שלו
        public async Task<ActionResult> ConifiremSigniture(int id, int StudentId , string password)
        {
            string Id = HttpContext.Session.GetString("User");
            var u = await _context.Users.SingleOrDefaultAsync(m => m.StundetId.Equals("Id"));
            //return RedirectToAction("NotAut", "Home");

            ViewBag.StudentId = StudentId;
            var functions = new functions();
            var user = await _context.Users.SingleOrDefaultAsync(m => m.StundetId == StudentId);
            if (user != null)
            {
                if (functions.CheckPassword(password, user.Password))
                {
         
                    var visitorsLog = await _context.VisitorsLogs.SingleOrDefaultAsync(m => m.Id == id);
                    visitorsLog.Signature = true;
                    _context.Update(visitorsLog);
                    await _context.SaveChangesAsync();

                    return View();
                }
                else
                {
                    ViewBag.Message = "Thank you!";
                    TempData["msg"] = "<script>alert('סיסמה לא נכונה');</script>";

                   return RedirectToAction(nameof(Index));
                }
            }
           return  RedirectToAction("Index", "Home");





        }
        public async Task<ActionResult> CheckVistorLog()
        {
            DateTime LastDate = Globals.LastDate;
            var vistor = from s in _context.VisitorsLogs
                         where s.EnteryDate > LastDate && s.EnteryDate < DateTime.Now
                         && s.Signature == false 
                         select s;

            List<VisitorsLog> warningList = new List<VisitorsLog>();
            foreach (var v in vistor)
            {
                if (!(v.EnteryDate.Date == v.ExitDate.Value.Date))
                {
                    var w = await _context.Warnings.SingleOrDefaultAsync(m => m.StudentId == v.StudentId);
                    Warning warning = new Warning();
                    warning.StudentId = v.StudentId;
                    warning.Date = v.EnteryDate;
                    if (w == null)
                    {
                        warning.WarningNumber = WarningNumber.ראשונה;
                    }

                    // 2 warinigs

                    else if (w.WarningNumber.Equals("שנייה"))
                    {
                        warning.WarningNumber = WarningNumber.שלישית;
                    }

                    // 1 waring 
                    else if (w.WarningNumber.Equals("ראשונה"))
                    {
                        warning.WarningNumber = WarningNumber.שנייה;
                    }

                    //  3 warnings
                    else
                    {
                        warning.WarningNumber = WarningNumber.ראשונה;
                    }
                    warningList.Add(v);
                    _context.Warnings.Add(warning);
                }
            }
            _context.SaveChanges();
            ViewBag.warningList = warningList;
            Globals.LastDate = DateTime.Now;

            return View();

        }

        private async Task SendWarning(int? studentId, DateTime date)
        {


        
        

        }
    }
}
