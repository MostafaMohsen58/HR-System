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
    public class HRContext : IdentityDbContext<ApplicationUser>
    {
        public HRContext(DbContextOptions<HRContext> options) : base(options)
        {

        }
        public DbSet<Department> Department { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<PayRoll> PayRoll { get; set; }
        public DbSet<OfficialHoliday> OfficialHoliday { get; set; }
        public DbSet<Setting> Setting { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
