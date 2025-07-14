using HRManagementSystem.DAL.Models;
using HRManagementSystem.DAL.Models.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.DAL.Data.Context
{
    public class HRContext : IdentityDbContext<ApplicationUser, Role, string> 

    {
        public HRContext(DbContextOptions<HRContext> options) : base(options)
        {}
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

            builder.Entity<Setting>().HasData(
                new Setting
                {
                    Id = 1,
                    Type = SettingType.Hour,
                    FirstHoliday = WeekDay.Thursday,
                    SecondHoliday = WeekDay.Friday,
                    OverTime = 50.00m,
                    Deduction = 25.00m
                }
            );
        }
    }
}
