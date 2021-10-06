using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCoWorking.Data;
using TestCoWorking.Models;

namespace TestCoWorking.Controllers
{
    [Authorize(Roles = "Developer, Admin")]
    public class DeveloperController : Controller
    {
        private ApplicationContext db;
        public DeveloperController(ApplicationContext context)
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
                db.Bookings.Add(booking);

                await db.SaveChangesAsync();

                return RedirectToAction("Account", "Developer");                              
            }

            ModelState.AddModelError("", "Перевірта введені дані");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var book = await db.Bookings.FirstOrDefaultAsync(b => b.Id == id);

            if(book != null)
            {
                return View(book);
            }

            return RedirectToAction("Account", "Developer");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Booking booking)
        {
            db.Bookings.Update(booking);
            await db.SaveChangesAsync();

            return RedirectToAction("Account", "Developer");
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

            return RedirectToAction("Account", "Developer");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Booking booking)
        {
            if(booking != null)
            {
                var book = await db.Bookings.Include(e => e.ReservedEmployeer).Include(c => c.Comments).FirstOrDefaultAsync(b => b.Id == booking.Id);

                db.Bookings.Remove(book);

                await db.SaveChangesAsync();

                return RedirectToAction("Account", "Developer");
            }

            ModelState.AddModelError("", "Виникла помилка");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FollowedUsers()
        {
            var book =  await db.Bookings
                .Include(b => b.ReservedEmployeer)
                .FirstOrDefaultAsync(b => b.DevEmail == User.Identity.Name);

            if (book != null)
            {             
                return View(book.ReservedEmployeer.Where(e => e.Appoved == true).ToList());
            }

            return RedirectToAction("Account", "Developer");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmployeer(int? id)
        {
            if(id != null)
            {
                var employeer = await db.Employees.FirstOrDefaultAsync(e => e.Id == id);
                employeer.Appoved = true;

                var book = await db.Bookings.FirstOrDefaultAsync(b => b.Id == employeer.BookingId);
                book.EmployeerCount--;

                await db.SaveChangesAsync();

                if(book.EmployeerCount == 0)
                {
                    return RedirectToAction("Account", "Developer");
                }
                else
                {
                    return RedirectToAction("EmployeesWantToSignUp", "Developer", new { id = book.Id });
                }
            }

            return RedirectToAction("Account", "Developer");
        }

        [HttpGet]
        public async Task<IActionResult> Comments()
        {
            var book = await db.Bookings.Include(b => b.Comments).FirstOrDefaultAsync(b => b.DevEmail == User.Identity.Name);

            if (book != null)
            {              
                return View(book.Comments);
            }

            return RedirectToAction("Account", "Developer");
        }

        [HttpGet]
        public async Task<IActionResult> EmployeesWantToSignUp(int? id)
        {
            if(id != null)
            {
                var book = await db.Bookings.Include(e => e.ReservedEmployeer).FirstOrDefaultAsync();

                return View(book.ReservedEmployeer.Where(e => e.Appoved == false).ToList());
            }

            return RedirectToAction("Account", "Developer");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployeer(int? id)
        {
            if(id != null)
            {
                var employeer = await db.Employees.FirstOrDefaultAsync(e => e.Id == id);
                var book = await db.Bookings.FirstOrDefaultAsync(b => b.Id == employeer.BookingId);

                db.Employees.Remove(employeer);

                if(employeer.Appoved == true)
                {
                    book.EmployeerCount++;
                }

                await db.SaveChangesAsync();
            }

            return RedirectToAction("Account", "Developer");
        }
    }
}
