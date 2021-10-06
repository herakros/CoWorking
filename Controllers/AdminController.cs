using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCoWorking.Data;
using TestCoWorking.Models;
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

        public IActionResult Account()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ApprovedBooking()
        {
            return View(await db.Bookings.Where(b => b.Approved == true).ToArrayAsync());
        }

        #region Edit Users

        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            return View(await db.Users.Include(r => r.Role).ToArrayAsync());
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
                EditUserModel editUserModel = new EditUserModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    NickName = user.NickName,
                    Role = user.Role.Name
                };

                if (user != null)
                {
                    return View(editUserModel);
                }
            }

            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Id == model.Id);

                if (user != null)
                {
                    user.NickName = model.NickName;
                    user.Email = model.Email;

                    Role role = await db.Roles.FirstOrDefaultAsync(r => r.Name == model.Role);

                    user.Role = role;

                    db.Users.Update(user);
                    await db.SaveChangesAsync();
                }

                return RedirectToAction("UserList", "Admin");
            }

            return View(model);
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
            if (user != null)
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
            if (id != null)
            {
                var book = await db.Bookings.FirstOrDefaultAsync(b => b.Id == id);

                if (book != null)
                {
                    return View(book);
                }
            }

            return NotFound();
        }

        #endregion


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
                var book = await db.Bookings.Include(e => e.ReservedEmployeer).Include(c => c.Comments).FirstOrDefaultAsync(b => b.Id == booking.Id);

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
