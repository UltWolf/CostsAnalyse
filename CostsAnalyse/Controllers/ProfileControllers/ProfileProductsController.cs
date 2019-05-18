using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CostsAnalyse.Models.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CostsAnalyse.Controllers.ProfileControllers
{
    public class ProfileProductsController : Controller
    {
        private readonly string _userId;
        private readonly ApplicationContext _context;
        public ProfileProductsController(IHttpContextAccessor httpContextAccessor, ApplicationContext context)
        {
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _context = context;
        }

        public IActionResult Index()
        {
            var user = _context.Users.First(m=>m.Id ==_userId);
            return View(user.products);
        }
    }
}