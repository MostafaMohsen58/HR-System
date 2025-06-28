using HRManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.Interfaces
{
    public interface IOfficialHolidayService
    {
        Task<int> AddOfficialHolidayAsync(OfficialHoliday officialHoliday);
        Task<OfficialHoliday> UpdateOfficialHolidayAsync(OfficialHoliday officialHoliday);
        Task<int> DeleteOfficialHolidayAsync(int id);
        Task<OfficialHoliday> GetOfficialHolidayByIdAsync(int id);
        Task<IEnumerable<OfficialHoliday>> GetAllOfficialHolidaysAsync();
    }
}
