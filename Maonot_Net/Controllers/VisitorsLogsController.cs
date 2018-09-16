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
    public class VisitorsLogsController : Controller
    {
        private readonly MaonotNetContext _context;

        public VisitorsLogsController(MaonotNetContext context)
        {
            _context = context;
        }

        // GET: VisitorsLogs
        // return the visitor log list
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            ViewBag.LastDate = Globals.LastDate;
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            if (Aut.Equals("2") || Aut.Equals("6") || Aut.Equals("4"))
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
        //return the details of a record by id
        // GET: VisitorsLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            if (Aut.Equals("2") || Aut.Equals("6") || Aut.Equals("4"))
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
        //retuen entring a guset form 
        public IActionResult Create()
        {
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            if ( Aut.Equals("6"))
            {
                ViewData["FullName"] = new SelectList(_context.ApprovalKits, "ID", "FullName");
                ViewData["Apartments"] = new SelectList(_context.Apartments, "ApartmentNum", "ApartmentNum");
                return View();
            }
            return RedirectToAction("NotAut", "Home");
        }

        // POST: VisitorsLogs/Create
        //validate the fields from the visitor log. 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnteryDate,VistorName,VisitorID,StudentFullName,ExitDate,ApartmentNum,Room,Signature")] VisitorsLog visitorsLog)
        {
            User u = await _context.Users.AsNoTracking().SingleOrDefaultAsync(m => m.ApartmentNum == visitorsLog.ApartmentNum && m.Room == visitorsLog.Room);
            ViewData["FullName"] = new SelectList(_context.ApprovalKits, "ID", "FullName");
            ViewData["Apartments"] = new SelectList(_context.Apartments, "ApartmentNum", "ApartmentNum");
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;

            try
            {
                visitorsLog.EnteryDate = DateTime.Now;
                
                visitorsLog.StudentId = u.StundetId;
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
        // reporet on a exit with the option to change details 
        public async Task<IActionResult> Edit(int? id)
        {
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            if (Aut.Equals("6"))
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
        //save the cahnges of the record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EnteryDate,VistorName,VisitorID,StudentFullName,ExitDate,ApartmentNum,Room,Signature")] VisitorsLog visitorsLog)
        {

            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
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
        //send a msg if the guest
        private void SendMsg(int? studentId, DateTime enteryDate, string vistorName)
        {
            Message msg = new Message
            {
                Addressee = studentId.ToString(),
                From = "אבטחה",
                MsgTime = DateTime.Now,
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
        // get the details of the recoerd and ask the user if he is sure
        // GET: VisitorsLogs/Delete/5
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

        // if the user confirem the delete this function start and delete the recore from the DB
        // POST: VisitorsLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
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
        // signture form
        public async Task<IActionResult> Signature(int? id)
        {
            string Id = HttpContext.Session.GetString("User");
            var visitorsLog = await _context.VisitorsLogs.SingleOrDefaultAsync(m => m.Id == id);

            if (Id.Equals(visitorsLog.StudentId.ToString()))
            {
                if (id == null)
                {
                    return NotFound();
                }

                if (visitorsLog == null)
                {
                    return NotFound();
                }
                return View(visitorsLog);
            }
            else
            {
                TempData["msg1"] = "<script>alert('ת.ז לא נמצאה במערכת');</script>";
                return RedirectToAction("Wellcome", "Home");
                
            }

        }
        // confirm the user idntitiy and change the record
        public async Task<ActionResult> ConifiremSigniture(int id, int StudentId , string password)
        {
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            string Id = HttpContext.Session.GetString("User");
            //user by session
            var u = await _context.Users.SingleOrDefaultAsync(m => m.StundetId.ToString().Equals(Id));
       

            ViewBag.StudentId = StudentId;
            var functions = new functions();
            //user by StudentId
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

                       // return RedirectToAction(nameof(Index));
                    }
                
            }
           return  RedirectToAction("Wellcom", "Home");





        }
        //check the visitor log if the guest stsed after 00:00 is send a message to the student that he need to sing on the guest
        public async Task<ActionResult> CheckVistorLog()
        {

            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
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

    }


}
