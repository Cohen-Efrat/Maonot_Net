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
    public class AssigningsController : Controller
    {
        private readonly MaonotNetContext _context;

        public AssigningsController(MaonotNetContext context)
        {
            _context = context;
        }

        // GET: Assignings
        // return a list of al the assigning 
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            string Aut = HttpContext.Session.GetString("Aut");

            if (!Aut.Equals("2"))
            {
                return RedirectToAction("NotAut", "Home");
            }
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
                a = a.Where(s=> s.User.StundetId.ToString().Contains(searchString));
                                   
            }
            switch (sortOrder)
            {
                case "ID_desc":
                    a = a.OrderByDescending(s => s.StundetId);
                    break;
                case "ID":
                    a = a.OrderBy(s => s.StundetId);
                    break;
                case "apartment_desc":
                    a = a.OrderByDescending(s => s.ApartmentNum);
                    break;
                default:
                    a = a.OrderBy(s => s.ApartmentNum);
                    break;
            }

            ViewBag.c = a.Count();
            int pageSize = 10;

            return View(await PaginatedList<Assigning>.CreateAsync(a.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Assignings/Delete/5
        // get the details of the recoerd and ask the user if he is sure
        public async Task<IActionResult> Delete(int? id)
        {
            string Aut = HttpContext.Session.GetString("Aut");

            if (!Aut.Equals("2"))
            {
                return RedirectToAction("NotAut", "Home");
            }
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
        // if the user confirem the delete this function start and delete the record from the DB
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string Aut = HttpContext.Session.GetString("Aut");

            if (!Aut.Equals("2"))
            {
                return RedirectToAction("NotAut", "Home");
            }
            var assigning = await _context.Assigning.SingleOrDefaultAsync(m => m.ID == id);
            User user = await _context.Users.SingleOrDefaultAsync(u => u.StundetId == assigning.StundetId.Value);
            var apartment = await _context.Apartments.SingleOrDefaultAsync(a => a.ApartmentNum == assigning.ApartmentNum.Value);

            user.ApartmentNum = null;
            user.Room = null;
            apartment.capacity = apartment.capacity + 1;
            _context.Update(apartment);

            _context.Update(user);
            _context.Assigning.Remove(assigning);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssigningExists(int id)
        {
            return _context.Assigning.Any(e => e.ID == id);
        }
        // return the approval kit by id,
        //return all the apartment that relevant to the approval kit 
        // return the view with the assigning form 
        public async Task<IActionResult> Change(int id)
        {
            string Aut = HttpContext.Session.GetString("Aut");

            if (!Aut.Equals("2"))
            {
                return RedirectToAction("NotAut", "Home");
            }

            var approvalKit = await _context.ApprovalKits.SingleOrDefaultAsync(m => m.ID == id);
            ViewBag.app = approvalKit;
            List<Apartments> apartment = new List<Apartments> { };
            if (approvalKit.RoomType.Equals(RoomType.חדר_ליחיד))
            {
                var apartments = from s in _context.Apartments
                                 where (s.Gender.Equals(approvalKit.Gender) && (s.Type.Equals("Single") || s.Type.Equals("Accessible"))
                                 && s.LivingWithSmoker.Equals(approvalKit.LivingWithSmoker) && s.LivingWithReligious.Equals(approvalKit.LivingWithSmoker)
                                 && s.ReligiousType.Equals(approvalKit.ReligiousType) && s.capacity > 0) || s.capacity == 4
                                 select s;
                foreach (Apartments a in apartments)
                {
                    apartment.Add(a);

                };
                ViewBag.apartment = apartment;
            }
            else
            {
                var apartments = from s in _context.Apartments
                                 where s.capacity > 0 && s.Type.Equals("Couples")
                                 select s;
                foreach (Apartments a in apartments)
                {
                    apartment.Add(a);
                };
                ViewBag.apartment = apartment;
            };

            ViewData["room"]= new SelectList(apartment, "ApartmentNum", "ApartmentNum");

            return View();
        }
        // after submintig the assainging form this function change the user field, 
        //creating ner record of assinging and chacnge the apartment capacity.
        public async Task<IActionResult> ChangeA(Assigning assigning)
        {
            string Aut = HttpContext.Session.GetString("Aut");

            if (!Aut.Equals("2"))
            {
                return RedirectToAction("NotAut", "Home");
            }
            var user = await _context.Users.SingleOrDefaultAsync(u => u.StundetId == assigning.StundetId.Value);
            var apartment = await _context.Apartments.SingleOrDefaultAsync(a => a.ApartmentNum == assigning.ApartmentNum.Value);

            if (ModelState.IsValid)
            {
                _context.Add(assigning);
                user.ApartmentNum = assigning.ApartmentNum.Value;
                if (assigning.Room==0)
                {
                    user.Room = RoomNum.OneA;
                }
                else if (assigning.Room == 1)
                {
                    user.Room = RoomNum.TwoA;
                }
                else if (assigning.Room == 2)
                {
                    user.Room = RoomNum.ThreeA;
                }
                else
                {
                    user.Room = RoomNum.FourA;
                }
                _context.Update(user);
                apartment.capacity = apartment.capacity - 1;
                _context.Update(apartment);
                await _context.SaveChangesAsync();
               
            }


            return RedirectToAction("NotAssigning", "Apartments");
        }

    }
}
