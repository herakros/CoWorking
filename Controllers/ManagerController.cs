using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCoWorking.Data;
using TestCoWorking.Models;
using TestCoWorking.ViewModels;

namespace TestCoWorking.Controllers
{
    [Authorize(Roles = "Manager, Admin")]
    public class ManagerController : Controller
    {
        private ApplicationContext db;

        public ManagerController(ApplicationContext context)
        {
            db = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> Look(int? id)
        {
            if(id != null)
            {
                var book = await db.Bookings.Include(c => c.Comments).FirstOrDefaultAsync(b => b.Id == id);              

                if(book != null)
                {
                    var booking = new LookBookingModel() { Booking = book };
                    return View(booking);
                }
            }

            return RedirectToAction("Account", "Manager");
        }

        [HttpGet]
        public async Task<IActionResult> Follow(int? id)
        {
            var book = await db.Bookings.FirstOrDefaultAsync(b => b.Id == id);

            if(book != null)
            {
                return View();
            }

            return RedirectToAction("Account", "Manager");
        }

        [HttpPost]
        public async Task<IActionResult> Follow(User employee)
        {
            employee.Id = 0;
            if(employee != null)
            {
                db.Users.Add(employee);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Look", "Manager"); 
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(LookBookingModel model)
        {            
            if(model.Text != null)
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

                var comment = new Comment() { UserName = user.NickName, BookingId = model.BookingId, Message = model.Text };

                db.Comments.Add(comment);

                await db.SaveChangesAsync();
            }

            return RedirectToAction("Look", "Manager", new { id = model.BookingId });
        }

        [HttpGet]
        public async Task<IActionResult> Account()
        {
            var booking = await db.Bookings.ToArrayAsync();
            return View(booking);
        }
    }
}
