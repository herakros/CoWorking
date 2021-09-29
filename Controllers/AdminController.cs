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
using TestCoWorking.VIewModels;

namespace TestCoWorking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationContext db;

        public AdminController(ApplicationContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<IActionResult> Account()
        {
            var homeModel = new AdminHomeModel();

            var place = await db.Place.FirstOrDefaultAsync(p => p.Id == 1);
            var users = await db.Users.CountAsync();
            var books = await db.Bookings.ToArrayAsync();

            homeModel.PlaceCount = (int)place.Count;
            homeModel.UsersCount = users;
            homeModel.ApprovedBookingCount = books.Where(b => b.Approved == true).Count();
            homeModel.PendingApporvedCount = books.Where(b => b.Approved != true).Count();

            return View(homeModel);
        }

        [HttpGet]
        public IActionResult AddSeas()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSeas(PlaceModel model)
        {
            if(ModelState.IsValid)
            {
                var place = await db.Place.FirstOrDefaultAsync();
                place.Count = (int)model.Count;
                AddBookings(place, (int)model.Count);

                db.Place.Update(place);
                await db.SaveChangesAsync();

                return RedirectToAction("Account");
            }

            return View();
        }

        private void AddBookings(Place place, int count)
        {
            for(int i = 0; i < count; i++)
            {
                place.Bookings.Add(new Booking());
            }
        }

        [HttpGet]
        public async Task<IActionResult> ApprovedBooking()
        {
            return View(await db.Bookings.Where(b => b.Approved == true).ToArrayAsync());
        }

        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            return View(await db.Users.Include(r => r.Role).ToArrayAsync());
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

                return RedirectToAction("ApprovedBooking", "Admin");
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
                var book = await db.Bookings.Include(c => c.Comments).FirstOrDefaultAsync(b => b.Id == booking.Id);

                db.Bookings.Remove(book);

                await db.SaveChangesAsync();

                return RedirectToAction("ApprovedBooking", "Admin");
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> PendingApproved()
        {
            return View(await db.Bookings.Where(b => b.Approved == false).ToArrayAsync());
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmBook(int? id)
        {
            if(id != null)
            {
                var booking = await db.Bookings.FirstOrDefaultAsync(b => b.Id == id);

                if(booking != null)
                {
                    return View(booking);
                }
            }

            ModelState.AddModelError("", "Виникла помилка");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfrimBooking(Booking book)
        {
            if(book != null)
            {
                book.Approved = true;
                db.Bookings.Update(book);

                await db.SaveChangesAsync();

                return RedirectToAction("PendingApproved", "Admin");
            }

            return RedirectToAction("Account", "Admin");
        }
    }
}
