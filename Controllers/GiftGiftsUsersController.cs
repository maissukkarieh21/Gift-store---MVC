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
    public class GiftGiftsUsersController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;
        public GiftGiftsUsersController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: GiftGiftsUsers
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.GiftGiftsUsers.Include(g => g.Gift).Include(g => g.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: GiftGiftsUsers/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.GiftGiftsUsers == null)
            {
                return NotFound();
            }

            var giftGiftsUser = await _context.GiftGiftsUsers
                .Include(g => g.Gift)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftGiftsUser == null)
            {
                return NotFound();
            }

            return View(giftGiftsUser);
        }

        // GET: GiftGiftsUsers/Create
        public IActionResult Create()
        {
            ViewData["GiftId"] = new SelectList(_context.GiftGifts, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.GiftUsers, "Id", "Id");
            return View();
        }

        // POST: GiftGiftsUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Quantity,DateFrom,DateTo,UserId,GiftId,Id")] GiftGiftsUser giftGiftsUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giftGiftsUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GiftId"] = new SelectList(_context.GiftGifts, "Id", "Id", giftGiftsUser.GiftId);
            ViewData["UserId"] = new SelectList(_context.GiftUsers, "Id", "Id", giftGiftsUser.UserId);
            return View(giftGiftsUser);
        }

        // GET: GiftGiftsUsers/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.GiftGiftsUsers == null)
            {
                return NotFound();
            }

            var giftGiftsUser = await _context.GiftGiftsUsers.FindAsync(id);
            if (giftGiftsUser == null)
            {
                return NotFound();
            }
            ViewData["GiftId"] = new SelectList(_context.GiftGifts, "Id", "Id", giftGiftsUser.GiftId);
            ViewData["UserId"] = new SelectList(_context.GiftUsers, "Id", "Id", giftGiftsUser.UserId);
            return View(giftGiftsUser);
        }

        // POST: GiftGiftsUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Quantity,DateFrom,DateTo,UserId,GiftId,Id")] GiftGiftsUser giftGiftsUser)
        {
            if (id != giftGiftsUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giftGiftsUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiftGiftsUserExists(giftGiftsUser.Id))
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
            ViewData["GiftId"] = new SelectList(_context.GiftGifts, "Id", "Id", giftGiftsUser.GiftId);
            ViewData["UserId"] = new SelectList(_context.GiftUsers, "Id", "Id", giftGiftsUser.UserId);
            return View(giftGiftsUser);
        }

        // GET: GiftGiftsUsers/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.GiftGiftsUsers == null)
            {
                return NotFound();
            }

            var giftGiftsUser = await _context.GiftGiftsUsers
                .Include(g => g.Gift)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftGiftsUser == null)
            {
                return NotFound();
            }

            return View(giftGiftsUser);
        }

        // POST: GiftGiftsUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.GiftGiftsUsers == null)
            {
                return Problem("Entity set 'ModelContext.GiftGiftsUsers'  is null.");
            }
            var giftGiftsUser = await _context.GiftGiftsUsers.FindAsync(id);
            if (giftGiftsUser != null)
            {
                _context.GiftGiftsUsers.Remove(giftGiftsUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiftGiftsUserExists(decimal id)
        {
          return (_context.GiftGiftsUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
