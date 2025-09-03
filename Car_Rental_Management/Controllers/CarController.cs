using Car_Rental_Management.Data;
using Car_Rental_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Car_Rental_Management.Controllers
{
    public class CarController : Controller
    {
        private readonly ApplicationDbContext DbContext;

        public CarController(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        // GET: Car/Index
        public async Task<IActionResult> Index()
        {
            var cars = DbContext.Cars.Include(c => c.CarModel);
            return View(await cars.ToListAsync());
        }

        // GET: Car/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var car = await DbContext.Cars
                .Include(c => c.CarModel)
                .FirstOrDefaultAsync(m => m.CarId == id);

            if (car == null) return NotFound();

            return View(car);
        }

        // GET: Car/Create
        public IActionResult Create()
        {
            ViewData["CarModelId"] = new SelectList(DbContext.CarModals, "CarModelId", "ModelName");
            return View();
        }

        // POST: Car/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car)
        {
            if (ModelState.IsValid)
            {
                DbContext.Add(car);
                await DbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CarModelId"] = new SelectList(DbContext.CarModals, "CarModelId", "ModelName", car.CarModelId);
            return View(car);
        }

        // GET: Car/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var car = await DbContext.Cars.FindAsync(id);
            if (car == null) return NotFound();

            ViewData["CarModelId"] = new SelectList(DbContext.CarModals, "CarModelId", "ModelName", car.CarModelId);
            return View(car);
        }

        // POST: Car/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Car car)
        {
            if (id != car.CarId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    DbContext.Update(car);
                    await DbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.CarId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CarModelId"] = new SelectList(DbContext.CarModals, "CarModelId", "ModelName", car.CarModelId);
            return View(car);
        }

        // GET: Car/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var car = await DbContext.Cars
                .Include(c => c.CarModel)
                .FirstOrDefaultAsync(m => m.CarId == id);

            if (car == null) return NotFound();

            return View(car);
        }

        // POST: Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await DbContext.Cars.FindAsync(id);
            if (car != null)
            {
                DbContext.Cars.Remove(car);
                await DbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return DbContext.Cars.Any(e => e.CarId == id);
        }
    }
}
