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

        List<ApprovalKit> NotAssigning = new List<ApprovalKit> { };

        public async Task<IActionResult> Assigning()
        {
           // List<ApprovalKit> NotAssigning = new List<ApprovalKit> { };
            //Couples ApprovalKit
            var Cop = from s in _context.ApprovalKits 
                         where s.RoomType.Equals("דירה_זוגית")
                         select s;
            List<ApprovalKit> Couples = new List<ApprovalKit> { };
            foreach (var c in Cop)
            {
                Couples.Add(c);
            }
            //Male ApprovalKit
            var Male = from s in _context.ApprovalKits
                      where s.RoomType.Equals("חדר_ליחיד") && s.Reg.gender.Equals("זכר") && s.HealthCondition.Equals("ללא_מגבלה")
                      select s;

            List<ApprovalKit> Males = new List<ApprovalKit> { };
            foreach (var m in Male)
            {
                Males.Add(m);
            }

            //Female ApprovalKit
            var Female = from s in _context.ApprovalKits
                       where s.RoomType.Equals("חדר_ליחיד") && s.Reg.gender.Equals("נקבה") && s.HealthCondition.Equals("ללא_מגבלה")
                       select s;

            List<ApprovalKit> Females = new List<ApprovalKit> { };
            foreach (var f in Female)
            {
                Females.Add(f);
            }

            var Acs = from s in _context.ApprovalKits
                      where s.RoomType.Equals("חדר_ליחיד") && s.HealthCondition.Equals("נכה_צהל") 
                      || s.HealthCondition.Equals("נכות")
                      || s.HealthCondition.Equals("מגבלה_פיזית_אחרת")
                      select s;

            List<ApprovalKit> Accessible = new List<ApprovalKit> { };
            foreach (var f in Acs)
            {
                Accessible.Add(f);
            }
            // single Apartments
            var SApartments = from s in _context.Apartments
                              where s.Type.Equals("Single") && s.Type.Equals("Accessible")
                              select s;

            //Couples

            foreach (ApprovalKit a in Couples)
            {
                //a main student || c main student parnter
                var c = await _context.ApprovalKits.SingleOrDefaultAsync(m => m.StundetId == a.PartnerId1.Value); //find the partner of the main student
                
                if (c.PartnerId1.Value==a.StundetId)
                {
                    //get empty apartment
                    var apartment = await _context.Apartments.SingleOrDefaultAsync(m => m.Type.Equals("Couples") && m.capacity==0);
                    apartment.capacity=2;
                    _context.Update(apartment);
                    //if there is no empty apartment
                    if (apartment==null)
                    {
                        NotAssigning.Add(c);
                        NotAssigning.Add(a);
                    }
                    // the user obj of a 
                    var u1 = await _context.Users.SingleOrDefaultAsync(m => m.StundetId == a.StundetId);
                    //the user obj of c
                    var u2 = await _context.Users.SingleOrDefaultAsync(m => m.StundetId == c.StundetId);

                    Assigning p1 = new Assigning
                    {
                        StundetId = a.StundetId.Value,
                        ApartmentNum = apartment.ApartmentNum,
                        User = u1,
                    };
                    var item1 = Couples.Single(x => x.StundetId == a.StundetId);
                    Couples.Remove(item1);
                    Assigning p2 = new Assigning
                    {
                        StundetId = c.StundetId.Value,
                        ApartmentNum = apartment.ApartmentNum,
                        User = u2,
                    };
                    var item2 = Couples.Single(x => x.StundetId == c.StundetId);
                    Couples.Remove(item2);
                    _context.Add(p1);
                    _context.Add(p2);
                    await _context.SaveChangesAsync();
                }
                //if there is no partner
                else
                {
                    NotAssigning.Add(a);
                };
            }
            //Accessible
            foreach (ApprovalKit a in Accessible)
            {
                ApprovalKit[] roomies = new ApprovalKit[3];
                roomies[0] = a;
                int size = 1;
                if (a.PartnerId1 != null)
                {
                    var c = await _context.ApprovalKits.SingleOrDefaultAsync(m => m.StundetId == a.PartnerId1.Value && m.Reg.gender == a.Reg.gender);
                    if (c.PartnerId1 == a.StundetId || c.PartnerId2 == a.StundetId)
                    {
                        roomies[1] = c;
                        size++;
                    }

                }
                if (a.PartnerId2 != null)
                {
                    var c = await _context.ApprovalKits.SingleOrDefaultAsync(m => m.StundetId == a.PartnerId2.Value && m.Reg.gender == a.Reg.gender);
                    if (c.PartnerId1 == a.StundetId || c.PartnerId2 == a.StundetId)
                    {
                        roomies[2] = c;
                        size++;
                    }
                }
                //change proprties of apartment
                var apartment = await _context.Apartments.SingleOrDefaultAsync(m => m.Type.Equals("Accessible") && m.capacity == 3);
                if (apartment != null)
                {
                    apartment.LivingWithReligious = a.LivingWithReligious;
                    apartment.LivingWithSmoker = a.LivingWithSmoker;
                    apartment.Gender = a.Reg.gender;
                    apartment.capacity = apartment.capacity-size;
                    _context.Update(apartment);

                    // save proprties of roomeis
                    foreach (ApprovalKit u in roomies)
                    {
                        int c = 1;
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
                            c++;

                            var item = Accessible.Single(x => x.StundetId == u.StundetId);
                            if (item != null)
                            {
                                Accessible.Remove(item);
                            }
                            else
                            {
                                if (a.Reg.gender.Equals("נקבה"))
                                {
                                    item = Females.Single(x => x.StundetId == u.StundetId);
                                    Females.Remove(item);
                                }
                                else
                                {
                                    item = Males.Single(x => x.StundetId == u.StundetId);
                                    Males.Remove(item);
                                }
                            }
                        }
                    }

                }
                else
                {
                    foreach (ApprovalKit u in roomies)
                    {
                        NotAssigning.Add(u);
                        var item = Accessible.Single(x => x.StundetId == u.StundetId);
                        if (item != null)
                        {
                            Accessible.Remove(item);
                        }
                        else
                        {
                            if (a.Reg.gender.Equals("נקבה"))
                            {
                                item = Females.Single(x => x.StundetId == u.StundetId);
                                Females.Remove(item);
                            }
                            else
                            {
                                item = Males.Single(x => x.StundetId == u.StundetId);
                                Males.Remove(item);
                            }
                        }
                    }
                }
            }
            //Single

            Single(Males);
            Single(Females);
            await _context.SaveChangesAsync();
            return View();
        }

        private async void Single(List<ApprovalKit> list)
        {
            //אחרי שסיימו לאחד את השותפים למערך לחפש דירה שמתאימה בגודל לכמות של המערך ולתנאים של האנשים.
            

            foreach (ApprovalKit a in list)
            {
                ApprovalKit[] roomies = new ApprovalKit[4];
                roomies[0] = a;
                int size = 1;

                if (a.PartnerId1 != null)
                {
                    var c = await _context.ApprovalKits.SingleOrDefaultAsync(m => m.StundetId == a.PartnerId1.Value && m.Reg.gender == a.Reg.gender);
                    if (c.PartnerId1 == a.StundetId || c.PartnerId2 == a.StundetId|| c.PartnerId3 == a.StundetId)
                    {
                        roomies[1] = c;
                        size++;
                    }

                }
                if (a.PartnerId2 != null)
                {
                    var c = await _context.ApprovalKits.SingleOrDefaultAsync(m => m.StundetId == a.PartnerId2.Value && m.Reg.gender == a.Reg.gender);
                    if (c.PartnerId1 == a.StundetId || c.PartnerId2 == a.StundetId || c.PartnerId3 == a.StundetId)
                    {
                        roomies[2] = c;
                        size++;
                    }

                }
                if (a.PartnerId3 != null)
                {
                    var c = await _context.ApprovalKits.SingleOrDefaultAsync(m => m.StundetId == a.PartnerId3.Value && m.Reg.gender == a.Reg.gender);
                    if (c.PartnerId1 == a.StundetId || c.PartnerId2 == a.StundetId || c.PartnerId3 == a.StundetId)
                    {
                        roomies[3] = c;
                        size++;
                    }

                }
                //capacity- how many spots opne
                var apartment = await _context.Apartments.SingleOrDefaultAsync(m =>( m.Type.Equals("Single") || m.Type.Equals("Accessible"))
                && m.capacity >= size && m.Gender.Equals(a.Reg.gender)&& m.LivingWithReligious.Equals(a.LivingWithReligious)
                && m.LivingWithSmoker.Equals(a.LivingWithSmoker)&& m.ReligiousType.Equals(a.ReligiousType));
                if (apartment != null)
                {
                    apartment.LivingWithReligious = a.LivingWithReligious;
                    apartment.LivingWithSmoker = a.LivingWithSmoker;
                    apartment.Gender = a.Reg.gender;
                    apartment.capacity = apartment.capacity - size;


                    foreach (ApprovalKit u in roomies)
                    {
                        int c = 1;
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
                            c++;

                            var item = list.Single(x => x.StundetId == u.StundetId);
                            list.Remove(item);


                        }
                    }
                }
                else
                {
                    foreach (ApprovalKit u in roomies)
                    {
                        NotAssigning.Add(u);
                        var item = list.Single(x => x.StundetId == u.StundetId);
                        list.Remove(item);

                    }
                }

            }

        }
    }


}
