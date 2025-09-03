using Car_Rental_Management.Data;
using Car_Rental_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_Management.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext DbContext;

        public BookingController(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        // GET: Booking
        public async Task<IActionResult> Index()
        {
            var booking = await DbContext.Bookings
                .Include(b => b.CarModel)
                .Include(b => b.Customer)
                .ToListAsync();

            return View(booking);
        }

        // GET: Booking/Details/5
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

        // GET: Booking/Create
        public IActionResult Create()
        {
            ViewData["CarModelId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(DbContext.CarModals, "CarModelId", "ModelName");
            ViewData["CustomerId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(DbContext.Customers, "CustomerId", "Name");
            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,CarModelId,CustomerId,StartDate,EndDate,TotalPrice")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                int days = (booking.EndDate - booking.StartDate).Days;
                var car = await DbContext.CarModals.FindAsync(booking.CarModelId);
                booking.TotalPrice = days * car.PricePerDay;

                DbContext.Add(booking);
                await DbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CarModelId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(DbContext.CarModals, "CarModelId", "ModelName", booking.CarModelId);
            ViewData["CustomerId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(DbContext.Customers, "CustomerId", "Name", booking.CustomerId);
            return View(booking);
        }

        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await DbContext.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            ViewData["CarModelId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(DbContext.CarModals, "CarModelId", "ModelName", booking.CarModelId);
            ViewData["CustomerId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(DbContext.Customers, "CustomerId", "Name", booking.CustomerId);
            return View(booking);
        }

        // POST: Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,CarModelId,CustomerId,StartDate,EndDate,TotalPrice")] Booking booking)
        {
            if (id != booking.BookingId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    int days = (booking.EndDate - booking.StartDate).Days;
                    var car = await DbContext.CarModals.FindAsync(booking.CarModelId);
                    booking.TotalPrice = days * car.PricePerDay;

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

            ViewData["CarModelId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(DbContext.CarModals, "CarModelId", "ModelName", booking.CarModelId);
            ViewData["CustomerId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(DbContext.Customers, "CustomerId", "Name", booking.CustomerId);
            return View(booking);
        }


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

        // POST: Booking/Delete/5
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
