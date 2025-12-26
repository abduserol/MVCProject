using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PatientsProject.App.Models;
using PatientsProject.Core.App.Services.MVC;

namespace PatientsProject.Mvc.Controllers
{
    [Authorize]
    public class PatientsController : Controller
    {
        private readonly IService<PatientRequest, PatientResponse> _patientService;
        private readonly IService<DoctorRequest, DoctorResponse> _doctorService;
        private readonly IService<UserRequest, UserResponse> _userService;
        private readonly IService<GroupRequest, GroupResponse> _groupService;

        public PatientsController(
            IService<PatientRequest, PatientResponse> patientService,
            IService<DoctorRequest, DoctorResponse> doctorService,
            IService<UserRequest, UserResponse> userService,
            IService<GroupRequest, GroupResponse> groupService)
        {
            _patientService = patientService;
            _doctorService = doctorService;
            _userService = userService;
            _groupService = groupService;
        }

        private void SetViewData()
        {
            var doctors = _doctorService.List();
            ViewBag.Doctors = new MultiSelectList(doctors, "Id", "DoctorName");

            var users = _userService.List();
            ViewBag.UserId = new SelectList(users, "Id", "UserName");

            var groups = _groupService.List();
            ViewBag.GroupId = new SelectList(groups, "Id", "Title");
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        public IActionResult Index()
        {
            var list = _patientService.List();
            return View(list);
        }

        public IActionResult Details(int id)
        {
            var item = _patientService.Item(id);
            return View(item);
        }

        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult Create()
        {
            SetViewData();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult Create(PatientRequest patient)
        {
            if (ModelState.IsValid)
            {
                var response = _patientService.Create(patient);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(patient);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var item = _patientService.Edit(id);
            SetViewData();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(PatientRequest patient)
        {
            if (ModelState.IsValid)
            {
                var response = _patientService.Update(patient);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(patient);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var item = _patientService.Item(id);
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _patientService.Delete(id);
            SetTempData(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}