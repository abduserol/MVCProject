using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PatientsProject.App.Models;
using PatientsProject.App.Services;
using PatientsProject.Core.App.Services.MVC;

namespace PatientsProject.Mvc.Controllers
{
    public class UsersController : Controller
    {
        private readonly IService<UserRequest, UserResponse> _userService;
        private readonly IService<RoleRequest, RoleResponse> _roleService;
        private readonly IService<GroupRequest, GroupResponse> _groupService;

        public UsersController(
            IService<UserRequest, UserResponse> userService,
            IService<RoleRequest, RoleResponse> roleService,
            IService<GroupRequest, GroupResponse> groupService)
        {
            _userService = userService;
            _roleService = roleService;
            _groupService = groupService;
        }

        private void SetViewData()
        {
            var groups = _groupService.List();
            ViewBag.GroupId = new SelectList(groups, "Id", "Title");

            var roles = _roleService.List();
            ViewBag.RoleIds = new MultiSelectList(roles, "Id", "Name");
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        bool IsOwnAccount(int id)
        {
            return id.ToString() == (User.Claims.SingleOrDefault(claim => claim.Type == "Id")?.Value ?? string.Empty);
        }

        [Authorize]
        public IActionResult Index()
        {
            var list = _userService.List();
            return View(list);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var item = _userService.Item(id);
            return View(item);
        }

        [Authorize]
        public IActionResult Create()
        {
            if (!User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            SetViewData();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(UserRequest user)
        {
            if (!User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var response = _userService.Create(user);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(user);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var item = _userService.Edit(id);
            SetViewData();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(UserRequest user)
        {
            if (!IsOwnAccount(user.Id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var response = _userService.Update(user);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(user);
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var item = _userService.Item(id);
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        [Authorize]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var response = _userService.Delete(id);
            SetTempData(response.Message);
            return RedirectToAction(nameof(Index));
        }

        // ==================== AUTHENTICATION ACTIONS ====================

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            if (ModelState.IsValid)
            {
                var userService = _userService as UserService;
                var response = await userService.Login(request);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", response.Message);
            }
            return View(request);
        }

        public async Task<IActionResult> Logout()
        {
            var userService = _userService as UserService;
            await userService.Logout();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Register(UserRegisterRequest request)
        {
            if (ModelState.IsValid)
            {
                var userService = _userService as UserService;
                var response = userService.Register(request);
                if (response.IsSuccessful)
                {
                    SetTempData("User registered successfully. Please login.");
                    return RedirectToAction(nameof(Login));
                }
                ModelState.AddModelError("", response.Message);
            }
            return View(request);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}