using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TestCoWorking.Data;
using TestCoWorking.Models;
using TestCoWorking.ViewModels;

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

        #region Edit Users

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

                return View(editUserModel);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Id == model.Id);

                user.NickName = model.NickName;
                user.Email = model.Email;

                Role role = await db.Roles.FirstOrDefaultAsync(r => r.Name == model.Role);
                user.Role = role;

                db.Users.Update(user);
                await db.SaveChangesAsync();             

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

        #endregion

        #region Edit Booking

        [HttpGet]
        public async Task<IActionResult> EditBooking(int? id)
        {
            if (id != null)
            {
                var book = await db.Bookings.FirstOrDefaultAsync(b => b.Id == id);

                if (book != null)
                {
                    EditBookingModel model = new EditBookingModel()
                    {
                        Id = id,
                        Name = book.Name,
                        DevEmail = book.DevEmail,
                        EmployeerCount = book.EmployeerCount,
                        Start = book.Start,
                        End = book.End
                    };

                    return View(model);
                }
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditBooking(EditBookingModel model)
        {
            if (ModelState.IsValid)
            {
                var book = await db.Bookings.FirstOrDefaultAsync(b => b.Id == model.Id);

                book.Name = model.Name;
                book.EmployeerCount = (int)model.EmployeerCount;
                book.DevEmail = model.DevEmail;
                book.Start = model.Start;
                book.End = model.End;

                db.Bookings.Update(book);
                await db.SaveChangesAsync();

                return RedirectToAction("ApprovedBooking", "Admin");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBooking(int? id)
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

        [HttpPost]
        public async Task<IActionResult> DeleteBooking(Booking booking)
        {
            if (booking != null)
            {
                var book = await db.Bookings.Include(e => e.ReservedEmployeer).Include(c => c.Comments).FirstOrDefaultAsync(b => b.Id == booking.Id);

                db.Bookings.Remove(book);

                await db.SaveChangesAsync();

                return RedirectToAction("ApprovedBooking", "Admin");
            }

            return NotFound();
        }

        #endregion

        public async Task<IActionResult> Account()
        {
            var booking = await db.Bookings.ToListAsync();

            AdminAccountModel model = new AdminAccountModel()
            {
                UserCount = db.Users.Count(),
                BookingCount = booking.Count,
                BookingApprovedCount = booking.Where(b => b.Approved == true).Count(),
                BookingPendingApprovedCount = booking.Where(b => b.Approved == false).Count()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            return View(await db.Users.Include(r => r.Role).ToArrayAsync());
        }

        [HttpGet]
        public async Task<IActionResult> ApprovedBooking()
        {
            return View(await db.Bookings.Where(b => b.Approved == true).ToArrayAsync());
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

            return RedirectToAction("PendingApproved", "Admin");
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
