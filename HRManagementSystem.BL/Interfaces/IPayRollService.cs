using HRManagementSystem.BL.DTOs.OfficialHoliday;
using HRManagementSystem.BL.DTOs.PayRoll;
using HRManagementSystem.BL.Utilities;
using HRManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.Interfaces
{
    public interface IPayRollService
    {
        Task<int> AddPayRollAsync(int month, int year, string employeeId, DateTime checkIn, DateTime checkOut);
        //Task<PayRoll> UpdatePayRollAsync(PayRoll payRoll);
        Task<List<PayRoll>> GetAllPayRollsAsync();

        Task<int> UpdatePayRoll(int oldMonth, int oldYear, int month, int year, string employeeId, DateTime oldCheckIn, DateTime oldCheckOut, DateTime checkIn, DateTime checkOut);

        Task<int> DeletePayRollAsync(int month, int year, string employeeId, DateTime checkIn, DateTime checkOut);
        Task<PaginatedList<PayRollDto>> GetPaginatedAttendancesAsync(int pageIndex, int pageSize, string? searchTerm, int? month, int? year);

        Task FinalizeHolidaySalariesAsync(int month, int year);
    }
}
