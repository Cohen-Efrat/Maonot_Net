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
    public class MessagesController : Controller
    {
        private readonly MaonotNetContext _context;

        public MessagesController(MaonotNetContext context)
        {
            _context = context;
        }

        // GET: Messages
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            string Aut = HttpContext.Session.GetString("Aut");
            string Id = HttpContext.Session.GetString("User");
           // var u = await _context.Users.SingleOrDefaultAsync(m => m.StundetId.Equals(302875125));
            if (!Aut.Equals("0"))
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

                var msg = from s in _context.Messages
                          where s.Addressee.Equals(Id) || s.Addressee.Equals("All")
                          select s;

                if (!String.IsNullOrEmpty(searchString))
                {
                    msg = msg.Where(s => s.Subject.Contains(searchString));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        msg = msg.OrderByDescending(s => s.Subject);
                        break;

                    default:
                        msg = msg.OrderBy(s => s.Subject);
                        break;
                }

                int pageSize = 3;
                return View(await PaginatedList<Message>.CreateAsync(msg.AsNoTracking(), page ?? 1, pageSize));
            }
            return RedirectToAction("NotAut", "Home");
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string Aut = HttpContext.Session.GetString("Aut");
            string Id = HttpContext.Session.GetString("User");
            var u = await _context.Users.SingleOrDefaultAsync(m => m.StundetId.ToString().Equals(Id));
            if (!Aut.Equals("0"))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var message = await _context.Messages
                    .SingleOrDefaultAsync(m => m.MessageID == id);
                if (message == null)
                {
                    return NotFound();
                }
                if (u.StundetId.ToString().Equals(message.Addressee))
                {
                    return View(message);
                }

                
            }
            return RedirectToAction("NotAut", "Home");
        }


        // GET: Messages/Create
        public IActionResult Create()
        {

            string Aut = HttpContext.Session.GetString("Aut");
            if (!Aut.Equals("0"))
            {
                ViewData["users"] = new SelectList(_context.Users, "StundetId", "FullName");
                return View();
            }
            return RedirectToAction("NotAut", "Home");

        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Addressee,Subject,Content")] Message message)
        {
            string Id = HttpContext.Session.GetString("User");
            var u = await _context.Users.SingleOrDefaultAsync(m => m.StundetId.ToString().Equals(Id));

            try
            {
                message.From = u.FullName;
                message.MsgTime = DateTime.Now;
                if (ModelState.IsValid)
                {
                    _context.Add(message);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "לא היה ניתן לשמור את השינויים, נא נסה שנית במועד מאוחר יותר");

            }
            return View(message);
        }

        // GET: Messages/Edit/5
        //no edit option
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages.SingleOrDefaultAsync(m => m.MessageID == id);
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MessageID,Addressee,Subject,Content")] Message message)
        {
            if (id != message.MessageID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.MessageID))
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
            return View(message);
        }
        //no Delete option
        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages.AsNoTracking()
                .SingleOrDefaultAsync(m => m.MessageID == id);
            if (message == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["EErrorMessage"] = "המחיקה נכשלה, נא נסה שנית במועד מאוחד יותר";
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Messages.AsNoTracking().SingleOrDefaultAsync(m => m.MessageID == id);
            if (message == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Index), new { id = id, saveCahngeError = true });
            }
        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.MessageID == id);
        }

        public IActionResult SendAll()
        {
            string Aut = HttpContext.Session.GetString("Aut");
            if (Aut.Equals("1")|| Aut.Equals("2") || Aut.Equals("3") || Aut.Equals("4") || Aut.Equals("5") )
            {
                return View();
            }
            return RedirectToAction("NotAut", "Home");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendAll([Bind("Subject,Content")] Message message)
        {
            string Id = HttpContext.Session.GetString("User");
            var u = await _context.Users.SingleOrDefaultAsync(m => m.StundetId.ToString().Equals(Id));

            try
            {
                if (message.Subject != null && message.Content != null)
                {
                    message.Addressee = "All";
                    message.From = u.FullName;
                    message.MsgTime = DateTime.Now;
                    _context.Add(message);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "לא היה ניתן לשמור את השינויים, נא נסה שנית במועד מאוחר יותר");

            }
            return View(message);
            
        }


    }
}
