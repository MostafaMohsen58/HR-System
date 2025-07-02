using HRManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.DAL.Data.Context
{
    public class HRContext : IdentityDbContext<ApplicationUser, Role, string> 

    {
        public HRContext(DbContextOptions<HRContext> options) : base(options)
        {

        }
        public DbSet<Department> Department { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<PayRoll> PayRoll { get; set; }
        public DbSet<OfficialHoliday> OfficialHoliday { get; set; }
        public DbSet<Setting> Setting { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Attendance>().HasData(
                new Attendance
                {
                    Id = 1,
                    Date = new DateTime(2025, 7, 1),
                    ArrivalTime = new DateTime(2025, 7, 1, 8, 30, 0),
                    DepartureTime = new DateTime(2025, 7, 1, 16, 30, 0),
                    EmployeeId = "b2d5f000-165f-4c97-9eba-0c8779320d47"
                },
                    new Attendance
                    {
                        Id = 2,
                        Date = new DateTime(2025, 7, 2),
                        ArrivalTime = new DateTime(2025, 7, 2, 9, 0, 0),
                        DepartureTime = new DateTime(2025, 7, 2, 17, 0, 0),
                        EmployeeId = "b2d5f000-165f-4c97-9eba-0c8779320d47"
                    }
                );
                        }
    }
}
