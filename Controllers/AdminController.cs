using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCoWorking.Data;
using TestCoWorking.Instruments;
using TestCoWorking.Models;

namespace TestCoWorking.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private ApplicationContext db;

        public AdminController(ApplicationContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> BookingList()
        {
            return View(await db.Bookings.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            return View(await db.Users.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int? id)
        {
            if(id != null)
            {
                var user = await db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);

                if(user != null)
                {
                    return View(user);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(User user)
        {
            db.Users.Update(user);
            await db.SaveChangesAsync();

            return RedirectToAction("UserList", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id != null)
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (user != null)
                {
                    return View(user);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(User user)
        {
            if(user != null)
            {
                db.Entry(user).State = EntityState.Deleted;
                await db.SaveChangesAsync();

                return RedirectToAction("UserList", "Admin");
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> EditBooking(int? id)
        {
            if(id != null)
            {
                var book = await db.Bookings.FirstOrDefaultAsync(b => b.Id == id);

                if(book != null)
                {
                    return View(book);
                }
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditBooking(Booking book)
        {
            if(book != null)
            {
                db.Bookings.Update(book);
                await db.SaveChangesAsync();

                return RedirectToAction("BookingList", "Admin");
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBooking(int? id)
        {
            if(id != null)
            {
                var book = await db.Bookings.FirstOrDefaultAsync(b => b.Id == id);

                if(book != null)
                {
                    return View(book);
                }
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBooking(Booking booking)
        {
            if(booking != null)
            {
                await CRUD.DeleteBook(booking, db);
                return RedirectToAction("BookingList", "Admin");
            }

            return NotFound();
        }
    }
}
