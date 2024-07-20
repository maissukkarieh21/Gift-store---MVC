using Gifts_Store_First_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Gifts_Store_First_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;


        public HomeController(ILogger<HomeController> logger, ModelContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.GiftCategories.ToList();
            var home = _context.GiftHomes.Where(h => h.Id == 1).SingleOrDefault();
            
            var bestGifts = new List<GiftGift>();
            foreach (var category in categories)
            {
                var bestGift = _context.GiftGifts
                    .Where(g => g.CategoryId == category.Id)
                    .OrderByDescending(g => g.Sale)
                    .FirstOrDefault();

                if (bestGift != null)
                {
                    bestGifts.Add(bestGift);
                }
            }
            
            ViewBag.categories = categories;
            ViewBag.home = home;
            //ViewBag.about = about;
            ViewBag.bestGifts = bestGifts;
            var homeTuple = Tuple.Create<IEnumerable<GiftCategory>, GiftHome, IEnumerable<GiftGift>>(categories, home, bestGifts);
            return View(homeTuple);
        }
        
        public IActionResult Category(int categoryId)
        {
            ViewBag.categories = _context.GiftCategories.ToList();
            var gifts = _context.GiftGifts.Where(c => c.CategoryId == categoryId).ToList();
            return View(gifts);
        }

        public IActionResult About()
        {
            var categories = _context.GiftCategories.ToList();
            ViewBag.categories = categories;
            var about = _context.GiftAbouts.Where(h => h.Id == 1).SingleOrDefault();
            return View(about);
        }

        public IActionResult Shop()
        {
            ViewBag.categories= _context.GiftCategories.ToList();
            var shop = _context.GiftGifts.ToList();
            return View(shop);

        }
        public IActionResult Contact()
        {
            ViewBag.categories = _context.GiftCategories.ToList();
            var contact = _context.GiftContacts.Where(h => h.Id == 1).SingleOrDefault();
            return View(contact);
        }

        //public IActionResult CheckOut(decimal? giftId)
        //{
        //    if (giftId == null)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    var userId = HttpContext.Session.GetInt32("sId");
        //    ViewBag.user = _context.GiftUsers.FirstOrDefault(u => u.Id == userId);
        //    ViewBag.giftId = giftId;
        //    ViewBag.categories = _context.GiftCategories.ToList();
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Order(int itemid, GiftOrder order)
        //{
        //    //try
        //    //{
        //        var userId = HttpContext.Session.GetInt32("sId");
        //        var gift = _context.GiftGiftsUsers.SingleOrDefault(g => g.GiftId == itemid);
        //        var user = _context.GiftUsers.SingleOrDefault(u => u.Id == userId);
        //        if (ModelState.IsValid && gift !=null && user!= null) 
        //        {
        //            order.GiftId = order.Id;
        //            order.OrderDate= DateTime.Now;
        //            order.Status = "Pending";
        //            order.UserId = user.Id;

        //            _context.Add(order);
        //            var makerId = gift.UserId;

        //            //if(makerId != null)
        //            //{

        //            //}
        //        }
        //    //}

        //}

        //public IActionResult CheckOut(decimal? giftId)
        //{
        //    // Get the logged-in user's information
        //    var userId = HttpContext.Session.GetInt32("sId");
        //    var user = _context.GiftUsers.SingleOrDefault(u => u.Id == userId);

        //    // Get the gift information
        //    var gift = _context.GiftGifts.SingleOrDefault(g => g.Id == giftId);

        //    if (user == null || gift == null)
        //    {
        //        // Handle the case where the user or gift is not found
        //        return RedirectToAction("Index");
        //    }

        //    // Pass the gift and user information to the view
        //    ViewBag.Gift = gift;
        //    ViewBag.User = user;
        //    ViewBag.categories = _context.GiftCategories.ToList();
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Order(decimal? giftId , string phoneNumber)
        //{
        //    var userId = HttpContext.Session.GetInt32("sId");
        //    var user = _context.GiftUsers.SingleOrDefault(u => u.Id == userId);

        //    var order = new GiftOrder
        //    {
        //        GiftId = giftId,
        //        UserId = user?.Id,
        //        PhoneNumber = phoneNumber,
        //        OrderDate = DateTime.Today,
        //        Status = "Pending"
        //    };

        //    _context.GiftOrders.Add(order);
        //    _context.SaveChanges();

        //    return RedirectToAction("OrderConfirmation");
        //}

        public IActionResult CheckOut(int? giftId)
        {
            var userId =  HttpContext.Session.GetInt32("Id");
            var user = _context.GiftUsers.SingleOrDefault(u => u.Id == userId);

            var gift = _context.GiftGifts.SingleOrDefault(g => g.Id == giftId);

            if (user == null || gift == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.GiftId = giftId;
            ViewBag.UserId = userId;
            ViewBag.User = user;
            ViewBag.Gift = gift;
            ViewBag.Categories = _context.GiftCategories.ToList();
            return View(gift);
        }

        [HttpPost]
        public IActionResult Order(decimal? giftId, int? userId, string phoneNumber)
        {
            if (giftId == null /*|| userId == null*/)
            {
                return RedirectToAction("Index");
            }

            var gift = _context.GiftGifts.SingleOrDefault(g => g.Id == giftId);
            var user = _context.GiftUsers.SingleOrDefault(u => u.Id == userId);

            if (gift == null || user == null)
            {
                // Handle the case where the gift or user is not found
                return RedirectToAction("Index");
            }

            var order = new GiftOrder
            {
                GiftId = giftId,
                UserId = userId,
                PhoneNumber = phoneNumber,
                OrderDate = DateTime.Today,
                Status = "Pending"
            };

            _context.GiftOrders.Add(order);
            _context.SaveChanges();

            return RedirectToAction("OrderConfirmation");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}