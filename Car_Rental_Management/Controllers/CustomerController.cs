using Car_Rental_Management.Data;
using Car_Rental_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_Management.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CustomerController(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        // GET: Customer/Index
        public IActionResult Index()
        {
            // Show welcome page or dashboard for logged-in customer
            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        // GET: Customer/ViewCars
        public async Task<IActionResult> ViewCars()
        {
            var cars = await _db.CarModals.ToListAsync();
            return View(cars);
        }

        // GET: Customer/BookCar/5
        public async Task<IActionResult> BookCar(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _db.CarModals.FindAsync(id);
            if (car == null)
                return NotFound();

            return View(car);
        }

        // POST: Customer/BookCar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookCar(int id, DateTime startDate, DateTime endDate)
        {
            var car = await _db.CarModals.FindAsync(id);
            if (car == null)
                return NotFound();

            // Get logged-in customer
            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
                return RedirectToAction("Login", "Account");

            if (endDate <= startDate)
            {
                ModelState.AddModelError("", "End date must be after start date.");
                return View(car);
            }

            var days = (endDate - startDate).Days;

            var booking = new Booking
            {
                CarModelId = car.CarModelId,
                CustomerId = customerId.Value,
                StartDate = startDate,
                EndDate = endDate,
                TotalPrice = days * car.PricePerDay
            };

            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Car booked successfully for {days} day(s)!";
            return RedirectToAction(nameof(MyBookings));
        }

        // GET: Customer/MyBookings
        public async Task<IActionResult> MyBookings()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
                return RedirectToAction("Login", "Account");

            var bookings = await _db.Bookings
                .Include(b => b.CarModel)
                .Where(b => b.CustomerId == customerId)
                .OrderByDescending(b => b.StartDate)
                .ToListAsync();

            return View(bookings);
        }
    }
}
