using AutoMapper;
using HRManagementSystem.BL.DTOs.OfficialHoliday;
using HRManagementSystem.BL.Interfaces;
using HRManagementSystem.DAL.Interfaces;
using HRManagementSystem.DAL.Models;
using HRManagementSystem.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.Services
{
    public class OfficialHolidayService : IOfficialHolidayService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public OfficialHolidayService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
        }
        
        public async Task<int> AddOfficialHolidayAsync(OfficialHolidayDto officialHolidayDto)
        {
            var officialHoliday = _mapper.Map<OfficialHoliday>(officialHolidayDto);
            
            var result= await _unitOfWork.officialHolidayRepository.AddAsync(officialHoliday);
            await _unitOfWork.Save();
            return result;
        }

        public async Task<OfficialHolidayUpdateDto> GetOfficialHolidayByIdAsync(int id)
        {
            var holiday = await _unitOfWork.officialHolidayRepository.GetByIdAsync(id);

            if (holiday == null)
                throw new KeyNotFoundException($"Official Holiday with Id {id} not found.");

            return _mapper.Map<OfficialHolidayUpdateDto>(holiday);
        }

        public async Task<OfficialHolidayUpdateDto> UpdateOfficialHolidayAsync(OfficialHolidayUpdateDto officialHolidayDto)
        {
            var officialHoliday = _mapper.Map<OfficialHoliday>(officialHolidayDto);

            var updatedHoliday = await _unitOfWork.officialHolidayRepository.UpdateAsync(officialHoliday);

            if (updatedHoliday == null)
                throw new KeyNotFoundException($"Official Holiday with ID {officialHolidayDto.Id} not found.");
            
            await _unitOfWork.Save();
            return _mapper.Map<OfficialHolidayUpdateDto>(updatedHoliday);
        }

        public async Task<int> DeleteOfficialHolidayAsync(int id)
        {
            var idDeleted = await _unitOfWork.officialHolidayRepository.DeleteAsync(id);

            if (idDeleted == 0)
                throw new KeyNotFoundException($"Official Holiday with Id {id} not found.");
            await _unitOfWork.Save();
            return idDeleted;
        }

        public async Task<IEnumerable<OfficialHolidayUpdateDto>> GetAllOfficialHolidaysAsync()
        {
            var holidays = await _unitOfWork.officialHolidayRepository.GetAllAsync();
            
            return _mapper.Map<IEnumerable<OfficialHolidayUpdateDto>>(holidays);
        }

        

        
    }
}
