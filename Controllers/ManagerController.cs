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
    [Authorize(Roles = "manager")]
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
                var book = await db.Bookings.FirstOrDefaultAsync(b => b.Id == id);

                if(book != null)
                {
                    return View(book);
                }
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Follow(int? id)
        {
            if(id != null)
            {
                var book = await db.Bookings.FirstOrDefaultAsync(b => b.Id == id);
                var user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

                if(CheckFollowed(user, book) && book.UserCount != 0)
                {
                    user.Booking = book;

                    db.Users.Update(user);

                    await db.SaveChangesAsync();

                    book.UserCount--;

                    await db.SaveChangesAsync();

                    return RedirectToAction("Account", "Manager");
                }
            }

            return RedirectToAction("Account", "Manager"); 
        }

        private bool CheckFollowed(User user, Booking booking)
        {
            return user.BookingId == booking.Id ? false : true;
        }


        [HttpGet]
        public async Task<IActionResult> Comments(int? id)
        {
            if(id != null)
            {
                var book = await db.Bookings.Include(b => b.Comments).FirstOrDefaultAsync(b => b.Id == id);

                if(book != null)
                {
                    return View(book);
                }
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> AddComment(int? id)
        {
            if (id != null)
            {
                var book = await db.Bookings.Include(b => b.Comments).FirstOrDefaultAsync(b => b.Id == id);

                if (book != null)
                {
                    var comment = new Comment() { Booking = book };
                    db.Comments.Add(comment);
                    await db.SaveChangesAsync();

                    return View(comment);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comm)
        {            
            if(comm != null)
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
                var comment = await db.Comments.FirstOrDefaultAsync(c => c.Id == comm.Id);

                comment.UserName = user.NickName;
                comment.Message = comm.Message;

                db.Comments.Update(comment);

                await db.SaveChangesAsync();

                return RedirectToAction("Account", "Manager");
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult Account()
        {
            var booking = db.Bookings.ToList();
            return View(booking);
        }
    }
}
