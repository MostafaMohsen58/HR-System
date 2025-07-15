using AutoMapper;
using HRManagementSystem.BL.DTOs.AttendanceDTOs;
using HRManagementSystem.BL.Interfaces;
using HRManagementSystem.BL.Utilities;
using HRManagementSystem.DAL.Interfaces;
using HRManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.BL.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IMapper _mapper;
        private readonly IPayRollService _payRollService;
        public AttendanceService(IAttendanceRepository attendanceRepository, IMapper mapper
            ,IPayRollService payRollService)
        {
            _attendanceRepository = attendanceRepository;
            _mapper = mapper;
            _payRollService = payRollService;
        }
        public async Task<int> AddAttendanceAsync(AttendanceDto attendanceDto)
        {
            if (attendanceDto == null)
                throw new ArgumentNullException(nameof(attendanceDto));

            ValidateAttendanceTimes(attendanceDto.ArrivalTime, attendanceDto.DepartureTime);

            bool isDuplicate = await _attendanceRepository.CheckDuplicate(attendanceDto.EmployeeId, attendanceDto.Date);
            if (isDuplicate)
                throw new InvalidOperationException("This employee already has an attendance record for the selected date.");

            try
            {
                var attendance = _mapper.Map<Attendance>(attendanceDto);
                var result = await _attendanceRepository.AddAsync(attendance);
                await _payRollService.AddPayRollAsync(attendanceDto.Date.Month,attendanceDto.Date.Year,attendance.EmployeeId,attendance.ArrivalTime, attendance.DepartureTime);
                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not add attendance record.", ex);
            }
        }

        public async Task DeleteAttendanceAsync(int id)
        {
            var existingAttendance = await _attendanceRepository.GetByIdAsync(id);
            await _payRollService.DeletePayRollAsync(existingAttendance.Date.Month, existingAttendance.Date.Year, existingAttendance.EmployeeId, existingAttendance.ArrivalTime, existingAttendance.DepartureTime);
            if (await _attendanceRepository.DeleteAsync(id) == 0)
                throw new KeyNotFoundException($"Attendance with Id {id} was not found.");
        }

        public async Task<IEnumerable<AttendanceUpdateDto>> GetAllAttendancesAsync()
        {
            try
            {
                var attendances = await _attendanceRepository.GetAllQueryable().ToListAsync();
                return _mapper.Map<IEnumerable<AttendanceUpdateDto>>(attendances);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not retrieve attendance records.", ex);
            }
        }

        public async Task<PaginatedList<AttendanceUpdateDto>> GetPaginatedAttendancesAsync(
            int pageIndex = 1,
            int pageSize = 5,
            string? searchTerm = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                var attendanceQuery = _attendanceRepository.GetAllQueryable();

                if(!string.IsNullOrEmpty(searchTerm))
                {
                    attendanceQuery = attendanceQuery
                    .Where(a =>
                        a.Employee.FullName.Contains(searchTerm)
                    );
                }

                if (startDate.HasValue)
                {
                    var normalizedStart = startDate.Value.Date;
                    attendanceQuery = attendanceQuery.Where(a => a.Date >= normalizedStart);
                }

                if (endDate.HasValue)
                {
                    var normalizedEnd = endDate.Value.Date.AddDays(1).AddTicks(-1);
                    attendanceQuery = attendanceQuery.Where(a => a.Date <= normalizedEnd);
                }
                if(startDate > endDate)
                {
                    throw new ArgumentException("Start date cannot be after end date");
                }
                attendanceQuery = attendanceQuery.OrderByDescending(a => a.Date);

                var paginatedAttendances = await PaginatedList<Attendance>.CreateAsync(attendanceQuery, pageIndex, pageSize);

                var mappedItems = _mapper.Map<List<AttendanceUpdateDto>>(paginatedAttendances.Items);

                return new PaginatedList<AttendanceUpdateDto>(
                    mappedItems,
                    paginatedAttendances.TotalItems,
                    paginatedAttendances.PageIndex,
                    paginatedAttendances.PageSize
                );
            }
            catch(ArgumentException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not retrieve paginated attendance records.", ex);
            }
        }

        public async Task<AttendanceUpdateDto> GetAttendanceByIdAsync(int id)
        {
            try
            {
                var attendance = await _attendanceRepository.GetByIdAsync(id);
                if (attendance == null)
                    throw new KeyNotFoundException($"Attendance with Id {id} was not found.");

                return _mapper.Map<AttendanceUpdateDto>(attendance);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not retrieve attendance with ID {id}.", ex);
            }
        }

        public async Task<AttendanceUpdateDto> UpdateAttendanceAsync(AttendanceUpdateDto attendanceUpdateDto)
        {
            if (attendanceUpdateDto == null)
                throw new ArgumentNullException(nameof(attendanceUpdateDto));

            var attendance = _mapper.Map<Attendance>(attendanceUpdateDto);
            ValidateAttendanceTimes(attendance.ArrivalTime, attendance.DepartureTime);

            bool isDuplicate = await _attendanceRepository.CheckDuplicate(attendanceUpdateDto.EmployeeId, attendanceUpdateDto.Date, attendanceUpdateDto.Id);
            if (isDuplicate)
                throw new InvalidOperationException("This employee already has an attendance record for the selected date.");

            //get attendence by id
            var existingAttendance = await GetAttendanceByIdAsync(attendanceUpdateDto.Id);

            var updatedAttendance = await _attendanceRepository.UpdateAsync(attendance);

            if (updatedAttendance == null)
                throw new ArgumentNullException(nameof(attendanceUpdateDto));

            
            // Update payroll 
            await _payRollService.UpdatePayRoll(existingAttendance.Date.Month,existingAttendance.Date.Year,attendanceUpdateDto.Date.Month, attendanceUpdateDto.Date.Year,attendanceUpdateDto.EmployeeId,existingAttendance.ArrivalTime, existingAttendance.DepartureTime, attendanceUpdateDto.ArrivalTime, attendanceUpdateDto.DepartureTime);

            return _mapper.Map<AttendanceUpdateDto>(updatedAttendance);
            
        }
        public async Task<IEnumerable<AttendanceUpdateDto>> GetAllFilteredAsync(string? searchTerm, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var query = _attendanceRepository.GetAllQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                    //query = query.Where(a => a.Employee.FullName.Contains(searchTerm));
                    query = query.Where(a => a.Employee.FullName.ToLower().StartsWith(searchTerm.ToLower()));

                if (startDate.HasValue)
                    query = query.Where(a => a.Date >= startDate.Value);

                if (endDate.HasValue)
                    query = query.Where(a => a.Date <= endDate.Value);

                var attendances = await query.OrderByDescending(a => a.Date).ToListAsync();
                return _mapper.Map<IEnumerable<AttendanceUpdateDto>>(attendances);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not retrieve filtered attendance records.", ex);
            }
        }
        public async Task<bool> CheckDuplicate(string employeeId, DateTime date, int? excludeId = null)
        {
            if(excludeId.HasValue)
            {
                return await _attendanceRepository.CheckDuplicate(employeeId, date, excludeId.Value);
            }
            return await _attendanceRepository.CheckDuplicate(employeeId, date);
        }
        public async Task<bool> CheckDuplicate(string employeeId, DateTime date)
        {
            return await _attendanceRepository.CheckDuplicate(employeeId, date);
        }

        private void ValidateAttendanceTimes(DateTime arrival, DateTime departure)
        {
            if (arrival >= departure)
                throw new ArgumentException("Check-in Time cannot be after Check-out Time");
        }

        public async Task<double> GetDailyAttendanceForUsersAsync()
        {
            return await _attendanceRepository.GetDailyAttendanceForUsersAsync();
        }


    }
}
