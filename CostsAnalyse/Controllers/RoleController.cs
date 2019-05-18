using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CostsAnalyse.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CostsAnalyse.Controllers
{
    public class RoleController : Controller
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly RoleManager<UserApp> _roleManager;
        public RoleController(UserManager<UserApp> userManager, RoleManager<UserApp> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

        }
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult ListAdmins()
        {
            var users = this._userManager.GetUsersInRoleAsync("admin");
            return View(users);
        }
        public IActionResult ListModerators()
        {
            var users = this._userManager.GetUsersInRoleAsync("moderator");
            return View(users);
        }

        public IActionResult ListUsers()
        {
            var users = this._userManager.GetUsersInRoleAsync("user");
            return View(users);
        }

        public async Task<IActionResult> RemoveFromAdmins([FromRoute] string userId)
        {
            var user = _userManager.Users.First(m => m.Id == userId);
            if (user != null)
            {
                var identityResult = await _userManager.RemoveFromRoleAsync(user, "admin");
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("ListAdmins");
                }
            }
            return View();
        }
        public async Task<IActionResult> RemoveFromModerators([FromRoute] string userId)
        {
            var user = _userManager.Users.First(m => m.Id == userId);
            if (user != null)
            {
                var identityResult = await _userManager.RemoveFromRoleAsync(user, "moderator");
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("ListModerators");
                }
            }
            return View();
        }
        public async Task<IActionResult> RemoveFromUsers([FromRoute] string userId)
        {
            var user = _userManager.Users.First(m => m.Id == userId);
            if (user != null)
            {
                var identityResult = await _userManager.RemoveFromRoleAsync(user, "users");
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
            }
            return View();
        }
        public async Task<IActionResult> Ban([FromRoute] string userId)
        {
            var user = _userManager.Users.First(m => m.Id == userId);
            if (user != null)
            {
                var identityResult = await _userManager.AddToRoleAsync(user, "banned");
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
            }
            return View();
        }
    }
}