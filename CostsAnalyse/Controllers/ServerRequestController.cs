using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostsAnalyse.Services;
using CostsAnalyse.Services.MenuDrivers;
using CostsAnalyse.Services.ProxyServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CostsAnalyse.Controllers
{
    public class ServerRequestController : Controller
    {
      
         [Authorize(Roles = "Administrator")]
                         public async Task<IActionResult> UpdateProxy()
                         {
                             new ProxyBuilder().GenerateProxy();
           
            return RedirectToAction("Index", "Products");
        }
         [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> getPagesMin(){
             RozetkaMenuDriver rmd = RozetkaMenuDriver.GetInstanse();
             rmd.getPages();
           
             return RedirectToAction("Index", "Products");
        }
         [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetPagesAuto(){
             RozetkaMenuDriver rmd = RozetkaMenuDriver.GetInstanse();
            rmd.GetPagesAuto();
              FoxtrotMenuDriver fmd = new FoxtrotMenuDriver();
             fmd.GetPagesAuto();
            return RedirectToAction("Index", "Products");

        }

    }
}