using Car_Rental_Management.Data;
using Car_Rental_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            // Trim spaces
            email = email?.Trim();
            password = password?.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Please enter both email and password.";
                return View();
            }

            // Check database
            var customer = _db.Customers
                .AsEnumerable()  // fetch into memory for case-insensitive comparison
                .FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
                                     && c.Password == password);

            if (customer != null)
            {
                HttpContext.Session.SetInt32("CustomerId", customer.CustomerId);
                HttpContext.Session.SetString("CustomerEmail", customer.Email);
                return RedirectToAction("Index", "Customer");
            }

            ViewBag.Error = "Invalid email or password!";
            return View();
        }




        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Customer customer)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                var existingCustomer = _db.Customers.FirstOrDefault(c => c.Email == customer.Email);
                if (existingCustomer != null)
                {
                    ViewBag.Error = "Email already registered!";
                    return View(customer);
                }

                _db.Customers.Add(customer);
                await _db.SaveChangesAsync();

                // Optional: login immediately after registration
                HttpContext.Session.SetInt32("CustomerId", customer.CustomerId);
                HttpContext.Session.SetString("CustomerEmail", customer.Email);

                return RedirectToAction("Index", "Customer");
            }

            return View(customer);
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
