using HRManagementSystem.DAL.Models;

namespace HRManagementSystem.DAL.Interfaces
{
    public interface ISettingRepository
    {
        Task<int> AddAsync(Setting department);
        Task<Setting> GetByIdAsync(int id);
        Task<Setting> UpdateAsync(Setting setting);
    }
}
