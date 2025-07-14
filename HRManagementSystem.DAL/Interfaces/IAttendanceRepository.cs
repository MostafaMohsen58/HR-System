using HRManagementSystem.DAL.Models;

namespace HRManagementSystem.DAL.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<int> AddAsync(Attendance attendance);
        Task<bool> CheckDuplicate(string employeeId, DateTime date);
        Task<bool> CheckDuplicate(string employeeId, DateTime date, int excludeId);
        Task<int> DeleteAsync(int id);
        IQueryable<Attendance> GetAllQueryable();
        Task<Attendance> GetByIdAsync(int id);
        Task<Attendance> UpdateAsync(Attendance attendance);
        
        Task<double> GetDailyAttendanceForUsersAsync();


    }
}
