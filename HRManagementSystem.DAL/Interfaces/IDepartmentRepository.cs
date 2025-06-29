using HRManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.DAL.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<int> AddAsync(Department department);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department> GetByIdAsync(int id);
        Task<Department> UpdateAsync(Department department);
    }
}
