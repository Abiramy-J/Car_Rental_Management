using Car_Rental_Management.Data;
using Car_Rental_Management.Models;
using Car_Rental_Management.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_Management.Controllers
{
    public class CarModelController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CarModelController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: CarModel
        public async Task<IActionResult> Index()
        {
            var carModels = await _db.CarModals
                .Select(c => new CarModelViewModel
                {
                    CarModelId = c.CarModelId,
                    ModelName = c.ModelName,
                    Manufacturer = c.Manufacturer,
                    Year = c.Year,
                    TransmissionType = c.TransmissionType,
                    SeatingCapacity = c.SeatingCapacity,
                    PricePerDay = c.PricePerDay
                }).ToListAsync();

            return View(carModels);
        }

        // GET: CarModel/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CarModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarModelViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var car = new CarModal
                {
                    ModelName = vm.ModelName,
                    Manufacturer = vm.Manufacturer,
                    Year = vm.Year,
                    TransmissionType = vm.TransmissionType,
                    SeatingCapacity = vm.SeatingCapacity,
                    PricePerDay = vm.PricePerDay
                };

                _db.CarModals.Add(car);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: CarModel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var car = await _db.CarModals.FindAsync(id);
            if (car == null) return NotFound();

            var vm = new CarModelViewModel
            {
                CarModelId = car.CarModelId,
                ModelName = car.ModelName,
                Manufacturer = car.Manufacturer,
                Year = car.Year,
                TransmissionType = car.TransmissionType,
                SeatingCapacity = car.SeatingCapacity,
                PricePerDay = car.PricePerDay
            };

            return View(vm);
        }

        // POST: CarModel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CarModelViewModel vm)
        {
            if (id != vm.CarModelId) return NotFound();

            if (ModelState.IsValid)
            {
                var car = await _db.CarModals.FindAsync(id);
                if (car == null) return NotFound();

                car.ModelName = vm.ModelName;
                car.Manufacturer = vm.Manufacturer;
                car.Year = vm.Year;
                car.TransmissionType = vm.TransmissionType;
                car.SeatingCapacity = vm.SeatingCapacity;
                car.PricePerDay = vm.PricePerDay;

                try
                {
                    _db.Update(car);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarModelExists(car.CarModelId)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        // GET: CarModel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var car = await _db.CarModals.FindAsync(id);
            if (car == null) return NotFound();

            var vm = new CarModelViewModel
            {
                CarModelId = car.CarModelId,
                ModelName = car.ModelName,
                Manufacturer = car.Manufacturer,
                Year = car.Year,
                TransmissionType = car.TransmissionType,
                SeatingCapacity = car.SeatingCapacity,
                PricePerDay = car.PricePerDay
            };

            return View(vm);
        }

        // POST: CarModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _db.CarModals.FindAsync(id);
            if (car != null)
            {
                _db.CarModals.Remove(car);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: CarModel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var car = await _db.CarModals.FindAsync(id);
            if (car == null) return NotFound();

            var vm = new CarModelViewModel
            {
                CarModelId = car.CarModelId,
                ModelName = car.ModelName,
                Manufacturer = car.Manufacturer,
                Year = car.Year,
                TransmissionType = car.TransmissionType,
                SeatingCapacity = car.SeatingCapacity,
                PricePerDay = car.PricePerDay
            };

            return View(vm);
        }

        private bool CarModelExists(int id)
        {
            return _db.CarModals.Any(e => e.CarModelId == id);
        }
    }
}
