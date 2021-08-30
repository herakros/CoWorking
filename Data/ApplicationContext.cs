using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCoWorking.Models;

namespace TestCoWorking.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Booking> Bookings {get;set;}

        public DbSet<Comment> Comments { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string devRoleName = "dev";
            string managerRoleName = "manager";

            string adminEmail = "Admin@gmail.com";
            string adminPassword = "12345";
            string adminNickName = "herakros";

            // Added needed roles
            Role adminRole = new Role() { Id = 1, Name = adminRoleName };
            Role devRole = new Role() { Id = 2, Name = devRoleName };
            Role managerRole = new Role() { Id = 3, Name = managerRoleName };

            // Creating basic admin
            User adminUser = new User() { Id = 1, Email = adminEmail, Password = adminPassword, NickName = adminNickName, RoleId = 1 };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, devRole, managerRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });

            base.OnModelCreating(modelBuilder);
        }
    }
}
