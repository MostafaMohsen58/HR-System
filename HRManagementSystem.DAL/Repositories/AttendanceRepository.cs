using HRManagementSystem.DAL.Data.Context;
using HRManagementSystem.DAL.Interfaces;
using HRManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.DAL.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly HRContext _context;
        public AttendanceRepository(HRContext context) => _context = context;

        public async Task<int> AddAsync(Attendance attendance)
        {
            await _context.Attendance.AddAsync(attendance);
            await _context.SaveChangesAsync();
            return attendance.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            Attendance attendance = await GetByIdAsync(id);
            if (attendance == null)
                return -1;
            _context.Attendance.Remove(attendance);
            await _context.SaveChangesAsync();
            return attendance.Id;
        }

        public IQueryable<Attendance> GetAllQueryable() => _context.Attendance.Include(a => a.Employee).AsQueryable();
        
        public async Task<Attendance> GetByIdAsync(int id) => await _context.Attendance.Include(a => a.Employee).FirstOrDefaultAsync(a => a.Id == id);

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

            await _context.SaveChangesAsync();
            return ExistingAttendance;
            
        }
    }
}
