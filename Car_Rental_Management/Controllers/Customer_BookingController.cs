using Car_Rental_Management.Data;
using Car_Rental_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Rental_Management.Controllers
{
    public class Customer_BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Customer_BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Show available cars for booking
        public async Task<IActionResult> AvailableCars()
        {
            var availableCars = await _context.Cars
                .Include(c => c.CarModel)
                .Where(c => c.IsAvailable)
                .ToListAsync();

            return View(availableCars);
        }

        // GET Booking form
        [HttpGet]
        public async Task<IActionResult> BookCar(int id)
        {
            var car = await _context.Cars
                .Include(c => c.CarModel)
                .FirstOrDefaultAsync(c => c.CarId == id);

            if (car == null || !car.IsAvailable)
            {
                return NotFound("Car not available for booking.");
            }

            ViewBag.CarId = car.CarId;
            ViewBag.CarName = car.CarName;
            return View();
        }

        // POST BookCar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookCar(int carId, DateTime startDate, DateTime endDate)
        {
            if (endDate <= startDate)
            {
                ModelState.AddModelError("", "End date must be after start date.");
                return View();
            }

            var car = await _context.Cars
                .Include(c => c.CarModel)
                .FirstOrDefaultAsync(c => c.CarId == carId);

            if (car == null || !car.IsAvailable)
            {
                return NotFound("Car not available for booking.");
            }

            
            var customerId = 1; 

            var totalDays = (endDate - startDate).Days;
            if (totalDays <= 0) totalDays = 1;

            var totalPrice = car.CarModel.PricePerDay * totalDays;

            var booking = new Booking
            {
                CarModelId = car.CarModelId,
                CustomerId = customerId,
                StartDate = startDate,
                EndDate = endDate,
                TotalPrice = totalPrice
            };

            _context.Bookings.Add(booking);

            // Mark car as unavailable
            car.IsAvailable = false;
            _context.Cars.Update(car);

            await _context.SaveChangesAsync();

            return RedirectToAction("MyBookings");
        }

        // GET: Show logged-in customer's bookings
        public async Task<IActionResult> MyBookings()
        {
           
            var customerId = 1; 

            var bookings = await _context.Bookings
                .Include(b => b.CarModel)
                .Include(b => b.Customer)
                .Where(b => b.CustomerId == customerId)
                .ToListAsync();

            return View(bookings);
        }

        // Cancel  booking
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.CarModel)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            // Mark car available again
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.CarModelId == booking.CarModelId);
            if (car != null)
            {
                car.IsAvailable = true;
                _context.Cars.Update(car);
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyBookings");
        }
    }
}
