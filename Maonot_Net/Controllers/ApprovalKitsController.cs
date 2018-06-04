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
    public class ApprovalKitsController : Controller
    {
        private readonly MaonotNetContext _context;

        public ApprovalKitsController(MaonotNetContext context)
        {
            _context = context;
        }

        // GET: ApprovalKits
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApprovalKits.ToListAsync());
        }

        // GET: ApprovalKits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalKit = await _context.ApprovalKits
                .SingleOrDefaultAsync(m => m.ID == id);
            if (approvalKit == null)
            {
                return NotFound();
            }

            return View(approvalKit);
        }

        // GET: ApprovalKits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApprovalKits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StundetId,LastName,FirstName,RoomType,LivingWithReligious,LivingWithSmoker,ReligiousType,HealthCondition,PartnerId1,PartnerId2,PartnerId3,PartnerId4")] ApprovalKit approvalKit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(approvalKit);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "לא היה ניתן לשמור את השינויים, נא נסה שנית במועד מאוחר יותר");

            }
            return View(approvalKit);
        }

        // GET: ApprovalKits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalKit = await _context.ApprovalKits.SingleOrDefaultAsync(m => m.ID == id);
            if (approvalKit == null)
            {
                return NotFound();
            }
            return View(approvalKit);
        }

        // POST: ApprovalKits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,StundetId,LastName,FirstName,RoomType,LivingWithReligious,LivingWithSmoker,ReligiousType,HealthCondition,PartnerId1,PartnerId2,PartnerId3,PartnerId4")] ApprovalKit approvalKit)
        {
            if (id != approvalKit.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(approvalKit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApprovalKitExists(approvalKit.ID))
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
            return View(approvalKit);
        }

        // GET: ApprovalKits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalKit = await _context.ApprovalKits
                .SingleOrDefaultAsync(m => m.ID == id);
            if (approvalKit == null)
            {
                return NotFound();
            }

            return View(approvalKit);
        }

        // POST: ApprovalKits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var approvalKit = await _context.ApprovalKits.SingleOrDefaultAsync(m => m.ID == id);
            _context.ApprovalKits.Remove(approvalKit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApprovalKitExists(int id)
        {
            return _context.ApprovalKits.Any(e => e.ID == id);
        }
    }
}
