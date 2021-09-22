using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TestCoWorking.Models;

namespace TestCoWorking.ViewModels
{
    public class LookBookingModel
    {
        public Booking Booking { get; set; }

        [Required(ErrorMessage = "Напишіть коментар")]
        public string Text { get; set; }

        public int BookingId { get; set; }
    }
}
