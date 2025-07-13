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
    }
}
