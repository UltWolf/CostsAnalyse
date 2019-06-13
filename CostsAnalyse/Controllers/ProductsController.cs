﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CostsAnalyse.Models;
using CostsAnalyse.Models.Context;
using Microsoft.AspNetCore.Authorization;
using CostsAnalyse.Services;
using CostsAnalyse.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CostsAnalyse.Controllers
{
    [Authorize(Roles = "Administrator, Moderator")]
    public class ProductsController : Controller
    {
        private readonly ApplicationContext _context;
       private readonly UserManager<UserApp> _userManager;

        public ProductsController(ApplicationContext context, UserManager<UserApp> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        private Task<UserApp> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [AllowAnonymous]
        // GET: Products
        public async Task<IActionResult> Index([FromRoute]int page = 0)
        {
            var userIdentity = (ClaimsIdentity)User.Identity;
            var claims = userIdentity.Claims;
            var roleClaimType = userIdentity.RoleClaimType;
            var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();
             var products = this._context.Products.Skip(page * 20).Take(20).Include(m=> m.Price).Include(m=>m.Information).ToList();
             if(roles.Count>0){
            if(roles[0].Value.ToLowerInvariant()=="administrator"){
             return View("IndexAdmin",products);
            }else{
            return View("IndexUser",products);
            }
             }else{
                 return View("IndexUser",products);
             }
        }



        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AutoAdd()
        {

            List<Product> products = new List<Product>();
            var services = ParsingServicesManager.GetListServices(_context);
            foreach (var service in services)
            {
               service.GetProducts();
            }
            
            return new JsonResult("Ok");
        }
        public void InitHrefs()
        {
            RozetkaMenuDriver rmd = new RozetkaMenuDriver();
            rmd.getPages();
        }
        [AllowAnonymous]
        public async Task<IActionResult> Search(string nameOfProduct)
        {
            if (nameOfProduct != null)
            {
                var products = _context.Products.Where(m => m.Name.Contains(nameOfProduct));
                if (products == null)
                {
                    return View(products);
                }
                return NotFound();
            }
            else
            {
                return View("InputName");
            }
        }
        [HttpGet("Subscribe/{id}")]
        public async Task<IActionResult> Subscribe(int id){

           var product = _context.Products.First(m=> m.Id==id);
           if(product!=null){
               var user = await GetCurrentUserAsync();
               var userId = user?.Id;
               if(userId !=null){
                    UserProduct UP = new UserProduct();
                    UP.Products = product;
                    UP.Users = user;
               product.Subscribers.Add(UP);
               _context.Update(product);
               await _context.SaveChangesAsync();
                return Ok();
               }
           }
            return BadRequest();
        }
        [AllowAnonymous]
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(m=>m.Price)
                .Include(m => m.Information)
                .ThenInclude(s=>s.Value)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
          
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Category")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
 
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Category")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        } 

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
