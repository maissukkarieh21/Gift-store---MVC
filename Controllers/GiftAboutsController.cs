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
    public class GiftAboutsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public GiftAboutsController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: GiftAbouts
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.GiftAbouts.Include(g => g.Home);
            return View(await modelContext.ToListAsync());
        }

        // GET: GiftAbouts/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.GiftAbouts == null)
            {
                return NotFound();
            }

            var giftAbout = await _context.GiftAbouts
                .Include(g => g.Home)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftAbout == null)
            {
                return NotFound();
            }

            return View(giftAbout);
        }

        // GET: GiftAbouts/Create
        public IActionResult Create()
        {
            ViewData["HomeId"] = new SelectList(_context.GiftHomes, "Id", "Id");
            return View();
        }

        // POST: GiftAbouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content,ImageFile,HomeId,Id")] GiftAbout giftAbout)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + giftAbout.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/homeassets/img/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))

                {
                    await giftAbout.ImageFile.CopyToAsync(fileStream);
                }
                giftAbout.ImagePath = fileName;
                _context.Add(giftAbout);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HomeId"] = new SelectList(_context.GiftHomes, "Id", "Id", giftAbout.HomeId);
            return View(giftAbout);
        }

        // GET: GiftAbouts/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.GiftAbouts == null)
            {
                return NotFound();
            }
            id = 1;
            var giftAbout = await _context.GiftAbouts.FindAsync(id);
            if (giftAbout == null)
            {
                return NotFound();
            }
            ViewData["HomeId"] = new SelectList(_context.GiftHomes, "Id", "Id", giftAbout.HomeId);
            return View(giftAbout);
        }

        // POST: GiftAbouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Content,ImagePath,HomeId,Id,ImageFile")] GiftAbout giftAbout)
        {
            if (id != giftAbout.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingAbout = await _context.GiftAbouts.FindAsync(id);
                    if (existingAbout == null)
                    {
                        return NotFound();
                    }

                    if (giftAbout.ImageFile != null)
                    {
                        // Delete the previous image if it exists
                        if (!string.IsNullOrEmpty(existingAbout.ImagePath))
                        {
                            var previousImagePath = Path.Combine(_webHostEnviroment.WebRootPath, "homeassets/img", existingAbout.ImagePath);
                            if (System.IO.File.Exists(previousImagePath))
                            {
                                System.IO.File.Delete(previousImagePath);
                            }
                        }

                        // Save the new image
                        string wwwRootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + giftAbout.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath, "homeassets/img", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await giftAbout.ImageFile.CopyToAsync(fileStream);
                        }

                        existingAbout.ImagePath = fileName;
                    }

                    existingAbout.Content = giftAbout.Content;
                    _context.Update(giftAbout);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiftAboutExists(giftAbout.Id))
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
            ViewData["HomeId"] = new SelectList(_context.GiftHomes, "Id", "Id", giftAbout.HomeId);
            return View(giftAbout);
        }

        // GET: GiftAbouts/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.GiftAbouts == null)
            {
                return NotFound();
            }

            var giftAbout = await _context.GiftAbouts
                .Include(g => g.Home)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftAbout == null)
            {
                return NotFound();
            }

            return View(giftAbout);
        }

        // POST: GiftAbouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.GiftAbouts == null)
            {
                return Problem("Entity set 'ModelContext.GiftAbouts'  is null.");
            }
            var giftAbout = await _context.GiftAbouts.FindAsync(id);
            if (giftAbout == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(giftAbout.ImagePath))
            {
                var imagePath = Path.Combine(_webHostEnviroment.WebRootPath, "homeassets/img", giftAbout.ImagePath);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _context.GiftAbouts.Remove(giftAbout);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiftAboutExists(decimal id)
        {
          return (_context.GiftAbouts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
