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
    public class GiftUsersController : Controller
    {
        private readonly ModelContext _context;

        public GiftUsersController(ModelContext context)
        {
            _context = context;
        }

        // GET: GiftUsers
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.GiftUsers.Include(g => g.Category).Include(g => g.Role);
            return View(await modelContext.ToListAsync());
        }

        // GET: GiftUsers/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.GiftUsers == null)
            {
                return NotFound();
            }

            var giftUser = await _context.GiftUsers
                .Include(g => g.Category)
                .Include(g => g.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftUser == null)
            {
                return NotFound();
            }

            return View(giftUser);
        }

        // GET: GiftUsers/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.GiftCategories, "Id", "Id");
            ViewData["RoleId"] = new SelectList(_context.GiftRoles, "Id", "Id");
            return View();
        }

        // POST: GiftUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Username,Password,Fname,Lname,PhoneNumber,Status,ImagePath,Id,RoleId,CategoryId")] GiftUser giftUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giftUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.GiftCategories, "Id", "Id", giftUser.CategoryId);
            ViewData["RoleId"] = new SelectList(_context.GiftRoles, "Id", "Id", giftUser.RoleId);
            return View(giftUser);
        }

        // GET: GiftUsers/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.GiftUsers == null)
            {
                return NotFound();
            }

            var giftUser = await _context.GiftUsers.FindAsync(id);
            if (giftUser == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.GiftCategories, "Id", "Id", giftUser.CategoryId);
            ViewData["RoleId"] = new SelectList(_context.GiftRoles, "Id", "Id", giftUser.RoleId);
            return View(giftUser);
        }

        // POST: GiftUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Email,Username,Password,Fname,Lname,PhoneNumber,Status,ImagePath,Id,RoleId,CategoryId")] GiftUser giftUser)
        {
            if (id != giftUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giftUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiftUserExists(giftUser.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.GiftCategories, "Id", "Id", giftUser.CategoryId);
            ViewData["RoleId"] = new SelectList(_context.GiftRoles, "Id", "Id", giftUser.RoleId);
            return View(giftUser);
        }

        // GET: GiftUsers/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.GiftUsers == null)
            {
                return NotFound();
            }

            var giftUser = await _context.GiftUsers
                .Include(g => g.Category)
                .Include(g => g.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftUser == null)
            {
                return NotFound();
            }

            return View(giftUser);
        }

        // POST: GiftUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.GiftUsers == null)
            {
                return Problem("Entity set 'ModelContext.GiftUsers'  is null.");
            }
            var giftUser = await _context.GiftUsers.FindAsync(id);
            if (giftUser != null)
            {
                _context.GiftUsers.Remove(giftUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiftUserExists(decimal id)
        {
          return (_context.GiftUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
