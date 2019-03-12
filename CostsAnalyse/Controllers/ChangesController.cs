using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostsAnalyse.Models;
using CostsAnalyse.Models.Context;
using Microsoft.AspNetCore.Mvc;

namespace CostsAnalyse.Controllers
{
    public class ChangesController : Controller
    {
        private readonly ApplicationContext _appContext;
        public ChangesController(ApplicationContext context) {
            _appContext = context;
        }
        [HttpPost]
        public void AddChange(Changes change)
        {
            _appContext.Add(change);
            _appContext.SaveChangesAsync();
        }
        public IActionResult GetChanges([FromRoute] int skip)
        { 
            var changes = this._appContext.Changes.Skip(skip * 20).Take(20);
            return View(changes);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}