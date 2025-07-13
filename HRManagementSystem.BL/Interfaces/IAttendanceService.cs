

using HRManagementSystem.BL.DTOs.AttendanceDTOs;
using HRManagementSystem.BL.Utilities;

namespace HRManagementSystem.BL.Interfaces
{
    public interface IAttendanceService
    {
        Task<int> AddAttendanceAsync(AttendanceDto attendanceDto);
        Task<AttendanceUpdateDto> UpdateAttendanceAsync(AttendanceUpdateDto attendanceUpdateDto);
        Task DeleteAttendanceAsync(int id);
        Task<AttendanceUpdateDto> GetAttendanceByIdAsync(int id);
        Task<IEnumerable<AttendanceUpdateDto>> GetAllAttendancesAsync();
        Task<PaginatedList<AttendanceUpdateDto>> GetPaginatedAttendancesAsync(int pageIndex, int pageSize, string? searchTerm, DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<AttendanceUpdateDto>> GetAllFilteredAsync(string? searchTerm, DateTime? startDate, DateTime? endDate);
        Task<bool> CheckDuplicate(string employeeId, DateTime date);
        Task<bool> CheckDuplicate(string employeeId, DateTime date, int? excludeId);
        Task<double> GetAverageDailyAttendanceForUsersAsync();


    }
}
