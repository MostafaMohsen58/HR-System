using HRManagementSystem.BL.DTOs.OfficialHoliday;
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
        Task<int> AddOfficialHolidayAsync(OfficialHolidayDto officialHolidayDto);
        Task<OfficialHolidayUpdateDto> UpdateOfficialHolidayAsync(OfficialHolidayUpdateDto officialHolidayDto);
        Task<int> DeleteOfficialHolidayAsync(int id);
        Task<OfficialHolidayUpdateDto> GetOfficialHolidayByIdAsync(int id);
        Task<IEnumerable<OfficialHolidayUpdateDto>> GetAllOfficialHolidaysAsync();
    }
}
