using HRManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.DAL.Interfaces
{
    public interface IOfficialHolidayRepository
    {
        Task<int> AddAsync(OfficialHoliday officialHoliday);
        Task<OfficialHoliday> UpdateAsync(OfficialHoliday officialHoliday);
        Task<int> DeleteAsync(int id);
        Task<OfficialHoliday> GetByIdAsync(int id);
        Task<IEnumerable<OfficialHoliday>> GetAllAsync();

    }
}
