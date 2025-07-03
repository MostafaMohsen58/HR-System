using HRManagementSystem.DAL.Models;
using HRManagementSystem.DAL.Models.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
            builder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "IT" },
                new Department { Id = 2, Name = "HR" }
            );

            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "b2d5f000-165f-4c97-9eba-0c8779320d47",
                    UserName = "admin@example.com",
                    NormalizedUserName = "ADMIN@EXAMPLE.COM",
                    Email = "admin@example.com",
                    NormalizedEmail = "ADMIN@EXAMPLE.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEHy6rJ5zC8pPj3JZx5mZ7XhY9W7vV2lKjXhY7W8z5qKt1vY2bN3cXpLkY9wZ4g==",
                    SecurityStamp = "SecurityStamp",
                    ConcurrencyStamp = "ConcurrencyStamp",
                    PhoneNumber = "01012345678",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    FullName = "Admin User",
                    Nationality = "Egyptian",
                    Salary = 15000m,
                    NationalId = "29810000000000",
                    Gender = Gender.Male,
                    Address = "123 Main St, Cairo",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    ContractDate = new DateTime(2020, 1, 1),
                    StartTime = new DateTime(1, 1, 1, 8, 0, 0),
                    EndTime = new DateTime(1, 1, 1, 16, 0, 0),
                    DepartmentId = 2
                }
            );

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
