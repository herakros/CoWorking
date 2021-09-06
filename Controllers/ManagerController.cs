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
            var employee = new Employee() { Booking = book, BookingId = book.Id };

            if(book != null)
            {
                return View(employee);
            }

            return RedirectToAction("Account", "Manager");
        }

        [HttpPost]
        public async Task<IActionResult> Follow(Employee employee)
        {
            employee.Id = 0;
            if(employee != null)
            {
                db.Employees.Add(employee);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Look", "Manager", new { id = employee.BookingId }); 
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comment)
        {            
            if(comment != null)
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

                comment.UserName = user.NickName;

                db.Comments.Add(comment);

                await db.SaveChangesAsync();
            }

            return RedirectToAction("Look", "Manager", new { id = comment.BookingId });
        }

        [HttpGet]
        public async Task<IActionResult> Account()
        {
            var booking = await db.Bookings.Where(b => b.Approved == true).ToArrayAsync();
            return View(booking);
        }
    }
}
