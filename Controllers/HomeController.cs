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

                if (user.Role.Name == "dev")
                {
                    return RedirectToAction("Account", "Development");
                }

                if (user.Role.Name == "manager")
                {
                    return RedirectToAction("Account", "Manager");
                }

                if (user.Role.Name == "admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
            }

            return RedirectToAction("Reservation", "Home");
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Reservation()
        {
            return View(db.Bookings.ToList());
        }

        public IActionResult Warwing()
        {
            return View();
        }
    }
}
