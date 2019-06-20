using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostsAnalyse.Models;
using CostsAnalyse.Models.Context;
using CostsAnalyse.Services.Logging;
using CostsAnalyse.Services.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CostsAnalyse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribeController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly FileLogging _logging;
        private readonly SubscribeRepository _subscribeRepository;
        private readonly UserManager<UserApp> _userManager;
        private readonly ProductRepository _productRepository;
        private Task<UserApp> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        public SubscribeController(ApplicationContext context, UserManager<UserApp> userManager)
        {
            _context = context;
            _userManager = userManager;
            _logging = new FileLogging();
            _subscribeRepository = new SubscribeRepository(_context);
            _productRepository = new ProductRepository(_context);
        }
        [HttpGet("Subscribe/{id}")]
        public async Task<IActionResult> Subscribe(int id)
        {

            var product =  await _productRepository.GetAsync(id);
            if (product != null)
            { 
                    var user = await GetCurrentUserAsync();
                    if (user != null)
                    {
                    if (user.EmailConfirmed == true)
                    {
                        _subscribeRepository.Add(user, product);
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("Email doesn`t confirmed");
                    }
                    } 
                
            }
            return BadRequest();
        }

        [HttpGet("unsubscribe/{id}")]
        public async Task<IActionResult> Unsubscring(int id)
        {
            var product = await _productRepository.GetAsync(id) ;
            if (product != null)
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                { 
                        _subscribeRepository.Delete(user, product);
                    return Ok();
                }
            }
            return BadRequest();
        }
    }
}