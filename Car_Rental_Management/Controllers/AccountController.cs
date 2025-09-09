using Car_Rental_Management.Data;
using Car_Rental_Management.Models;
using Car_Rental_Management.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Rental_Management.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AccountController(ApplicationDbContext db)
        {
            _db = db;
        }
        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        //[HttpPost]
        //public IActionResult Login(string username, string password)
        //{
        //    var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        //    if (user != null)
        //    {
        //        HttpContext.Session.SetInt32("UserID", user.UserID);
        //        HttpContext.Session.SetString("Role", user.Role);

        //        if (user.Role == "Admin")
        //            return RedirectToAction("Dashboard", "Admin");
        //        else
        //            return RedirectToAction("Dashboard", "Customer");
        //    }

        //    ViewBag.Error = "Invalid username or password!";
        //    return View();
        //}

        // GET: /Account/Register
        public IActionResult Register()
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

            var inputUsername = model.Username?.Trim().ToLower();
            var inputPassword = model.Password?.Trim();

            // Match username in lowercase and password as-is
            var user = _db.Users
                .FirstOrDefault(u =>
                    u.Username.ToLower() == inputUsername &&
                    u.Password == inputPassword);

            if (user != null)
            {
                // Save session
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                // Redirect to role-based dashboard
                return user.Role switch
                {
                    "Admin" => RedirectToAction("AdminDashboard", "Home"),
                    "Staff" => RedirectToAction("StaffDashboard", "Home"),
                    "Customer" => RedirectToAction("CustomerDashboard", "Home"),
                    _ => RedirectToAction("GeneralDashboard", "Home")
                };
            }

            // Login failed
            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return View(model);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if username exists
                if (_db.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(model);
                }

                // Create new user → default Customer role
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password, // ⚠️ plain text for now, later use hashing
                    Role = "Customer"
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                // Save session after registration
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                // Redirect Customer to home dashboard
                return RedirectToAction("CustomerDashboard", "Home");
            }

            return View(model);
        }


        // GET: Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
