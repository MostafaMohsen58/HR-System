using HRManagementSystem.DAL.Data.Context;
using HRManagementSystem.DAL.Interfaces;
using HRManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HRManagementSystem.DAL.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly HRContext _context;
        public AttendanceRepository(HRContext context) => _context = context;

        public async Task<int> AddAsync(Attendance attendance)
        {
            await _context.Attendance.AddAsync(attendance);
            return attendance.Id;
        }
        public async Task<bool> CheckDuplicate(string employeeId, DateTime date)
        {
            return await _context.Attendance
                .AnyAsync(a => a.EmployeeId == employeeId && a.Date.Date == date.Date);
        }
        public async Task<bool> CheckDuplicate(string employeeId, DateTime date, int excludeId)
        {
            return await _context.Attendance
                .AnyAsync(a => a.EmployeeId == employeeId && a.Date.Date == date.Date && a.Id != excludeId);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var rowsAffected = await _context.Attendance.Where(a => a.Id == id).ExecuteDeleteAsync();
            
            return rowsAffected;
        }

        public IQueryable<Attendance> GetAllQueryable() => _context.Attendance.Include(a => a.Employee).ThenInclude(e => e.Department).AsQueryable();
        
        public async Task<Attendance> GetByIdAsync(int id) => await _context.Attendance.Include(a => a.Employee).ThenInclude(e => e.Department).FirstOrDefaultAsync(a => a.Id == id);

        public async Task<Attendance> UpdateAsync(Attendance attendance)
        {
            Attendance ExistingAttendance = await GetByIdAsync(attendance.Id);
            if (ExistingAttendance == null) return null;

            var employeeExists = await _context.Users.AnyAsync(a => a.Id == attendance.EmployeeId);
            if (!employeeExists) throw new ArgumentException("EmployeeId does not exist.");

            ExistingAttendance.EmployeeId = attendance.EmployeeId;
            ExistingAttendance.ArrivalTime = attendance.ArrivalTime;
            ExistingAttendance.DepartureTime = attendance.DepartureTime;
            ExistingAttendance.Date = attendance.Date;

            return ExistingAttendance;
            
        }

        public async Task<double> GetDailyAttendanceForUsersAsync()
        {
            DateTime today = DateTime.Today;

            var count = await (
                from attendance in _context.Attendance
                join userRole in _context.UserRoles on attendance.EmployeeId equals userRole.UserId
                join role in _context.Roles on userRole.RoleId equals role.Id
                where role.Name == "User" && attendance.Date.Date == today
                select attendance.EmployeeId
            ).Distinct().CountAsync();

            return count;
        }


    }
}
