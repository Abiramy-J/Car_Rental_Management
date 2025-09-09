using Car_Rental_Management.Data;
using Car_Rental_Management.Models;
using Car_Rental_Management.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Car_Rental_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Username = model.Username?.Trim();
            model.Password = model.Password?.Trim();

            Console.WriteLine("Entered Username: " + model.Username);
            Console.WriteLine("Entered Password: " + model.Password);

            var user = _db.Users
                .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                return user.Role switch
                {
                    "Admin" => RedirectToAction("AdminDashboard", "Home"),
                    "Staff" => RedirectToAction("StaffDashboard", "Home"),
                    "Customer" => RedirectToAction("CustomerDashboard", "Home"),
                    _ => RedirectToAction("GeneralDashboard", "Home")
                };
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password");
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
                if (_db.Users.Any(u => u.Username == model.Username))
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

                _db.Users.Add(user);
                _db.SaveChanges();

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
