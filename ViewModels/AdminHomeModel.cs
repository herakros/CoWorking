using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCoWorking.ViewModels
{
    public class AdminHomeModel
    {
        public int PlaceCount { get; set; }

        public int ApprovedBookingCount { get; set; }

        public int PendingApporvedCount { get; set; }

        public int UsersCount { get; set; }
    }
}
