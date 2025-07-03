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
        public AttendanceService(IAttendanceRepository attendanceRepository, IMapper mapper)
        {
            _attendanceRepository = attendanceRepository;
            _mapper = mapper;
        }
        public async Task<int> AddAttendanceAsync(AttendanceDto attendanceDto)
        {
            if (attendanceDto == null)
                throw new ArgumentNullException(nameof(attendanceDto));

            if (attendanceDto.ArrivalTime > attendanceDto.DepartureTime)
                throw new ArgumentException("Arrival time cannot be after departure time");

            try
            {
                var attendance = _mapper.Map<Attendance>(attendanceDto);
                return await _attendanceRepository.AddAsync(attendance);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not add attendance record.", ex);
            }
        }

        public async Task DeleteAttendanceAsync(int id)
        {
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

        public async Task<PaginatedList<AttendanceUpdateDto>> GetPaginatedAttendancesAsync(int pageIndex = 1, int pageSize = 5)
        {
            try
            {
                var attendanceQuery = _attendanceRepository.GetAllQueryable();

                var paginatedAttendances = await PaginatedList<Attendance>.CreateAsync(attendanceQuery, pageIndex, pageSize);

                var mappedItems = _mapper.Map<List<AttendanceUpdateDto>>(paginatedAttendances);

                return new PaginatedList<AttendanceUpdateDto>(
                    mappedItems,
                    paginatedAttendances.Count,
                    paginatedAttendances.PageIndex,
                    pageSize
                );
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
            try
            {
                var attendance = _mapper.Map<Attendance>(attendanceUpdateDto);
                var updatedAttendance = await _attendanceRepository.UpdateAsync(attendance);

                if (updatedAttendance == null)
                    throw new ArgumentNullException(nameof(attendanceUpdateDto));

                return _mapper.Map<AttendanceUpdateDto>(updatedAttendance);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not update attendance with ID {attendanceUpdateDto.Id}.", ex);
            }
        }
    }
}
