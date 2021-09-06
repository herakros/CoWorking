using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TestCoWorking.Data;
using TestCoWorking.Models;
using TestCoWorking.VIewModels;

namespace TestCoWorking.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext db;
        public AccountController(ApplicationContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if(ModelState.IsValid)
            {
                if (model.Role == "admin")
                {
                    return RedirectToAction("Warning", "Home");
                }

                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email 
                && u.Password == model.Password 
                && u.NickName == model.NickName);

                if(user == null)
                {
                    user = new User { Email = model.Email, Password = model.Password, NickName = model.NickName };
                    Role role = await db.Roles.FirstOrDefaultAsync(r => r.Name == model.Role);

                    if(role != null)
                    {
                        user.Role = role;
                    }

                    db.Users.Add(user);

                    await db.SaveChangesAsync();
                    await Authenticate(user);

                    return RedirectToAction("Account", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильно введена дані або такий користувач уже існує");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(ModelState.IsValid)
            {
                User user = await db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == model.Email
                && u.Password == model.Password);

                if (user != null)
                {
                    await Authenticate(user);

                    return RedirectToAction("Account", Char.ToUpper(user.Role.Name[0]) + user.Role.Name.Substring(1));
                }

                ModelState.AddModelError("", "Неправильно введена пошта і(або) пароль");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };

            ClaimsIdentity id = new ClaimsIdentity
                (claims,
                "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
