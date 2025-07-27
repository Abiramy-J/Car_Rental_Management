using Car_Rental_Management.Data;
using Car_Rental_Management.Models;
using Microsoft.AspNetCore.Mvc;
using System;

public class CarModelController : Controller
{
    private readonly ApplicationDbContext _dbcontext;

    public CarModelController(ApplicationDbContext context)
    {
        _dbcontext = context;
    }

    // GET: CarModel
    public IActionResult Index()
    {
        var models = _dbcontext.CarModels.ToList();
        return View(models);
    }

    // GET: CarModel/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CarModel/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CarModel model)
    {
        if (ModelState.IsValid)
        {
            _dbcontext.CarModels.Add(model);
            _dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(model);
    }

    // GET: CarModel/Edit/5
    public IActionResult Edit(int id)
    {
        var model = _dbcontext.CarModels.Find(id);
        if (model == null) return NotFound();
        return View(model);
    }

    // POST: CarModel/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CarModel model)
    {
        if (ModelState.IsValid)
        {
            _dbcontext.CarModels.Update(model);
            _dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(model);
    }

    // GET: CarModel/Delete/5
    public IActionResult Delete(int id)
    {
        var model = _dbcontext.CarModels.Find(id);
        if (model == null) return NotFound();
        return View(model);
    }

    // POST: CarModel/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var model = _dbcontext.CarModels.Find(id);
        _dbcontext.CarModels.Remove(model);
        _dbcontext.SaveChanges();
        TempData["SuccessMessage"] = "✅ Car model deleted successfully!";

        return RedirectToAction("Index");
    }
}
