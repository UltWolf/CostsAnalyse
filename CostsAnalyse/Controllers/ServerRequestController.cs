using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostsAnalyse.Services;
using CostsAnalyse.Services.ProxyServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CostsAnalyse.Controllers
{
    public class ServerRequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
         [Authorize(Roles = "Administrator")]
                         public async Task<IActionResult> UpdateProxy()
        {
            ProxyServerConnectionManagment.SerialiseProxyServers(true);
           
            return RedirectToAction("Index", "Products");
        }
         [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> getPagesMin(){
             RozetkaMenuDriver rmd = new RozetkaMenuDriver();
             rmd.getPages();
             return RedirectToAction("Index", "Products");
        }
         [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetPagesAuto(){
             RozetkaMenuDriver rmd = new RozetkaMenuDriver();
            rmd.GetPagesAuto();
            return RedirectToAction("Index", "Products");

        }

    }
}