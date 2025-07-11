using HRManagementSystem.DAL.Models;

namespace HRManagementSystem.DAL.Interfaces
{
    public interface ISettingRepository
    {
        Task<int> AddAsync(Setting s);
        Task<Setting> GetByIdAsync(int id=1);
        Task<Setting> UpdateAsync(Setting setting);
    }
}
