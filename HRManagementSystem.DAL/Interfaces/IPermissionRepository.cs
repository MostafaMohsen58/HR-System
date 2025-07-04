using HRManagementSystem.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPermissionRepository
{
    Task<List<Permission>> GetAllAsync();
    Task<Permission> GetByIdAsync(int id);
    Task AddAsync(Permission permission);
    Task UpdateAsync(Permission permission);
    Task DeleteAsync(int id);
}
