using HRManagementSystem.DAL.Data.Context;
using HRManagementSystem.DAL.Interfaces;
using HRManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.DAL.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HRContext _context;

        public DepartmentRepository(HRContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Department department)
        {
            await _context.Department.AddAsync(department);
            await _context.SaveChangesAsync();
            return department.Id; 
        }

        public async Task<int> DeleteAsync(int id)
        {
            var department = await _context.Department.FindAsync(id);
            if (department == null)
                return 0; // Return 0 to indicate no entity was found

            _context.Department.Remove(department);
            await _context.SaveChangesAsync();
            return department.Id;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Department
                .Include(e => e.Employees)
                .ToListAsync();
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            return await _context.Department.FindAsync(id);
        }

        public async Task<Department> UpdateAsync(Department department)
        {
            // Find the existing entity first
            var existingDepartment = await _context.Department.FindAsync(department.Id);
            if (existingDepartment == null)
                return null;
                
            // Update properties manually
            existingDepartment.Name = department.Name;
            
            // Save changes
            await _context.SaveChangesAsync();
            return existingDepartment;
        }
    }
}
