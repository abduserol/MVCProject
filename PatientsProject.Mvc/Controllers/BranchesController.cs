using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientsProject.App.Models;
using PatientsProject.Core.App.Services.MVC;

namespace PatientsProject.Mvc.Controllers
{
    [Authorize]
    public class BranchesController : Controller
    {
        private readonly IService<BranchRequest, BranchResponse> _branchService;

        public BranchesController(IService<BranchRequest, BranchResponse> branchService)
        {
            _branchService = branchService;
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        public IActionResult Index()
        {
            var list = _branchService.List();
            return View(list);
        }

        public IActionResult Details(int id)
        {
            var item = _branchService.Item(id);
            return View(item);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(BranchRequest branch)
        {
            if (ModelState.IsValid)
            {
                var response = _branchService.Create(branch);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            return View(branch);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var item = _branchService.Edit(id);
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(BranchRequest branch)
        {
            if (ModelState.IsValid)
            {
                var response = _branchService.Update(branch);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            return View(branch);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var item = _branchService.Item(id);
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _branchService.Delete(id);
            SetTempData(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}