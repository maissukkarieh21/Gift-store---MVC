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
    public class GiftContactsController : Controller
    {
        private readonly ModelContext _context;

        public GiftContactsController(ModelContext context)
        {
            _context = context;
        }

        // GET: GiftContacts
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.GiftContacts.Include(g => g.Home);
            return View(await modelContext.ToListAsync());
        }

        // GET: GiftContacts/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.GiftContacts == null)
            {
                return NotFound();
            }

            var giftContact = await _context.GiftContacts
                .Include(g => g.Home)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftContact == null)
            {
                return NotFound();
            }

            return View(giftContact);
        }

        // GET: GiftContacts/Create
        public IActionResult Create()
        {
            ViewData["HomeId"] = new SelectList(_context.GiftHomes, "Id", "Id");
            return View();
        }

        // POST: GiftContacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PhoneNumber,Location,Email")] GiftContact giftContact)
        {
            if (ModelState.IsValid)
            {
                giftContact.HomeId = 1;
                _context.Add(giftContact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HomeId"] = new SelectList(_context.GiftHomes, "Id", "Id", giftContact.HomeId);
            return View(giftContact);
        }

        // GET: GiftContacts/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.GiftContacts == null)
            {
                return NotFound();
            }

            var giftContact = await _context.GiftContacts.FindAsync(id);
            if (giftContact == null)
            {
                return NotFound();
            }
            ViewData["HomeId"] = new SelectList(_context.GiftHomes, "Id", "Id", giftContact.HomeId);
            return View(giftContact);
        }

        // POST: GiftContacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,PhoneNumber,Location,Email")] GiftContact giftContact)
        {
            if (id != giftContact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    giftContact.HomeId = 1;
                    _context.Update(giftContact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiftContactExists(giftContact.Id))
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
            ViewData["HomeId"] = new SelectList(_context.GiftHomes, "Id", "Id", giftContact.HomeId);
            return View(giftContact);
        }

        // GET: GiftContacts/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.GiftContacts == null)
            {
                return NotFound();
            }

            var giftContact = await _context.GiftContacts
                .Include(g => g.Home)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftContact == null)
            {
                return NotFound();
            }

            return View(giftContact);
        }

        // POST: GiftContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.GiftContacts == null)
            {
                return Problem("Entity set 'ModelContext.GiftContacts'  is null.");
            }
            var giftContact = await _context.GiftContacts.FindAsync(id);
            if (giftContact != null)
            {
                _context.GiftContacts.Remove(giftContact);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiftContactExists(decimal id)
        {
          return (_context.GiftContacts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
