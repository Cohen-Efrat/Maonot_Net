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
    public class ApartmentsController : Controller
    {
        private readonly MaonotNetContext _context;

        public ApartmentsController(MaonotNetContext context)
        {
            _context = context;
        }

        // GET: Apartments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Apartments.ToListAsync());
        }

        // GET: Apartments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartments = await _context.Apartments
                .SingleOrDefaultAsync(m => m.ID == id);
            if (apartments == null)
            {
                return NotFound();
            }

            return View(apartments);
        }

        // GET: Apartments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Apartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ApartmentNum,Type,LivingWithReligious,LivingWithSmoker,ReligiousType,Gender,capacity")] Apartments apartments)
        {
            if (ModelState.IsValid)
            {
                _context.Add(apartments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(apartments);
        }

        // GET: Apartments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartments = await _context.Apartments.SingleOrDefaultAsync(m => m.ID == id);
            if (apartments == null)
            {
                return NotFound();
            }
            return View(apartments);
        }

        // POST: Apartments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ApartmentNum,Type,LivingWithReligious,LivingWithSmoker,ReligiousType,Gender,capacity")] Apartments apartments)
        {
            if (id != apartments.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apartments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApartmentsExists(apartments.ID))
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
            return View(apartments);
        }

        // GET: Apartments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartments = await _context.Apartments
                .SingleOrDefaultAsync(m => m.ID == id);
            if (apartments == null)
            {
                return NotFound();
            }

            return View(apartments);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apartments = await _context.Apartments.SingleOrDefaultAsync(m => m.ID == id);
            _context.Apartments.Remove(apartments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApartmentsExists(int id)
        {
            return _context.Apartments.Any(e => e.ID == id);
        }


        //capacity כמה מקום פנוי יש
        public async Task<IActionResult> Assigning()
        {
            //Couples ApprovalKit

            List<ApprovalKit> Couples = _context.ApprovalKits.Where(
                a =>
                a.RoomType == RoomType.דירה_זוגית).ToList();

            //Male ApprovalKit
            List<ApprovalKit> Males = _context.ApprovalKits.Where(
                r =>
                r.RoomType == RoomType.חדר_ליחיד &&
                r.HealthCondition == HealthCondition.ללא_מגבלה &&
                r.Gender == Gender.זכר
                ).ToList();

            //Female ApprovalKit
            List<ApprovalKit> Females = _context.ApprovalKits.Where(
                r =>
                r.RoomType == RoomType.חדר_ליחיד &&
                r.HealthCondition == HealthCondition.ללא_מגבלה &&
                r.Gender == Gender.נקבה
                ).ToList();

            //Accessible ApprovalKit
            List<ApprovalKit> Accessible = _context.ApprovalKits.Where(
                    r =>
                    r.RoomType == RoomType.חדר_ליחיד &&(
                    r.HealthCondition == HealthCondition.מגבלה_פיזית_אחרת ||
                    r.HealthCondition == HealthCondition.נכה_צהל ||
                    r.HealthCondition == HealthCondition.נכות)
                    ).ToList();


            //Couples

            foreach (ApprovalKit a in Couples)
            {
                var asaing = await _context.Assigning.SingleOrDefaultAsync(u => u.StundetId.Value == a.StundetId.Value);
                if (asaing == null)
                {
                    //a main student || c main student parnter
                    if (a.PartnerId1 != null)
                    {
                        var c = await _context.ApprovalKits.SingleOrDefaultAsync(m => m.StundetId.Value == a.PartnerId1.Value); //find the partner of the main student
                        if (c != null)
                        {
                            if (c.PartnerId1.Value == a.StundetId.Value)
                            {
                                //get empty apartment
                                var apartment = await _context.Apartments.FirstOrDefaultAsync(m => m.Type.Equals("Couples") && m.capacity == 2);
                                apartment.capacity = 0;
                                _context.Update(apartment);
                                //if there is no empty apartment
                                if (apartment == null)
                                {
                                    var temp = Globals.NotAssigning.Find(x => x.ID == c.ID);
                                    if (temp == null)
                                    {
                                        Globals.NotAssigning.Add(c);
                                    }
                                    temp = Globals.NotAssigning.Find(x => x.ID == a.ID);
                                    if (temp == null)
                                    {
                                        Globals.NotAssigning.Add(a);
                                    }
                                }
                                // the user obj of a 
                                var u1 = await _context.Users.SingleOrDefaultAsync(m => m.StundetId == a.StundetId);
                                //the user obj of c
                                var u2 = await _context.Users.SingleOrDefaultAsync(m => m.StundetId == c.StundetId);

                                Assigning p1 = new Assigning
                                {
                                    StundetId = a.StundetId.Value,
                                    ApartmentNum = apartment.ApartmentNum,
                                    User = u1,// if not workig need to update the user also
                                };
                                //var item1 = Couples.Single(x => x.StundetId == a.StundetId);
                                // Couples.Remove(item1);
                                Assigning p2 = new Assigning
                                {
                                    StundetId = c.StundetId.Value,
                                    ApartmentNum = apartment.ApartmentNum,
                                    User = u2,
                                };
                                // var item2 = Couples.Single(x => x.StundetId == c.StundetId);
                                // Couples.Remove(item2);
                                _context.Add(p1);
                                _context.Add(p2);
                                await _context.SaveChangesAsync();
                            }
                        }
                        //if there is no partner


                    }
                    else
                    {
                        var temp = Globals.NotAssigning.Find(x => x.ID == a.ID);
                        if (temp == null)
                        {
                            Globals.NotAssigning.Add(a);
                        }

                    };
                }
            }
            //Accessible
            foreach (ApprovalKit a in Accessible)
            {
                var asaing = await _context.Assigning.SingleOrDefaultAsync(u => u.StundetId.Value == a.StundetId.Value);
                if (asaing == null)
                {
                    ApprovalKit[] roomies = new ApprovalKit[3];
                    roomies[0] = a;
                    int size = 1;
                    if (a.PartnerId1 != null)
                    {
                        var c = await _context.ApprovalKits.SingleOrDefaultAsync(m => m.StundetId.Value == a.PartnerId1.Value && m.Gender == a.Gender);
                        if (c.PartnerId1.Value == a.StundetId.Value || c.PartnerId2.Value == a.StundetId.Value)
                        {
                            roomies[1] = c;
                            size++;
                        }

                    }
                    if (a.PartnerId2 != null)
                    {
                        var c = await _context.ApprovalKits.SingleOrDefaultAsync(m => m.StundetId == a.PartnerId2.Value && m.Gender == a.Gender);
                        if (c.PartnerId1.Value == a.StundetId.Value || c.PartnerId2.Value == a.StundetId.Value)
                        {
                            roomies[2] = c;
                            size++;
                        }
                    }
                    //change proprties of apartment
                    var apartment = await _context.Apartments.FirstOrDefaultAsync(m => m.Type.Equals("Accessible") && m.capacity == 3);
                    if (apartment != null)
                    {
                        apartment.LivingWithReligious = a.LivingWithReligious;
                        apartment.LivingWithSmoker = a.LivingWithSmoker;
                        apartment.Gender = a.Gender;
                        apartment.capacity = apartment.capacity - size;
                        _context.Update(apartment);
                        await _context.SaveChangesAsync();

                        int c = 3;
                        // save proprties of roomeis
                        foreach (ApprovalKit u in roomies)
                        {
                            if (u != null)
                            {
                                var user = await _context.Users.SingleOrDefaultAsync(m => m.StundetId == u.StundetId.Value);
                                Assigning r = new Assigning
                                {
                                    StundetId = u.StundetId.Value,
                                    ApartmentNum = apartment.ApartmentNum,
                                    Room = c,
                                    User = user
                                };
                                
                                _context.Add(r);
                                await _context.SaveChangesAsync();
                                c--;
                            }
                        }

                    }
                    else
                    {
                        foreach (ApprovalKit u in roomies)
                        {
                            if (u != null)
                            {
                                var temp = Globals.NotAssigning.Find(x => x.ID == u.ID);
                                if (temp == null)
                                {
                                    Globals.NotAssigning.Add(u);
                                }
                            }
                        }
                    }
                }
            }
            //Single
            string male = "m";
            string female = "w";
            Single(male);
            Single(female);
            //await _context.SaveChangesAsync();
            ViewBag.NotAssigning = Globals.NotAssigning;

            return RedirectToAction("NotAssigning", "Apartments");
        }
        public IActionResult NotAssigning()
        {
            List<User> users = _context.Users.Where(r =>r.ApartmentNum == null&&r.Authorization==9).ToList();
            foreach (User u in users)
            {
                var item = _context.ApprovalKits.SingleOrDefault(a => a.StundetId.Value == u.StundetId);
                var temp = Globals.NotAssigning.Find(x => x.StundetId == u.StundetId);
                if (temp == null)
                {
                    Globals.NotAssigning.Add(item);
                }
            }

            ViewBag.NotAssigning = Globals.NotAssigning;
            return View();
        }

        private async void Single(string gender)
        {
            List<ApprovalKit> list = new List<ApprovalKit> { };
            if (gender.Equals("m"))
            {
               list = _context.ApprovalKits.Where(
               r =>
               r.RoomType == RoomType.חדר_ליחיד &&
               r.HealthCondition == HealthCondition.ללא_מגבלה &&
               r.Gender == Gender.זכר
               ).ToList();
            }
            if(gender.Equals("w"))
            {
                list = _context.ApprovalKits.Where(
                    r =>
                    r.RoomType == RoomType.חדר_ליחיד &&
                    r.HealthCondition == HealthCondition.ללא_מגבלה &&
                    r.Gender == Gender.נקבה
                    ).ToList();
            }
            foreach (ApprovalKit a in list)
            {
                var asaing = await _context.Assigning.SingleOrDefaultAsync(u => u.StundetId.Value == a.StundetId.Value);
                if (asaing == null)
                {
                    ApprovalKit[] roomies = new ApprovalKit[4];
                    roomies[0] = a;
                    int size = 1;

                    if (a.PartnerId1 != null)
                    {
                        var c = await _context.ApprovalKits.SingleOrDefaultAsync(m => m.StundetId.Value == a.PartnerId1.Value && m.Gender == a.Gender);
                        if (c.PartnerId1.Value == a.StundetId.Value || c.PartnerId2.Value == a.StundetId.Value || c.PartnerId3.Value == a.StundetId.Value)
                        {
                            roomies[1] = c;
                            size++;
                        }

                    }
                    if (a.PartnerId2 != null)
                    {
                        var c = await _context.ApprovalKits.SingleOrDefaultAsync(m => m.StundetId.Value == a.PartnerId2.Value && m.Gender == a.Gender);
                        if (c.PartnerId1.Value == a.StundetId.Value || c.PartnerId2.Value == a.StundetId.Value || c.PartnerId3.Value == a.StundetId.Value)
                        {
                            roomies[2] = c;
                            size++;
                        }

                    }
                    if (a.PartnerId3 != null)
                    {
                        var c = await _context.ApprovalKits.SingleOrDefaultAsync(m => m.StundetId == a.PartnerId3.Value && m.Gender == a.Gender);
                        if (c.PartnerId1.Value == a.StundetId.Value || c.PartnerId2.Value == a.StundetId.Value || c.PartnerId3.Value == a.StundetId.Value)
                        {
                            roomies[3] = c;
                            size++;
                        }

                    }
                    //capacity- how many spots opne
                    //  Apartments apartment2 = await _context.Apartments.FirstOrDefaultAsync(m => m.capacity >= size && (m.Type.Equals("Single") || m.Type.Equals("Accessible"))
                    // && m.LivingWithReligious.Equals(a.LivingWithReligious) && m.LivingWithSmoker.Equals(a.LivingWithSmoker)
                    // && m.ReligiousType.Equals(a.ReligiousType) && m.Gender.ToString().Equals(a.Gender.ToString()));


                    Apartments apartment = await _context.Apartments.FirstOrDefaultAsync(m => m.capacity >= size &&
                    m.Type.Equals("Single") &&
                                             m.LivingWithReligious == a.LivingWithReligious &&
                                             m.LivingWithSmoker == a.LivingWithSmoker &&
                                             m.ReligiousType == a.ReligiousType &&
                                             m.Gender == a.Gender
                    );

                    if (apartment != null)
                    {
                        apartment.LivingWithReligious = a.LivingWithReligious;
                        apartment.LivingWithSmoker = a.LivingWithSmoker;
                        apartment.Gender = a.Gender;
                        apartment.capacity = apartment.capacity - size;
                        _context.Update(apartment);
                        

                        foreach (ApprovalKit u in roomies)
                        {
                            int c = 4;
                            if (u != null)
                            {
                                var user = await _context.Users.SingleOrDefaultAsync(m => m.StundetId == u.StundetId);
                                Assigning r = new Assigning
                                {
                                    StundetId = u.StundetId.Value,
                                    ApartmentNum = apartment.ApartmentNum,
                                    Room = c,
                                    User = user
                                };
                                _context.Add(r);
                                
                                c--;
                  }
                        }
                        
                    }
                
                    else
                    {
                        foreach (ApprovalKit u in roomies)
                        {
                            if (u != null)
                            {
                                var temp = Globals.NotAssigning.Find(x => x.ID == u.ID);
                                if (temp == null)
                                {
                                    Globals.NotAssigning.Add(u);
                                }
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                }

            }
            }


        }
    }

