﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCoWorking.Models
{
    public class Place
    {
        public int Id { get; set; }

        public int? Count { get; set; }

        public List<Booking> Bookings { get; set; }

        public Place()
        {
            Bookings = new List<Booking>();
        }
    }
}
