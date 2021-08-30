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
    [Authorize(Roles = "dev")]
    public class DevelopmentController : Controller
    {
        private ApplicationContext db;
        public DevelopmentController(ApplicationContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<IActionResult> Account()
        {
            var book = await db.Bookings.FirstOrDefaultAsync(b => b.DevEmail == User.Identity.Name);
            return View(book);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Booking booking)
        {
            if(booking != null)
            {
                booking.DevEmail = User.Identity.Name;

                if(booking.UserCount > 0)
                {
                    db.Bookings.Add(booking);
                    await db.SaveChangesAsync();

                    return RedirectToAction("Account", "Development");
                }

                ModelState.AddModelError("", "Усі місця заповнені");

                return RedirectToAction("Add", "Development");
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var book = await db.Bookings.FirstOrDefaultAsync(b => b.Id == id);

            if(book != null)
            {
                return View(book);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Booking booking)
        {
            booking.DevEmail = User.Identity.Name;
            db.Bookings.Update(booking);
            await db.SaveChangesAsync();

            return RedirectToAction("Account", "Development");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
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
        public async Task<IActionResult> Delete(Booking booking)
        {
            if(booking != null)
            {
                await CRUD.DeleteBook(booking, db);

                return RedirectToAction("Account", "Development");
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> FollowedUsers()
        {
            var book =  await db.Bookings.Include(b => b.ReservedUsers).FirstOrDefaultAsync(b => b.DevEmail == User.Identity.Name);

            if (book != null)
            {             
                return View(book.ReservedUsers);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Comments()
        {
            var book = await db.Bookings.Include(b => b.Comments).FirstOrDefaultAsync(b => b.DevEmail == User.Identity.Name);

            if (book != null)
            {              
                return View(book.Comments);
            }

            return NotFound();
        }
    }
}
