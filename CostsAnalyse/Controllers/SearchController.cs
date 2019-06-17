using System.Linq;
using System.Threading.Tasks;
using CostsAnalyse.Models.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CostsAnalyse.Controllers
{
public class SearchController:Controller{
    private readonly ApplicationContext _context;
    public SearchController(ApplicationContext context){
        this._context = context;
    }
        [HttpGet("FastSearch/{part}")]
        public async Task<IActionResult>  FastSearch(string part)
        {
            if (part != null)
            {
                 return new JsonResult(_context.Products.Where(m => m.Name.Contains(part)).Include(m=>m.Price).Take(5).ToList());
            }
            return BadRequest();
        }
        [HttpGet("/Search/{nameOfProduct}")]
        public async Task<IActionResult> Search(string nameOfProduct)
        {
            if (nameOfProduct != null)
            {
                var products = _context.Products.Where(m => m.Name.Contains(nameOfProduct)).Include(m=>m.Price).Take(20);
                if (products != null)
                {
                    return View(products);
                }
                return NotFound();
            }
            else
            {
                return BadRequest();
            }
        } 
}
}