using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestCoWorking.Data;

namespace TestCoWorking.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
        public HomeController(ApplicationContext context)
        {
            db = context;
        }

        public IActionResult Account()
        {
            var email = User.Identity.Name;

            if(email != null)
            {
                var user = db.Users.Include(b => b.Role).FirstOrDefault(u => u.Email == email);
              
                return RedirectToAction("Account", Char.ToUpper(user.Role.Name[0]) + user.Role.Name.Substring(1));
            }

            return RedirectToAction("Reservation", "Home");
        }

        public IActionResult About()
        {
            return View();
        }

        public async Task<IActionResult> Reservation()
        {
            return View(await db.Bookings.Where(b => b.Approved == true).ToArrayAsync());
        }

        public IActionResult Warwing()
        {
            return View();
        }
    }
}
