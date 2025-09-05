using Car_Rental_Management.Data;
using Car_Rental_Management.ViewModels; // Ensure this namespace exists in your project
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Car_Rental_Management.Models;
using System.ComponentModel.DataAnnotations;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ========================
    // GET: /Admin/AddCar
    // ========================
    public IActionResult AddCar()
    {
        var vm = new CarVM
        {
            Car = new Car(),  // empty car object for the form to bind

            // Load dropdown for CarModel from DB
            CarModelList = _context.CarModels.Select(m => new SelectListItem
            {
                Value = m.CarModelID.ToString(),
                Text = m.ModelName
            }).ToList(),

            // Hardcoded status dropdown
            StatusList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Available", Text = "Available" },
                new SelectListItem { Value = "Maintenance", Text = "Maintenance" }
            }
        };

        return View(vm);
    }

    // ========================
    // POST: /Admin/AddCar
    // ========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddCar(CarVM vm)
    {
        // 🛠️ Remove them manually before validation
        ModelState.Remove("CarModelList");
        ModelState.Remove("StatusList");

        if (!ModelState.IsValid)
        {
            // Reload dropdowns
            vm.CarModelList = _context.CarModels.Select(m => new SelectListItem
            {
                Value = m.CarModelID.ToString(),
                Text = m.ModelName
            }).ToList();

            vm.StatusList = new List<SelectListItem>
        {
            new SelectListItem { Value = "Available", Text = "Available" },
            new SelectListItem { Value = "Maintenance", Text = "Maintenance" }
        };

            return View(vm);
        }

        _context.Cars.Add(vm.Car);
        _context.SaveChanges();

        return RedirectToAction("CarList");
    }

    // OPTIONAL: To show list of cars
    public IActionResult CarList(CarFilterVM filter)
    {
        if (filter == null)
            filter = new CarFilterVM();

        var cars = _context.Cars.Include(c => c.CarModel).AsQueryable();

        // Apply filtering
        if (filter.SelectedCarModelID.HasValue)
            cars = cars.Where(c => c.CarModelID == filter.SelectedCarModelID.Value);

        if (filter.MinRate.HasValue)
            cars = cars.Where(c => c.DailyRate >= filter.MinRate.Value);

        if (filter.MaxRate.HasValue)
            cars = cars.Where(c => c.DailyRate <= filter.MaxRate.Value);

        if (!string.IsNullOrEmpty(filter.Status))
            cars = cars.Where(c => c.Status == filter.Status);

        if (!string.IsNullOrEmpty(filter.Keyword))
            cars = cars.Where(c => c.Description.Contains(filter.Keyword));

        // Apply sorting
        switch (filter.SortOrder)
        {
            case "model_desc":
                cars = cars.OrderByDescending(c => c.CarModel.ModelName);
                break;
            case "rate_asc":
                cars = cars.OrderBy(c => c.DailyRate);
                break;
            case "rate_desc":
                cars = cars.OrderByDescending(c => c.DailyRate);
                break;
            default: // default sort by Model ascending
                cars = cars.OrderBy(c => c.CarModel.ModelName);
                break;
        }

        // Prepare ViewModel data for dropdowns and results
        filter.CarList = cars.ToList();

        filter.CarModelList = _context.CarModels.Select(m => new SelectListItem
        {
            Value = m.CarModelID.ToString(),
            Text = m.ModelName
        }).ToList();

        filter.StatusList = new List<SelectListItem>
    {
        new SelectListItem { Value = "Available", Text = "Available" },
        new SelectListItem { Value = "Maintenance", Text = "Maintenance" }
    };

        return View(filter);
    }
    // GET: Admin/EditCar/5
    // GET: Admin/EditCar/5
    public IActionResult EditCar(int id)
    {
        var car = _context.Cars
            .Include(c => c.CarModel)
            .FirstOrDefault(c => c.CarID == id);

        if (car == null) return NotFound();

        // Send car and CarModel list to view
        ViewBag.CarModels = _context.CarModels.ToList();
        return View(car);
    }

    // POST: Admin/EditCar/5
    [HttpPost]
    public IActionResult EditCar(Car updatedCar)
    {
        if (string.IsNullOrEmpty(updatedCar.ImageUrl2))
        {
            updatedCar.ImageUrl2 = string.Empty; // Or a default value
        }

        if (ModelState.IsValid)
        {
            _context.Cars.Update(updatedCar);
            _context.SaveChanges();
            return RedirectToAction("CarList");
        }

        // Reload dropdown if model validation fails
        ViewBag.CarModels = _context.CarModels.ToList();
        return View(updatedCar);
    }

    // GET: Admin/DeleteCar/5
    public IActionResult DeleteCar(int id)
    {
        var car = _context.Cars.Include(c => c.CarModel).FirstOrDefault(c => c.CarID == id);
        if (car == null) return NotFound();

        return View(car);
    }
    [HttpPost, ActionName("DeleteCar")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteCarConfirmed(int id)
    {
        var car = _context.Cars.Find(id);
        if (car == null) return NotFound();

        _context.Cars.Remove(car);
        _context.SaveChanges();

        return RedirectToAction("CarList");
    }

    [Required]
    public string Description { get; set; }


    // GET: Admin/Dashboard
    public IActionResult Dashboard()
    {
        // Collect statistics for dashboard
        var totalCars = _context.Cars.Count();
        var availableCars = _context.Cars.Count(c => c.Status == "Available");
        //var totalBookings = _context.Bookings.Count();
        //var totalCustomers = _context.Users.Count(u => u.Role == "Customer");

        // Pass data to View via ViewBag
        ViewBag.TotalCars = totalCars;
        ViewBag.AvailableCars = availableCars;
        //ViewBag.TotalBookings = totalBookings;
        // ViewBag.TotalCustomers = totalCustomers;

        // (Optional) Recent Bookings - last 5
        //var recentBookings = _context.Bookings
        //    .OrderByDescending(b => b.BookingDate)
        //    .Take(5)
        //    .ToList();

        //ViewBag.RecentBookings = recentBookings;

        return View();
    }
}
