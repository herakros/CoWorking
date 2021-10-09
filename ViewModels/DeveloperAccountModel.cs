using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCoWorking.Models;

namespace TestCoWorking.ViewModels
{
    public class DeveloperAccountModel
    {
        public Booking Booking { get; set; }

        public int CommentsCount { get; set; }

        public int FollowedUsersCount { get; set; }

        public int EmployeerWantSignCount { get; set; }
    }
}
