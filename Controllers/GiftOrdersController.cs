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
    public class GiftOrdersController : Controller
    {
        private readonly ModelContext _context;

        public GiftOrdersController(ModelContext context)
        {
            _context = context;
        }

        // GET: GiftOrders
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.GiftOrders.Include(g => g.Gift).Include(g => g.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: GiftOrders/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.GiftOrders == null)
            {
                return NotFound();
            }

            var giftOrder = await _context.GiftOrders
                .Include(g => g.Gift)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftOrder == null)
            {
                return NotFound();
            }

            return View(giftOrder);
        }

        // GET: GiftOrders/Create
        public IActionResult Create()
        {
            ViewData["GiftId"] = new SelectList(_context.GiftGifts, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.GiftUsers, "Id", "Id");
            return View();
        }

        // POST: GiftOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Status,PhoneNumber,AdminProfits,MakerProfits,OrderDate,UserId,GiftId,Id")] GiftOrder giftOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giftOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GiftId"] = new SelectList(_context.GiftGifts, "Id", "Id", giftOrder.GiftId);
            ViewData["UserId"] = new SelectList(_context.GiftUsers, "Id", "Id", giftOrder.UserId);
            return View(giftOrder);
        }

        // GET: GiftOrders/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.GiftOrders == null)
            {
                return NotFound();
            }

            var giftOrder = await _context.GiftOrders.FindAsync(id);
            if (giftOrder == null)
            {
                return NotFound();
            }
            ViewData["GiftId"] = new SelectList(_context.GiftGifts, "Id", "Id", giftOrder.GiftId);
            ViewData["UserId"] = new SelectList(_context.GiftUsers, "Id", "Id", giftOrder.UserId);
            return View(giftOrder);
        }

        // POST: GiftOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Status,PhoneNumber,AdminProfits,MakerProfits,OrderDate,UserId,GiftId,Id")] GiftOrder giftOrder)
        {
            if (id != giftOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giftOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiftOrderExists(giftOrder.Id))
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
            ViewData["GiftId"] = new SelectList(_context.GiftGifts, "Id", "Id", giftOrder.GiftId);
            ViewData["UserId"] = new SelectList(_context.GiftUsers, "Id", "Id", giftOrder.UserId);
            return View(giftOrder);
        }

        // GET: GiftOrders/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.GiftOrders == null)
            {
                return NotFound();
            }

            var giftOrder = await _context.GiftOrders
                .Include(g => g.Gift)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftOrder == null)
            {
                return NotFound();
            }

            return View(giftOrder);
        }

        // POST: GiftOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.GiftOrders == null)
            {
                return Problem("Entity set 'ModelContext.GiftOrders'  is null.");
            }
            var giftOrder = await _context.GiftOrders.FindAsync(id);
            if (giftOrder != null)
            {
                _context.GiftOrders.Remove(giftOrder);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiftOrderExists(decimal id)
        {
          return (_context.GiftOrders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
