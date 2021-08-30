using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCoWorking.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public int? BookingId { get; set; }

        public Booking Booking { get; set; }
    }
}
