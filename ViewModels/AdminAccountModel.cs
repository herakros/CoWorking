using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCoWorking.ViewModels
{
    public class AdminAccountModel
    {
        public int UserCount { get; set; }

        public int BookingCount { get; set; }

        public int BookingApprovedCount { get; set; }

        public int BookingPendingApprovedCount { get; set; }
    }
}
