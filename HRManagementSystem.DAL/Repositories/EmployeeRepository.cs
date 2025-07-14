using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRManagementSystem.DAL.Data.Context;
using HRManagementSystem.DAL.Interfaces;
using HRManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore; 

namespace HRManagementSystem.DAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HRContext _context;

        public EmployeeRepository(HRContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _context.Users
                .Include(e => e.Department)
                .ToListAsync();
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await _context.Users
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> UpdateAsync(ApplicationUser user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(ApplicationUser user)
        {
            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<ApplicationUser>> GetEmployeesByDepartmentIdAsync(int departmentId)
        {
            return await _context.Users
                .Include(e => e.Department)
                .Where(e => e.DepartmentId == departmentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetEmployeesByDepartmentNameAsync(string departmentName)
        {
                return await _context.Users
                .Include(e => e.Department)
                .Where(e => e.Department != null && e.Department.Name == departmentName)
                .ToListAsync();
        }


        public async Task<int> GetTotalCountAsync()
        {
            var userRoleId = await _context.Roles
                .Where(r => r.Name == "User")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            return await _context.Users
                .Where(u => _context.UserRoles
                    .Any(ur => ur.UserId == u.Id && ur.RoleId == userRoleId))
                .CountAsync();
        }

        public async Task<List<IGrouping<string, ApplicationUser>>> GroupEmployeesByDepartmentAsync()
        {
            var userRoleId = await _context.Roles
                .Where(r => r.Name == "User")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            var users = await _context.Users
                .Include(u => u.Department)
                .Where(u => _context.UserRoles
                    .Any(ur => ur.UserId == u.Id && ur.RoleId == userRoleId))
                .ToListAsync();

            return users
                .GroupBy(u => u.Department != null ? u.Department.Name : "No Department")
                .ToList();
        }

        public async Task<List<IGrouping<string, ApplicationUser>>> GroupEmployeesByGenderAsync()
        {
            var userRoleId = await _context.Roles
                .Where(r => r.Name == "User")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            var users = await _context.Users
                .Where(u => _context.UserRoles
                    .Any(ur => ur.UserId == u.Id && ur.RoleId == userRoleId))
                .ToListAsync();

            return users
                .GroupBy(u => u.Gender.ToString())
                .ToList();
        }

        public async Task<decimal> GetAverageSalaryAsync()
        {
            var userRoleId = await _context.Roles
                .Where(r => r.Name == "User")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            return await _context.Users
                .Where(u => _context.UserRoles
                    .Any(ur => ur.UserId == u.Id && ur.RoleId == userRoleId))
                .AverageAsync(u => u.Salary);
        }

        public async Task<List<IGrouping<string, ApplicationUser>>> GroupEmployeesByAgeGroupAsync()
        {
            var userRoleId = await _context.Roles
                .Where(r => r.Name == "User")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            var users = await _context.Users
                .Where(u => _context.UserRoles
                    .Any(ur => ur.UserId == u.Id && ur.RoleId == userRoleId))
                .ToListAsync();

            var today = DateTime.Today;

            return users
                .GroupBy(u =>
                {
                    var age = today.Year - u.DateOfBirth.Year;
                    if (u.DateOfBirth > today.AddYears(-age)) age--;

                    if (age < 25) return "Less than 25";
                    else if (age <= 35) return "25-35";
                    else if (age <= 45) return "36-45";
                    else return "Above 45";
                })
                .ToList();
        }

        public async Task<List<IGrouping<string, ApplicationUser>>> GroupEmployeesByNationalityAsync()
        {
            var userRoleId = await _context.Roles
                .Where(r => r.Name == "User")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            var users = await _context.Users
                .Where(u => _context.UserRoles
                    .Any(ur => ur.UserId == u.Id && ur.RoleId == userRoleId))
                .ToListAsync();

            return users
                .GroupBy(u => u.Nationality)
                .ToList();
        }

     





    }
}
