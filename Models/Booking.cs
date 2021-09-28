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

        public string Description { get; set; }

        public bool Approved { get; set; }

        public User User { get; set; }

        public int? UserId { get; set; }

        public List<Comment> Comments { get; set; }

        public Booking()
        {
            Comments = new List<Comment>();
        }
    }
}
