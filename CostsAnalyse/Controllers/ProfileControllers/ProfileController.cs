﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CostsAnalyse.Models;
using CostsAnalyse.Models.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CostsAnalyse.Controllers
{
    public class ProfileController : Controller
    {
        private readonly string _userId;
        private readonly ApplicationContext _context;
        public ProfileController(IHttpContextAccessor httpContextAccessor, ApplicationContext context)
        {
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _context = context;
        }
        [Authorize]
        [ResponseCache(Duration = 60)]
        public IActionResult Index()
        {
            var user = _context.Users.Include(m=> m.products).ThenInclude(m=>m.Products).First(m => m.Id == _userId);
            return View(user);
        }

        

    }
}