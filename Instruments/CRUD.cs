using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCoWorking.Data;
using TestCoWorking.Models;

namespace TestCoWorking.Instruments
{
    public static class CRUD
    {
        public static async Task DeleteBook(Booking booking, ApplicationContext db)
        {
            var bookComment = await db.Bookings.Include(c => c.Comments).FirstOrDefaultAsync(b => b.Id == booking.Id);
            var bookUser = await db.Bookings.Include(u => u.ReservedUsers).FirstOrDefaultAsync(b => b.Id == booking.Id);

            foreach (var b in bookComment.Comments)
            {
                db.Entry(b).State = EntityState.Deleted;
            }

            db.SaveChanges();

            foreach (var u in bookUser.ReservedUsers)
            {
                u.Booking = null;
                u.BookingId = null;
            }

            db.SaveChanges();

            var book = await db.Bookings.FirstOrDefaultAsync(b => b.Id == booking.Id);
            db.Entry(book).State = EntityState.Deleted;

            await db.SaveChangesAsync();
        }
    }
}
