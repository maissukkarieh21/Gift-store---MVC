using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gifts_Store_First_project.Models;
using System.Net.Mail;
using System.Net;
using System.Text;
namespace Gifts_Store_First_project.Controllers

{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;

        public AdminController(ModelContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewData["AName"] = HttpContext.Session.GetString("AName");
            ViewBag.NumberOfCustomers = _context.GiftUsers.Count();
            ViewData["Sales"] = _context.GiftGifts.Sum(x => x.Sale * x.Price); 
            ViewBag.MaxPrice = _context.GiftGifts.Max(y => y.Price);
            ViewBag.MinPrice = _context.GiftGifts.Min(z => z.Price);
            var customers = _context.GiftUsers.ToList();
            var products = _context.GiftGifts.ToList();
            var categories = _context.GiftCategories.ToList();
            var model = Tuple.Create<IEnumerable<GiftUser>, IEnumerable<GiftGift>, IEnumerable<GiftCategory>>(customers, products, categories);
            return View(model);
            
        }
        public IActionResult ManageGiftMakers()
        {
            //var giftMakers = _context.GiftUsers.Where(u => u.RoleId == 2).ToList();

            //return View(giftMakers);
            var giftMakers = _context.GiftUsers
            .Where(u => u.RoleId == 2)
            .Include(u => u.Category) 
            .ToList();

            return View(giftMakers);
        }

        [HttpPost]
        public IActionResult AcceptGiftMaker(decimal id)
        {
            var giftMaker = _context.GiftUsers.FirstOrDefault(u => u.Id == id);
            if (giftMaker != null && giftMaker.Status == "Pending")
            {
                giftMaker.Status = "Accepted";
                _context.SaveChanges();
                SendEmail(giftMaker.Email, "Congratulations, your gift maker application has been Accepted.", "Gift Maker Application");
            }

            return RedirectToAction("ManageGiftMakers");
        }

        [HttpPost]
        public IActionResult RejectGiftMaker(decimal id)
        {
            var giftMaker = _context.GiftUsers.FirstOrDefault(u => u.Id == id);
            if (giftMaker != null && giftMaker.Status == "Pending")
            {
                giftMaker.Status = "Rejected";
                _context.SaveChanges();
                SendEmail(giftMaker.Email, "Sorry, your gift maker application has been Rejected.", "Gift Maker Application");
            }

            return RedirectToAction("ManageGiftMakers");
        }


        public void SendEmail(string to, string body, string subject)
        {

            string from = "eglorious.gifts@gmail.com";
            MailMessage message = new MailMessage(from, to);

            string mailbody = body;
            message.Subject = subject;
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            try
            {
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential("eglorious.gifts@gmail.com", "gtsdjbrhirbtndqk");

                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);

            }


        }

    }
}
