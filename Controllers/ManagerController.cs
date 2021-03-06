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
            var employee = new AddEmployeeModel() { BookingId = book.Id };

            if(book != null)
            {
                return View(employee);
            }

            return RedirectToAction("Account", "Manager");
        }

        [HttpPost]
        public async Task<IActionResult> Follow(AddEmployeeModel model)
        {
            if(ModelState.IsValid)
            {
                var employee = new Employee()
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Description = model.Description,
                    BookingId = model.BookingId,
                    Booking = await db.Bookings.FirstOrDefaultAsync(b => b.Id == model.BookingId)
                };

                db.Employees.Add(employee);
                await db.SaveChangesAsync();

                return RedirectToAction("Look", "Manager", new { id = model.BookingId });
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(LookBookingModel model)
        {            
            if(ModelState.IsValid)
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

                var comment = new Comment() { UserName = user.NickName, BookingId = model.BookingId, Message = model.Text };

                db.Comments.Add(comment);

                await db.SaveChangesAsync();

                model.Text = string.Empty;
            }

            return View("Look", new LookBookingModel() 
            { 
                Booking = await db.Bookings.Include(c => c.Comments).FirstOrDefaultAsync(b => b.Id == model.BookingId),
                BookingId = model.BookingId
            });
        }

        [HttpGet]
        public async Task<IActionResult> Account()
        {
            var booking = await db.Bookings.Where(b => b.Approved == true).ToArrayAsync();
            return View(booking);
        }
    }
}
