using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientsProject.App.Models;
using PatientsProject.Core.App.Services.MVC;

namespace PatientsProject.Mvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GroupsController : Controller
    {
        private readonly IService<GroupRequest, GroupResponse> _groupService;

        public GroupsController(IService<GroupRequest, GroupResponse> groupService)
        {
            _groupService = groupService;
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
            var list = _groupService.List();
            return View(list);
        }

        public IActionResult Details(int id)
        {
            var item = _groupService.Item(id);
            return View(item);
        }

        public IActionResult Create()
        {
            SetViewData();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(GroupRequest group)
        {
            if (ModelState.IsValid)
            {
                var response = _groupService.Create(group);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(group);
        }

        public IActionResult Edit(int id)
        {
            var item = _groupService.Edit(id);
            SetViewData();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(GroupRequest group)
        {
            if (ModelState.IsValid)
            {
                var response = _groupService.Update(group);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(group);
        }

        public IActionResult Delete(int id)
        {
            var item = _groupService.Item(id);
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _groupService.Delete(id);
            SetTempData(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
