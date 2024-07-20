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
    public class GiftCategoriesController : Controller
    {
        private readonly ModelContext _context;

        private readonly IWebHostEnvironment _webHostEnviroment;

        public GiftCategoriesController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: GiftCategories
        public async Task<IActionResult> Index()
        {
              return _context.GiftCategories != null ? 
                          View(await _context.GiftCategories.ToListAsync()) :
                          Problem("Entity set 'ModelContext.GiftCategories'  is null.");
        }

        // GET: GiftCategories/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.GiftCategories == null)
            {
                return NotFound();
            }

            var giftCategory = await _context.GiftCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftCategory == null)
            {
                return NotFound();
            }

            return View(giftCategory);
        }

        // GET: GiftCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GiftCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ImageFile,Id")] GiftCategory giftCategory)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + giftCategory.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/CategoryImages/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))

                {
                    await giftCategory.ImageFile.CopyToAsync(fileStream);
                }
                giftCategory.ImagePath = fileName;
                _context.Add(giftCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(giftCategory);
        }

        // GET: GiftCategories/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.GiftCategories == null)
            {
                return NotFound();
            }

            var giftCategory = await _context.GiftCategories.FindAsync(id);
            if (giftCategory == null)
            {
                return NotFound();
            }
            return View(giftCategory);
        }

        // POST: GiftCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Name,ImagePath,Id,ImageFile")] GiftCategory giftCategory)
        {
            if (id != giftCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCategory = await _context.GiftCategories.FindAsync(id);
                    if (existingCategory == null)
                    {
                        return NotFound();
                    }

                    if (giftCategory.ImageFile != null)
                    {
                        // Delete the previous image if it exists
                        if (!string.IsNullOrEmpty(existingCategory.ImagePath))
                        {
                            var previousImagePath = Path.Combine(_webHostEnviroment.WebRootPath, "CategoryImage", existingCategory.ImagePath);
                            if (System.IO.File.Exists(previousImagePath))
                            {
                                System.IO.File.Delete(previousImagePath);
                            }
                        }

                        // Save the new image
                        string wwwRootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + giftCategory.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath, "CategoryImage", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await giftCategory.ImageFile.CopyToAsync(fileStream);
                        }

                        existingCategory.ImagePath = fileName;
                    }

                    existingCategory.Name = giftCategory.Name;

                    _context.Update(existingCategory);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiftCategoryExists(giftCategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(giftCategory);
        }

        // GET: GiftCategories/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.GiftCategories == null)
            {
                return NotFound();
            }

            var giftCategory = await _context.GiftCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftCategory == null)
            {
                return NotFound();
            }

            return View(giftCategory);
        }

        // POST: GiftCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.GiftCategories == null)
            {
                return Problem("Entity set 'ModelContext.GiftCategories'  is null.");
            }
            var giftCategory = await _context.GiftCategories.FindAsync(id);
            if (giftCategory == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(giftCategory.ImagePath))
            {
                var imagePath = Path.Combine(_webHostEnviroment.WebRootPath, "CategoryImage", giftCategory.ImagePath);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.GiftCategories.Remove(giftCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiftCategoryExists(decimal id)
        {
          return (_context.GiftCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
