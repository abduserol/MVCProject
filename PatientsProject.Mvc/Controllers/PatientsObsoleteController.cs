using Microsoft.AspNetCore.Mvc;
using PatientsProject.App.Domain;
using PatientsProject.App.Models;
using PatientsProject.App.Services;

namespace PatientsProject.Mvc.Controllers;

public class PatientsObsoleteController : Controller
{
    private readonly PatientObsoleteService _patientService;

    public PatientsObsoleteController(PatientObsoleteService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var list = _patientService.Query().ToList();

        if (list.Count == 0)
            ViewBag.Message = "No patients found";

        return View(list);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var item = _patientService.Query().SingleOrDefault(x => x.Id == id);

        if (item is null)
            ViewBag.Message = "No patients found";

        return View(item);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(PatientRequest request)
    {
        if (ModelState.IsValid)
        {
            var response = _patientService.Create(request);
            if (response.IsSuccessful)
            {
                TempData["Message"] = response.Message;

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Message = response.Message;
        }

        return View();
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var request = _patientService.Edit(id);
        if (request is null)
            ViewBag.Message = "Patient not found";

        return View(request);
    }

    [HttpPost]
    public IActionResult Edit(PatientRequest request)
    {
        if (ModelState.IsValid)
        {
            var response = _patientService.Update(request);
            if (response.IsSuccessful)
                return RedirectToAction(nameof(Details), new { id = response.Id });
            ViewBag.Message = response.Message;
        }
        return View(request);
    }

    public IActionResult Delete(int id)
    {
        var response = _patientService.Delete(id);
        
        TempData["Message"] = response.Message;
        
        return RedirectToAction(nameof(Index));
    }
}