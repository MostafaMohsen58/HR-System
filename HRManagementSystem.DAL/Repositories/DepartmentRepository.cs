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
            return department.Id; 
        }

        public async Task<int> DeleteAsync(int id)
        {
            var department = await _context.Department.FindAsync(id);
            if (department == null)
                return 0; 

            _context.Department.Remove(department);
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
            var existingDepartment = await _context.Department.FindAsync(department.Id);
            if (existingDepartment == null)
                return null;
            
            existingDepartment.Name = department.Name;
            
            return existingDepartment;
        }
    }
}
