using System;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CostsAnalyse.Services.Abstracts;
using CostsAnalyse.Services.Repositories;

namespace CostsAnalyse.Controllers
{
    [Authorize(Roles = "Administrator, Moderator")] 
    public class ProductsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<UserApp> _userManager;
        private readonly ILogging _logging;
        private readonly ProductRepository _productRepository;

        public ProductsController(ApplicationContext context, UserManager<UserApp> userManager, ILogging logging)
        {
            _context = context;
            _userManager = userManager;
            _logging = logging;
            _productRepository = new ProductRepository(context);
            
        }
        private Task<UserApp> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [AllowAnonymous]
        [HttpGet("/Products")]
        [HttpGet("/Products/{page}")]
        [HttpGet("/Products/index/{page}")]
        // GET: Products
        public async Task<IActionResult> Index(int? page)
        { 
                if (page == null)
                {
                    page = 0;
                }
                var userIdentity = (ClaimsIdentity)User.Identity;
                var claims = userIdentity.Claims;
                var roleClaimType = userIdentity.RoleClaimType;
                var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();

                var allProducts = this._context.Products;
                int countProducts = await allProducts.CountAsync();
                int pageSize = 9;

                ViewData["TotalPages"] = (int)Math.Ceiling(countProducts / (double)pageSize);
                if (page - 1 >= 0)
                {
                    ViewData["PreviusPage"] = page - 1;
                }
                ViewData["CurrentPage"] = page;
                if (page + 1 <= (int)ViewData["TotalPages"])
                {
                    ViewData["NextPage"] = page + 1;
                }


                var products = allProducts.Skip((int)page * pageSize).Take(pageSize).Include(m => m.Price).Include(m => m.Information).ToList();

                if (roles.Count > 0)
                {
                    if (roles[0].Value.ToLowerInvariant() == "administrator")
                    {
                        return View("IndexAdmin", products);
                    }
                    else
                    {
                        return View("IndexUser", products);
                    }
                }
                else
                {
                    return View("IndexUser", products);
                }
             
        }



        [Authorize(Roles = "Administrator")]
        [HttpGet("AutoAdd")]
        public async Task<IActionResult> AutoAdd()
        {
             
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
                await _productRepository.AddAsync(product);
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

            var product = await _productRepository.GetAsync(id);
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
                    _productRepository.Update(product); 
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

            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await _productRepository.DeleteAsync(product);

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
