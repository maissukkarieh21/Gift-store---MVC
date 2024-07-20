using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gifts_Store_First_project.Models;
using Microsoft.AspNetCore.Hosting;

namespace Gifts_Store_First_project.Controllers
{
    public class GiftGiftsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public GiftGiftsController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: GiftGifts
        //public async Task<IActionResult> Index()
        //{
        //    var gifts = await _context.GiftGifts.Include(g => g.Category).ToListAsync();
        //    return View(gifts);
        //    return _context.GiftGifts != null ?
        //                  View(await _context.GiftGifts.ToListAsync()) :
        //                  Problem("Entity set 'ModelContext.GiftGifts'  is null.");
        //}

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("Id");
            Console.WriteLine("UserId from session: " + userId);
            //if (userId == null)
            //{
            //    return Unauthorized();
            //}

            var user = await _context.GiftUsers
            .Include(u => u.Role)
            .SingleOrDefaultAsync(u => u.Id == (decimal)userId);

            Console.WriteLine(user.RoleId + "------------------");
            if (user.RoleId == 1)
            {
                // User is an Admin, retrieve all gifts
                var gifts = await _context.GiftGifts.Include(g => g.Category).ToListAsync();
                return View(gifts);
            }
            else if (user.RoleId == 2)
            {
                // User is a Maker, retrieve only their added gifts based on their chosen category
                var makerCategoryId = user.CategoryId;
                var gifts = await _context.GiftGifts
                    .Include(g => g.Category)
                    .Where(g => g.CategoryId == makerCategoryId)
                    .Where(g => g.GiftGiftsUsers.Any(gu => gu.UserId == userId))
                    .ToListAsync();
                return View(gifts);
            }

