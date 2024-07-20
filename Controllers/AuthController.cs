using Gifts_Store_First_project.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gifts_Store_First_project.Controllers
{
    public class AuthController : Controller
    {
        private readonly ModelContext _context;

        private readonly IWebHostEnvironment _webHostEnviroment;
        public AuthController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }


        public IActionResult CustomerRegister()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CustomerRegister([Bind("Fname,Lname,ImageFile,PhoneNumber,Password,Email,Username")] GiftUser user, string confirmPass)
        {
            if (ModelState.IsValid)
            {
                if (!IsPasswordValid(user.Password))
                {
                    ViewBag.msg = "Password must contain at least 8 characters, including an uppercase letter and a symbol.";
                    return View(user);
                }

                // Check if the email already exists
                if (_context.GiftUsers.Any(u => u.Email == user.Email))
                {
                    ViewBag.msg2 = "Email is already registered.";
                    return View(user);
                }

                // Check if the username already exists
                if (_context.GiftUsers.Any(u => u.Username == user.Username))
                {
                    ViewBag.msg3 = "Username is already taken.";
                    return View(user);
                }

                if(confirmPass== user.Password)
                {
                    ViewBag.msg4 = "Passwords do not match.";
                    return View(user);
                }

                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Customers_image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await user.ImageFile.CopyToAsync(fileStream);
                }

                user.ImagePath = fileName;
                user.RoleId = 3;
                user.Status = "Accepted";
                _context.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Auth");
            }

            return View(user);
        }

        //----------------------------------------------------------------------------------------------
        public IActionResult MakerRegister()
        {
            ViewData["CategoryId"] = new SelectList(_context.GiftCategories, "Id", "Name");
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakerRegister([Bind("Fname,Lname,ImageFile,PhoneNumber,Password,Email,Username,CategoryId")] GiftUser user)
        {
            if (ModelState.IsValid)
            {
                if (!IsPasswordValid(user.Password))
                {
                    ViewBag.msg = "Password must contain at least 8 characters, including an uppercase letter and a symbol.";
                    return View(user);
                }

                // Check if the email already exists
                if (_context.GiftUsers.Any(u => u.Email == user.Email))
                {
                    ViewBag.msg2 = "Email is already registered.";
                    return View(user);
                }

                // Check if the username already exists
                if (_context.GiftUsers.Any(u => u.Username == user.Username))
                {
                    ViewBag.msg3 = "Username is already taken.";
                    return View(user);
                }

                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Customers_image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await user.ImageFile.CopyToAsync(fileStream);
                }

                user.ImagePath = fileName;
                user.RoleId = 2;
                user.Status = "Pending";
                _context.Add(user);
                await _context.SaveChangesAsync();

                TempData["RegistrationMessage"] = "Your registration is successful. Please wait for the email with the Confirmation status, Thank you .";

                return RedirectToAction("Login", "Auth");
            }
            ViewData["CategoryId"] = new SelectList(_context.GiftCategories, "Id", "Name");

            return View(user);
        }

        //----------------------------------------------------------------------------------
        private bool IsPasswordValid(string pass)
        {
            var hasUpperCase = false;
            var hasSymbol = false;

            foreach (char c in pass)
            {
                if (char.IsUpper(c))
                    hasUpperCase = true;

                if (char.IsSymbol(c) || char.IsPunctuation(c))
                    hasSymbol = true;
            }

            return pass.Length >= 8 && hasUpperCase && hasSymbol;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([Bind("Username,Password")] GiftUser login)
        {
            var auth = _context.GiftUsers.FirstOrDefault(x => x.Username == login.Username && x.Password == login.Password);
            if (TempData.ContainsKey("RegistrationMessage"))
            {
                ViewBag.RegistrationMessage = TempData["RegistrationMessage"];
            }
            if (auth != null)
            {
                switch (auth.RoleId)
                {
                    case 2 when auth.Status == "Accepted":
                        //HttpContext.Session.SetString("CName", auth.Username);
                        HttpContext.Session.SetInt32("Id", (int)auth.Id);
                        return RedirectToAction("Index", "Maker");
                    case 3:
                        //HttpContext.Session.SetString("CName", auth.Username);
                        HttpContext.Session.SetInt32("Id", (int)auth.Id);
                        return RedirectToAction("Index", "Home");
                    case 1:
                        //HttpContext.Session.SetString("CName", auth.Username);
                        return RedirectToAction("Index", "Admin");
                    default:
                        ViewBag.ErrorMessage = "Your application is still pending or has been rejected. Please contact the administrator for more information.";
                        return View();
                }
            }
            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session = null;
            return RedirectToAction(nameof(Login));
        }
    }
}
