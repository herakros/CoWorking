using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCoWorking.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public bool Appoved { get; set; }

        public string Description { get; set; }

        public Booking Booking { get; set; }

        public int? BookingId { get; set; }
    }
}
