﻿using Microsoft.AspNetCore.Authorization;
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
            return View();
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
                var book = await db.Bookings.Include(c => c.Comments).FirstOrDefaultAsync(b => b.Id == booking.Id);

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
            return RedirectToAction("Account", "Developer");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmployeer(int? id)
        {
            if(id != null)
            {
                var employeer = await db.Users.FirstOrDefaultAsync(e => e.Id == id);

                await db.SaveChangesAsync();
            }

            return RedirectToAction("Account", "Developer");
        }

        [HttpGet]
        public async Task<IActionResult> Comments()
        {
            return RedirectToAction("Account", "Developer");
        }

        [HttpGet]
        public async Task<IActionResult> EmployeesWantToSignUp(int? id)
        {
            if(id != null)
            {
               
            }

            return RedirectToAction("Account", "Developer");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployeer(int? id)
        {
            if(id != null)
            {
                var employeer = await db.Users.FirstOrDefaultAsync(e => e.Id == id);

                db.Users.Remove(employeer);


                await db.SaveChangesAsync();
            }

            return RedirectToAction("Account", "Developer");
        }
    }
}
