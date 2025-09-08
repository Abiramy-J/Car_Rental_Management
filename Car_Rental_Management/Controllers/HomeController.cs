using Car_Rental_Management.Data;
using Car_Rental_Management.Models;
using Car_Rental_Management.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Car_Rental_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.Users
                .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

            if (user != null)
            {
                // Role-based dashboard redirect
                if (user.Role == "Admin")
                {
                    return RedirectToAction("AdminDashboard");
                }
                else if (user.Role == "Customer")
                {
                    return RedirectToAction("CustomerDashboard");
                }
                else if (user.Role == "Staff")
                {
                    return RedirectToAction("StaffDashboard");
                }
                else
                {
                    return RedirectToAction("GeneralDashboard");
                }
            }

            ViewBag.Error = "Invalid username or password";
            return View(model);
        }

        public IActionResult AdminDashboard() => View();
        public IActionResult CustomerDashboard() => View();
        public IActionResult StaffDashboard() => View();
        public IActionResult GeneralDashboard() => View();


        // GET: Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if username already exists
                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    ViewBag.Error = "Username already exists!";
                    return View(model);
                }

                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password, // ⚠️ Later use hashing!
                    Role = "Customer"
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(model);
        }

        //// GET: Login
        //public IActionResult Login()
        //{
        //    return View();
        //}

        // POST: Login
        //[HttpPost]
        //public IActionResult Login(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = _context.Users
        //            .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

        //        if (user != null)
        //        {
        //            if (user.Role == "Admin")
        //                return RedirectToAction("Dashboard", "Admin");
        //            else if (user.Role == "Staff")
        //                return RedirectToAction("Panel", "Staff");
        //            else
        //                return RedirectToAction("Index", "Customer");
        //        }

        //        ViewBag.Error = "Invalid username or password";
        //    }

        //    return View(model);
        //}
    }
}
