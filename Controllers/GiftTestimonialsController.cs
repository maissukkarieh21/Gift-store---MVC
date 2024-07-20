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
    public class GiftTestimonialsController : Controller
    {
        private readonly ModelContext _context;

        public GiftTestimonialsController(ModelContext context)
        {
            _context = context;
        }

        // GET: GiftTestimonials
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.GiftTestimonials.Include(g => g.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: GiftTestimonials/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.GiftTestimonials == null)
            {
                return NotFound();
            }

            var giftTestimonial = await _context.GiftTestimonials
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftTestimonial == null)
            {
                return NotFound();
            }

            return View(giftTestimonial);
        }

        // GET: GiftTestimonials/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.GiftUsers, "Id", "Id");
            return View();
        }

        // POST: GiftTestimonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Message,UserId,Id")] GiftTestimonial giftTestimonial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giftTestimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.GiftUsers, "Id", "Id", giftTestimonial.UserId);
            return View(giftTestimonial);
        }

        // GET: GiftTestimonials/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.GiftTestimonials == null)
            {
                return NotFound();
            }

            var giftTestimonial = await _context.GiftTestimonials.FindAsync(id);
            if (giftTestimonial == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.GiftUsers, "Id", "Id", giftTestimonial.UserId);
            return View(giftTestimonial);
        }

        // POST: GiftTestimonials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Message,UserId,Id")] GiftTestimonial giftTestimonial)
        {
            if (id != giftTestimonial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giftTestimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiftTestimonialExists(giftTestimonial.Id))
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
            ViewData["UserId"] = new SelectList(_context.GiftUsers, "Id", "Id", giftTestimonial.UserId);
            return View(giftTestimonial);
        }

        // GET: GiftTestimonials/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.GiftTestimonials == null)
            {
                return NotFound();
            }

            var giftTestimonial = await _context.GiftTestimonials
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftTestimonial == null)
            {
                return NotFound();
            }

            return View(giftTestimonial);
        }

        // POST: GiftTestimonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.GiftTestimonials == null)
            {
                return Problem("Entity set 'ModelContext.GiftTestimonials'  is null.");
            }
            var giftTestimonial = await _context.GiftTestimonials.FindAsync(id);
            if (giftTestimonial != null)
            {
                _context.GiftTestimonials.Remove(giftTestimonial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiftTestimonialExists(decimal id)
        {
          return (_context.GiftTestimonials?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
