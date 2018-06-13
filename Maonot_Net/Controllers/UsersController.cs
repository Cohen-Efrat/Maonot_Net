using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Maonot_Net.Data;
using Maonot_Net.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using BCrypt.Net;

namespace Maonot_Net.Controllers
{
    public class UsersController : Controller
    {
        private readonly MaonotNetContext _context;

        public UsersController(MaonotNetContext context)
        {
            _context = context;
        }

        // GET: Users
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

            var users = from s in _context.Users
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(u => u.LastName);
                    break;
                case "fname_desc":
                    users = users.OrderByDescending(u => u.FirstName);
                    break;
                case "fname":
                    users = users.OrderBy(u => u.FirstName);
                    break;
                default:
                    users = users.OrderBy(U => U.LastName);
                    break;
            }
            int pageSize = 3;

            return View(await PaginatedList<User>.CreateAsync(users.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StundetId,FirstName,LastName,Password,Email,ApartmentNum,Room")] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user.Password =
                         BCrypt.Net.BCrypt.HashPassword(user.Password);

                    //user.Password = HashPassword(user.Password);
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(DbUpdateException)
            {
                ModelState.AddModelError("", "לא היה ניתן לשמור את השינויים, נא נסה שנית במועד מאוחר יותר");

            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,StundetId,FirstName,LastName,Password,Email,ApartmentNum,Room")] User user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id,bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["EErrorMessage"] = "המחיקה נכשלה, נא נסה שנית במועד מאוחד יותר";
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.AsNoTracking().SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Index), new { id = id, saveCahngeError = true });
            }

        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.ID == id);
        }
        // bool validPassword = BCrypt.Net.BCrypt.Verify(submittedPassword, hashedPassword);
        // http://davismj.me/blog/bcrypt
        //https://github.com/BcryptNet/bcrypt.net
        private bool CheckPassword(string submittedPassword, string hashedPassword)
        {

            bool validPassword = BCrypt.Net.BCrypt.Verify(submittedPassword, hashedPassword);
            return validPassword;
        }

     //   public IActionResult LogIn()
        //{

       //     return View();
      //  }

        public async Task<IActionResult> LogIn(User _user)
        {
            var user = await _context.Users.SingleOrDefaultAsync(m => m.StundetId == _user.StundetId);
            if (user!=null)
            {
                if (CheckPassword(_user.Password ,user.Password))
                {
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    TempData["msg"] = "<script>alert('Password was incorrect');</script>";
                    return View();
                }
            }
            else
            {
                TempData["msg1"] = "<script>alert('E-mail not Found');</script>";
                return View();
            }

        }

           
    }




    }

