using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCoWorking.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string DevEmail { get; set; }

        public int UserCount { get; set; }

        public List<Comment> Comments { get; set; }

        public List<User> ReservedUsers { get; set; }

        public Booking()
        {
            Comments = new List<Comment>();
            ReservedUsers = new List<User>();
        }
    }
}
