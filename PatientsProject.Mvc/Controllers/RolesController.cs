using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientsProject.App.Models;
using PatientsProject.Core.App.Services.MVC;

namespace PatientsProject.Mvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly IService<RoleRequest, RoleResponse> _roleService;

        public RolesController(IService<RoleRequest, RoleResponse> roleService)
        {
            _roleService = roleService;
        }

        private void SetViewData()
        {
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        public IActionResult Index()
        {
            var list = _roleService.List();
            return View(list);
        }

        public IActionResult Details(int id)
        {
            var item = _roleService.Item(id);
            return View(item);
        }

        public IActionResult Create()
        {
            SetViewData();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(RoleRequest role)
        {
            if (ModelState.IsValid)
            {
                var response = _roleService.Create(role);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(role);
        }

        public IActionResult Edit(int id)
        {
            var item = _roleService.Edit(id);
            SetViewData();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(RoleRequest role)
        {
            if (ModelState.IsValid)
            {
                var response = _roleService.Update(role);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(role);
        }

        public IActionResult Delete(int id)
        {
            var item = _roleService.Item(id);
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _roleService.Delete(id);
            SetTempData(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
