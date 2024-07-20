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
    public class GiftHomesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;
        public GiftHomesController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: GiftHomes
        public async Task<IActionResult> Index()
        {
              return _context.GiftHomes != null ? 
                          View(await _context.GiftHomes.ToListAsync()) :
                          Problem("Entity set 'ModelContext.GiftHomes'  is null.");
        }

        // GET: GiftHomes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.GiftHomes == null)
            {
                return NotFound();
            }

            var giftHome = await _context.GiftHomes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftHome == null)
            {
                return NotFound();
            }

            return View(giftHome);
        }

        // GET: GiftHomes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GiftHomes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content,ImageFile,Id")] GiftHome giftHome)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + giftHome.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/homeassets/img/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))

                {
                    await giftHome.ImageFile.CopyToAsync(fileStream);
                }
                giftHome.ImagePath = fileName;
                _context.Add(giftHome);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(giftHome);
        }

        // GET: GiftHomes/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.GiftHomes == null)
            {
                return NotFound();
            }

            var giftHome = await _context.GiftHomes.FindAsync(id);
            if (giftHome == null)
            {
                return NotFound();
            }
            return View(giftHome);
        }

        // POST: GiftHomes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Content,ImagePath,Id,ImageFile")] GiftHome giftHome)
        {
            if (id != giftHome.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingHome = await _context.GiftHomes.FindAsync(id);
                    if (existingHome == null)
                    {
                        return NotFound();
                    }

                    if (giftHome.ImageFile != null)
                    {
                        // Delete the previous image if it exists
                        if (!string.IsNullOrEmpty(existingHome.ImagePath))
                        {
                            var previousImagePath = Path.Combine(_webHostEnviroment.WebRootPath, "homeassets/img", existingHome.ImagePath);
                            if (System.IO.File.Exists(previousImagePath))
                            {
                                System.IO.File.Delete(previousImagePath);
                            }
                        }

                        // Save the new image
                        string wwwRootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + giftHome.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath, "homeassets/img", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await giftHome.ImageFile.CopyToAsync(fileStream);
                        }

                        existingHome.ImagePath = fileName;
                    }

                    giftHome.Content = giftHome.Content;
                    _context.Update(giftHome);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiftHomeExists(giftHome.Id))
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
            return View(giftHome);
        }

        // GET: GiftHomes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.GiftHomes == null)
            {
                return NotFound();
            }

            var giftHome = await _context.GiftHomes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftHome == null)
            {
                return NotFound();
            }

            return View(giftHome);
        }

        // POST: GiftHomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.GiftHomes == null)
            {
                return Problem("Entity set 'ModelContext.GiftHomes'  is null.");
            }
            var giftHome = await _context.GiftHomes.FindAsync(id);
            if (giftHome == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(giftHome.ImagePath))
            {
                var imagePath = Path.Combine(_webHostEnviroment.WebRootPath, "homeassets/img", giftHome.ImagePath);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.GiftHomes.Remove(giftHome);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiftHomeExists(decimal id)
        {
          return (_context.GiftHomes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
