using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostsAnalyse.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CostsAnalyse.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LoggingController : Controller
    {
        private readonly ILogging _logging;
        public LoggingController(ILogging logging)
        {
            _logging = logging;
        }
        public IActionResult Index()
        {
           var lines =  _logging.ReadAsync();
            if (lines != null)
            {
                return View(lines);
            }
            return View();
        }
    }
}