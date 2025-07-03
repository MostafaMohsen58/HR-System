

using HRManagementSystem.BL.DTOs.AttendanceDTOs;
using HRManagementSystem.BL.Utilities;

namespace HRManagementSystem.BL.Interfaces
{
    public interface IAttendanceService
    {
        Task<int> AddAttendanceAsync(AttendanceDto attendanceDto);
        Task<AttendanceUpdateDto> UpdateAttendanceAsync(AttendanceUpdateDto attendanceUpdateDto);
        Task<int> DeleteAttendanceAsync(int id);
        Task<AttendanceUpdateDto> GetAttendanceByIdAsync(int id);
        Task<IEnumerable<AttendanceUpdateDto>> GetAllAttendancesAsync();
        Task<PaginatedList<AttendanceUpdateDto>> GetPaginatedAttendancesAsync(int pageIndex, int pageSize);
    }
}
