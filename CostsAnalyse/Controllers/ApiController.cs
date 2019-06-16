using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostsAnalyse.Models.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CostsAnalyse.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public ApiController(ApplicationContext context){
             _context = context;
        }
        

        [HttpGet("Price/{id}", Name="GetPrices")]
        public async Task<JsonResult> getPrices(int? id){

            var product = _context.Products.Include(m=>m.Price).ThenInclude(m=>m.Company).Where(m=>m.Id==id); 
          return new JsonResult(product);
        }
         [HttpGet("LastPrice/{id}", Name="GetLastPrices")]
        public async Task<JsonResult> getLastPrices(int? id){

            var product = _context.Products.Include(m=>m.LastPrice).ThenInclude(m=>m.Company).Where(m=>m.Id==id); 
          return new JsonResult(product);
        }
         
    }
}