            else
            {
                return Unauthorized();
            }
        }

        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.GiftGifts == null)
            {
                return NotFound();
            }

            var giftGift = await _context.GiftGifts
                .Include(g => g.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftGift == null)
            {
                return NotFound();
            }
            ViewData["CategoryName"] = giftGift.Category?.Name;
            return View(giftGift);
        }

        // GET: GiftGifts/Create
        //public IActionResult Create()
        //{
        //    var categories = _context.GiftCategories.ToList();
        //    ViewData["Categories"] = categories;
        //    ViewData["CategoryId"] = new SelectList(_context.GiftCategories, "Id", "Id");
        //    return View();
        //}

        //// POST: GiftGifts/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Name,Description,Price,Sale,ImageFile,Id")] GiftGift gift)
        //{
        //    if (ModelState.IsValid)
        //    {


        //        string wwwRootPath = _webHostEnviroment.WebRootPath;
        //        string fileName = Guid.NewGuid().ToString() + "_" + gift.ImageFile.FileName;
        //        string path = Path.Combine(wwwRootPath + "/GiftsImages/", fileName);
        //        using (var fileStream = new FileStream(path, FileMode.Create))
        //            {
        //                await gift.ImageFile.CopyToAsync(fileStream);
        //            }
        //        //var category = new GiftUser();
        //        //gift.CategoryId= category.CategoryId;
        //        //Console.WriteLine("---------"+gift.CategoryId);
        //        gift.ImagePath = fileName;
        //        _context.Add(gift);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));


        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.GiftCategories, "Id", "Id", gift.CategoryId);
        //    return View(gift);
        //}


        public IActionResult Create()
        {
            var categories = _context.GiftCategories.ToList();
            ViewData["Categories"] = categories;
            return View();
        }

        //// POST: GiftGifts/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Name,Description,Price,Sale,ImageFile,Id")] GiftGift gift)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Get the current user's ID
        //        decimal? userId = HttpContext.Session.GetInt32("Id");

        //        // Retrieve the user from the database including their category
        //        var user = await _context.GiftUsers.Include(u => u.Category).FirstOrDefaultAsync(u => u.Id == userId);

        //        // If the user is null or their category is null, return a not found error
        //        if (user == null || user.Category == null)
        //        {
        //            return NotFound();
        //        }

        //        // Set the category ID of the gift to the user's category ID
        //        gift.CategoryId = user.Category.Id;

        //        // Save the uploaded image file
        //        if (gift.ImageFile != null)
        //        {
        //            string wwwRootPath = _webHostEnviroment.WebRootPath;
        //            string fileName = Guid.NewGuid().ToString() + "_" + gift.ImageFile.FileName;
        //            string path = Path.Combine(wwwRootPath, "GiftsImages", fileName);
        //            using (var fileStream = new FileStream(path, FileMode.Create))
        //            {
        //                await gift.ImageFile.CopyToAsync(fileStream);
        //            }
        //            gift.ImagePath = fileName;
        //        }

        //        _context.Add(gift);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    ViewData["CategoryId"] = new SelectList(_context.GiftCategories, "Id", "Id", gift.CategoryId);
        //    return View(gift);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GiftGift gift, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                // Get the current user's ID
                decimal? userId = HttpContext.Session.GetInt32("Id");

                // Retrieve the user from the database including their category
                var user = await _context.GiftUsers
                    .Include(u => u.Category)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                // If the user is null or their category is null, return a not found error
                if (user == null || user.Category == null)
                {
                    return NotFound();
                }

                // Set the category ID of the gift to the user's category ID
                gift.CategoryId = user.Category.Id;

                // Save the uploaded image file
                if (imageFile != null)
                {
                    string wwwRootPath = _webHostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string path = Path.Combine(wwwRootPath, "GiftsImages", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    gift.ImagePath = fileName;
                }

                _context.Add(gift);
                await _context.SaveChangesAsync();

                // Create a new GiftGiftsUser entry
                var giftGiftsUser = new GiftGiftsUser
                {
                    UserId = user.Id,
                    GiftId = gift.Id
                };
                _context.Add(giftGiftsUser);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var categories = _context.GiftCategories.ToList();
            ViewData["Categories"] = categories;
            return View(gift);
        }




        // GET: GiftGifts/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.GiftGifts == null)
            {
                return NotFound();
            }

            var giftGift = await _context.GiftGifts.FindAsync(id);
            if (giftGift == null)
            {
                return NotFound();
            }
            var categories = _context.GiftCategories.ToList();
            ViewData["Categories"] = categories;
            ViewData["CategoryId"] = new SelectList(_context.GiftCategories, "Id", "Id", giftGift.CategoryId);
            return View(giftGift);
        }

        // POST: GiftGifts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Name,Description,Price,Sale,ImageFile,ImagePath,Id,CategoryId")] GiftGift gift)
        {
            if (id != gift.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingGift = await _context.GiftGifts.FindAsync(id);
                    if (existingGift == null)
                    {
                        return NotFound();
                    }

                    if (gift.ImageFile != null)
                    {
                        // Delete the previous image if it exists
                        if (!string.IsNullOrEmpty(existingGift.ImagePath))
                        {
                            var previousImagePath = Path.Combine(_webHostEnviroment.WebRootPath, "GiftsImages", existingGift.ImagePath);
                            if (System.IO.File.Exists(previousImagePath))
                            {
                                System.IO.File.Delete(previousImagePath);
                            }
                        }

                        // Save the new image
                        string wwwRootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + gift.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath, "GiftsImages", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await gift.ImageFile.CopyToAsync(fileStream);
                        }

                        existingGift.ImagePath = fileName;
                    }

                    existingGift.Name = gift.Name;
                    _context.Update(existingGift);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiftGiftExists(gift.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.GiftCategories, "Id", "Id", gift.CategoryId);
            return View(gift);
        }

        // GET: GiftGifts/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.GiftGifts == null)
            {
                return NotFound();
            }

            var giftGift = await _context.GiftGifts
                .Include(g => g.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giftGift == null)
            {
                return NotFound();
            }
            ViewData["CategoryName"] = giftGift.Category?.Name;
            return View(giftGift);
        }

        // POST: GiftGifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.GiftGifts == null)
            {
                return Problem("Entity set 'ModelContext.GiftGifts'  is null.");
            }
            var giftGift = await _context.GiftGifts.FindAsync(id);
            if (giftGift == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(giftGift.ImagePath))
            {
                var imagePath = Path.Combine(_webHostEnviroment.WebRootPath, "GiftsImages", giftGift.ImagePath);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.GiftGifts.Remove(giftGift);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiftGiftExists(decimal id)
        {
          return (_context.GiftGifts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
