using CostsAnalyse.Models;
using CostsAnalyse.Models.Context;
using CostsAnalyse.Services.EmailSenders;
using CostsAnalyse.Services.TokenBuilder;
using CostsAnalyse.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostsAnalyse.Controllers
{
    public class UserController : Controller
    {
        private readonly  UserManager<UserApp> _userManager;
        private readonly SignInManager<UserApp> _signInManager;
        private readonly ApplicationContext _applicationContext;
        private IConfiguration _config;
        public UserController( ApplicationContext applicationContext,UserManager<UserApp> userManager, SignInManager<UserApp> signInManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
            _applicationContext = applicationContext;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet("EmailConfirmed/{token}")]
        public async Task<IActionResult> ConfirmedEmail(string token)
        {
            var user = await _applicationContext.Users.FirstAsync(m => m.Token == token);
            if(user!=null)
            {
                user.EmailConfirmed = true;
                ViewData["Response"] = "Email is confirmed successed";
                return View();
            }
            else
            { 
                ViewData["Response"] = "Token is wrong";
                return View();
            }
        } 
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserApp user = new UserApp() {  Email = model.Email, UserName = model.Username, Country = model.Country};
                user.Token = TokenGenerator.Generate();
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                { 

                    await _signInManager.SignInAsync(user, false);
                    AuthMessageSender sender = new AuthMessageSender();
                    string baseUrl = $"{this.Request.Scheme}://{this.Request.Host}/";
                    string url  = baseUrl+"EmailConfirmed/"+user.Token;
                    string message = EmailMessageGenerator.GenerateEmailConfirmMessage(url);
                  
                    sender.SendEmailAsync(user.Email, "Please confirm your email", message);
                    return Redirect(baseUrl);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { });
        }

        [HttpPost] 
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            string baseUrl = $"{this.Request.Scheme}://{this.Request.Host}/";
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, true);
                if (result.Succeeded)
                { 
                     
                        return Redirect(baseUrl);
                  
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }
        [HttpGet] 
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null) //|| !(await _userManager.IsEmailConfirmedAsync(user)))
                { 
                    return View("ForgotPasswordConfirmation");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                AuthMessageSender authMessage = new AuthMessageSender();
                string header = "Reset Password";  
                var callbackUrl = Url.Action("ResetPassword", "User", new { userId = user.Id, token = token }, protocol: HttpContext.Request.Scheme);
     
                string message = EmailMessageGenerator.GenerateResetPasswordMessage(callbackUrl);
                await authMessage.SendEmailAsync(model.Email, "Reset Password",message);
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }
        [HttpGet] 
        public IActionResult ResetPassword(string token = null)
        {
            return token == null ? View("Error") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return View("ResetPasswordConfirmation");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return View("ResetPasswordConfirmation");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        [HttpGet("LogOff")] 
        public async Task<IActionResult> LogOff()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
}
}
