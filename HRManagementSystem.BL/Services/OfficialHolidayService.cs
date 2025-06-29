using AutoMapper;
using HRManagementSystem.BL.DTOs;
using HRManagementSystem.BL.Interfaces;
using HRManagementSystem.DAL.Interfaces;
using HRManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.Services
{
    public class OfficialHolidayService : IOfficialHolidayService
    {
        private readonly IOfficialHolidayRepository _officialHolidayRepository;
        private readonly IMapper _mapper;
        
        public OfficialHolidayService(IOfficialHolidayRepository officialHolidayRepository, IMapper mapper)
        {
            _officialHolidayRepository = officialHolidayRepository;
            _mapper = mapper;
        }
        
        public async Task<int> AddOfficialHolidayAsync(OfficialHolidayDto officialHolidayDto)
        {
            // Map DTO to entity
            var officialHoliday = _mapper.Map<OfficialHoliday>(officialHolidayDto);
            
            // Call repository
            return await _officialHolidayRepository.AddAsync(officialHoliday);
        }

        public async Task<int> DeleteOfficialHolidayAsync(int id)
        {
            // Get the holiday first to check if it exists
            var holiday = await _officialHolidayRepository.GetByIdAsync(id);
            if (holiday == null)
                throw new KeyNotFoundException($"Official Holiday with Id {id} not found.");
                
            // Delete if exists
            return await _officialHolidayRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<OfficialHolidayUpdateDto>> GetAllOfficialHolidaysAsync()
        {
            // Get all holidays from repository
            var holidays = await _officialHolidayRepository.GetAllAsync();
            
            // Map to DTOs and return
            return _mapper.Map<IEnumerable<OfficialHolidayUpdateDto>>(holidays);
        }

        public async Task<OfficialHolidayUpdateDto> GetOfficialHolidayByIdAsync(int id)
        {
            // Get holiday from repository
            var holiday = await _officialHolidayRepository.GetByIdAsync(id);
            
            // Check if holiday exists
            if (holiday == null)
                throw new KeyNotFoundException($"Official Holiday with Id {id} not found.");
                
            // Map to DTO and return
            return _mapper.Map<OfficialHolidayUpdateDto>(holiday);
        }

        public async Task<OfficialHolidayUpdateDto> UpdateOfficialHolidayAsync(OfficialHolidayUpdateDto officialHolidayDto)
        {
            // Map DTO to entity
            var officialHoliday = _mapper.Map<OfficialHoliday>(officialHolidayDto);
            
            // Update entity
            var updatedHoliday = await _officialHolidayRepository.UpdateAsync(officialHoliday);
            
            if (updatedHoliday == null)
                throw new KeyNotFoundException($"Official Holiday with ID {officialHolidayDto.Id} not found.");
                
            // Map back to DTO and return
            return _mapper.Map<OfficialHolidayUpdateDto>(updatedHoliday);
        }
    }
}
