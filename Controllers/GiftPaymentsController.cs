using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gifts_Store_First_project.Models;

namespace Gifts_Store_First_project.Controllers
{
    public class GiftPaymentsController : Controller
    {
        private readonly ModelContext _context;

        public GiftPaymentsController(ModelContext context)
        {
            _context = context;
        }

        // GET: GiftPayments
        public async Task<IActionResult> Index()
        {
              return _context.GiftPayments != null ? 
                          View(await _context.GiftPayments.ToListAsync()) :
                          Problem("Entity set 'ModelContext.GiftPayments'  is null.");
        }

        // GET: GiftPayments/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.GiftPayments == null)
            {
                return NotFound();
            }

            var giftPayment = await _context.GiftPayments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftPayment == null)
            {
                return NotFound();
            }

            return View(giftPayment);
        }

        // GET: GiftPayments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GiftPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CardNumber,Cvv,ExpiryDate,Balance,Id")] GiftPayment giftPayment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giftPayment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(giftPayment);
        }

        // GET: GiftPayments/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.GiftPayments == null)
            {
                return NotFound();
            }

            var giftPayment = await _context.GiftPayments.FindAsync(id);
            if (giftPayment == null)
            {
                return NotFound();
            }
            return View(giftPayment);
        }

        // POST: GiftPayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("CardNumber,Cvv,ExpiryDate,Balance,Id")] GiftPayment giftPayment)
        {
            if (id != giftPayment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giftPayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiftPaymentExists(giftPayment.Id))
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
            return View(giftPayment);
        }

        // GET: GiftPayments/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.GiftPayments == null)
            {
                return NotFound();
            }

            var giftPayment = await _context.GiftPayments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftPayment == null)
            {
                return NotFound();
            }

            return View(giftPayment);
        }

        // POST: GiftPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.GiftPayments == null)
            {
                return Problem("Entity set 'ModelContext.GiftPayments'  is null.");
            }
            var giftPayment = await _context.GiftPayments.FindAsync(id);
            if (giftPayment != null)
            {
                _context.GiftPayments.Remove(giftPayment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiftPaymentExists(decimal id)
        {
          return (_context.GiftPayments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
