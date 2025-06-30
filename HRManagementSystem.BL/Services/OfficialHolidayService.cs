using AutoMapper;
using HRManagementSystem.BL.DTOs.OfficialHoliday;
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
            var officialHoliday = _mapper.Map<OfficialHoliday>(officialHolidayDto);
            
            return await _officialHolidayRepository.AddAsync(officialHoliday);
        }

        public async Task<OfficialHolidayUpdateDto> GetOfficialHolidayByIdAsync(int id)
        {
            var holiday = await _officialHolidayRepository.GetByIdAsync(id);

            if (holiday == null)
                throw new KeyNotFoundException($"Official Holiday with Id {id} not found.");

            return _mapper.Map<OfficialHolidayUpdateDto>(holiday);
        }

        public async Task<OfficialHolidayUpdateDto> UpdateOfficialHolidayAsync(OfficialHolidayUpdateDto officialHolidayDto)
        {
            var officialHoliday = _mapper.Map<OfficialHoliday>(officialHolidayDto);

            var updatedHoliday = await _officialHolidayRepository.UpdateAsync(officialHoliday);

            if (updatedHoliday == null)
                throw new KeyNotFoundException($"Official Holiday with ID {officialHolidayDto.Id} not found.");

            return _mapper.Map<OfficialHolidayUpdateDto>(updatedHoliday);
        }

        public async Task<int> DeleteOfficialHolidayAsync(int id)
        {
            var idDeleted = await _officialHolidayRepository.DeleteAsync(id);

            if (idDeleted == 0)
                throw new KeyNotFoundException($"Official Holiday with Id {id} not found.");

            return idDeleted;
        }

        public async Task<IEnumerable<OfficialHolidayUpdateDto>> GetAllOfficialHolidaysAsync()
        {
            var holidays = await _officialHolidayRepository.GetAllAsync();
            
            return _mapper.Map<IEnumerable<OfficialHolidayUpdateDto>>(holidays);
        }

        

        
    }
}
