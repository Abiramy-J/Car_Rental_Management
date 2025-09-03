using Car_Rental_Management.Data;
using Car_Rental_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_Management.Controllers
{
    public class Car_BookingController : Controller
    {
        private readonly ApplicationDbContext DbContext;

        public Car_BookingController(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        // GET: Car_Booking
        public async Task<IActionResult> Index()
        {
            var bookings = await DbContext.Bookings
                .Include(b => b.CarModel)
                .Include(b => b.Customer)
                .ToListAsync();

            return View(bookings);
        }

        // GET: Car_Booking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await DbContext.Bookings
                .Include(b => b.CarModel)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // GET: Car_Booking/Create
        public IActionResult Create()
        {
            ViewData["CarModelId"] = new SelectList(DbContext.CarModals, "CarModelId", "ModelName");
            ViewData["CustomerId"] = new SelectList(DbContext.Customers, "CustomerId", "Name");
            return View();
        }

        // POST: Car_Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.TotalPrice = (booking.EndDate - booking.StartDate).Days * booking.CarModel.PricePerDay;
                DbContext.Add(booking);
                await DbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CarModelId"] = new SelectList(DbContext.CarModals, "CarModelId", "ModelName", booking.CarModelId);
            ViewData["CustomerId"] = new SelectList(DbContext.Customers, "CustomerId", "Name", booking.CustomerId);
            return View(booking);
        }

        // GET: Car_Booking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await DbContext.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            ViewData["CarModelId"] = new SelectList(DbContext.CarModals, "CarModelId", "ModelName", booking.CarModelId);
            ViewData["CustomerId"] = new SelectList(DbContext.Customers, "CustomerId", "Name", booking.CustomerId);
            return View(booking);
        }

        // POST: Car_Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.BookingId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    booking.TotalPrice = (booking.EndDate - booking.StartDate).Days * booking.CarModel.PricePerDay;
                    DbContext.Update(booking);
                    await DbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CarModelId"] = new SelectList(DbContext.CarModals, "CarModelId", "ModelName", booking.CarModelId);
            ViewData["CustomerId"] = new SelectList(DbContext.Customers, "CustomerId", "Name", booking.CustomerId);
            return View(booking);
        }

        // GET: Car_Booking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await DbContext.Bookings
                .Include(b => b.CarModel)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // POST: Car_Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await DbContext.Bookings.FindAsync(id);
            if (booking != null)
            {
                DbContext.Bookings.Remove(booking);
                await DbContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return DbContext.Bookings.Any(e => e.BookingId == id);
        }
    }
}
