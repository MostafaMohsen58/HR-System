using HRManagementSystem.DAL.Data.Context;
using HRManagementSystem.DAL.Models;
using HRManagementSystem.DAL.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HRManagementSystem.DAL.Data
{
    public class DbSeeder
    {
        // 1. Seed Departments
        private static async Task SeedDepartmentsAsync(HRContext context)
        {
            if (!context.Department.Any())
            {
                context.Department.AddRange(
                    new Department { Name = "Human Resources" },
                    new Department { Name = "Marketing" },
                    new Department { Name = "IT" },
                    new Department { Name = "Finance" }
                );
                await context.SaveChangesAsync();
            }
        }

        // 2. Seed Roles
        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "HR", "Employee", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        // 3. Seed Users
        private static async Task SeedUsersAsync(HRContext context, UserManager<ApplicationUser> userManager)
        {
            if (await userManager.FindByNameAsync("reem.atef") == null)
            {
                var user1 = new ApplicationUser
                {
                    UserName = "reem.atef",
                    Email = "reem.atef@example.com",
                    FullName = "Reem Atef",
                    Nationality = "Egyptian",
                    NationalId = "29805231234567",
                    Gender = Gender.Female,
                    Address = "Cairo, Egypt",
                    DateOfBirth = new DateTime(1999, 4, 19),
                    ContractDate = new DateTime(2022, 1, 1),
                    StartTime = new DateTime(1, 1, 1, 8, 0, 0),
                    EndTime = new DateTime(1, 1, 1, 16, 0, 0),
                    Salary = 15000.00m,
                    DepartmentId = 1,
                    EmailConfirmed = false
                };

                var result = await userManager.CreateAsync(user1, "P@ssword123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user1, "HR");
                }
            }

            if (await userManager.FindByNameAsync("sara.kamel") == null)
            {
                var user2 = new ApplicationUser
                {
                    UserName = "sara.kamel",
                    Email = "sara.kamel@example.com",
                    FullName = "Sara Kamel",
                    Nationality = "Egyptian",
                    NationalId = "29904151234567",
                    Gender = Gender.Female,
                    Address = "Alexandria, Egypt",
                    DateOfBirth = new DateTime(1999, 5, 23),
                    ContractDate = new DateTime(2021, 6, 10),
                    StartTime = new DateTime(1, 1, 1, 8, 0, 0),
                    EndTime = new DateTime(1, 1, 1, 16, 0, 0),
                    Salary = 17500.00m,
                    DepartmentId = 2,
                    EmailConfirmed = false,
                };

                var result = await userManager.CreateAsync(user2, "P@ssword123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user2, "User");
                }
            }
        }

        // 4. Seed Permissions
        private static async Task SeedPermissionsAsync(HRContext context)
        {
            if (!context.Permissions.Any())
            {
                var permissions = new List<Permission>
                {
                    new Permission { Page = "Dashboard", IsView = true, IsAdd = true, IsEdit = true, IsDelete = true },
                    new Permission { Page = "Users", IsView = true, IsAdd = true, IsEdit = true, IsDelete = true },
                    new Permission { Page = "Employees", IsView = true, IsAdd = true, IsEdit = true, IsDelete = true },
                    new Permission { Page = "Attendance", IsView = true, IsAdd = true, IsEdit = true, IsDelete = true },
                    new Permission { Page = "Report", IsView = true, IsAdd = true, IsEdit = true, IsDelete = true },
                    new Permission { Page = "Holidays", IsView = true, IsAdd = true, IsEdit = true, IsDelete = true },
                    new Permission { Page = "Settings", IsView = true, IsAdd = true, IsEdit = true, IsDelete = true }
                };

                context.Permissions.AddRange(permissions);
                await context.SaveChangesAsync();
            }
        }

        // 5. Seed Role Claims
        private static async Task SeedRoleClaimsAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = await roleManager.Roles.ToListAsync();

            foreach (var role in roles)
            {
                if (role.Name == "HR")
                {
                    await roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("Permission", "View Employees"));
                    await roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("Permission", "Edit Employees"));
                    await roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("Permission", "Delete Employees"));
                    await roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("Permission", "View Departments"));
                    await roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("Permission", "Edit Departments"));
                    await roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("Permission", "Delete Departments"));
                }
                else if (role.Name == "User")
                {
                    await roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("Permission", "View Employees"));
                }
            }
        }

        // 5. Public method to run all seeds
        public static async Task SeedAllAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<HRContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await SeedDepartmentsAsync(context);
            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(context, userManager);
            await SeedPermissionsAsync(context);
            await SeedRoleClaimsAsync(roleManager);
        }
    }
}
