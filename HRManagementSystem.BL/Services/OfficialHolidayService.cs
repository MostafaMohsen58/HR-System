using AutoMapper;
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
        public OfficialHolidayService(IOfficialHolidayRepository officialHolidayService,IMapper mapper)
        {
            _officialHolidayRepository = officialHolidayService;
            _mapper = mapper;
        }
        public async Task<int> AddOfficialHolidayAsync(OfficialHoliday officialHoliday)
        {
            //var officialHolidayEntity = _mapper.Map<OfficialHoliday>(officialHoliday);
            return await _officialHolidayRepository.AddAsync(officialHoliday);
        }

        public async Task<int> DeleteOfficialHolidayAsync(int id)
        {
            try
            {
                return await _officialHolidayRepository.DeleteAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                return await Task.FromException<int>(ex);
            }
        }

        public async Task<IEnumerable<OfficialHoliday>> GetAllOfficialHolidaysAsync()
        {
            return await _officialHolidayRepository.GetAllAsync();
        }

        public async Task<OfficialHoliday> GetOfficialHolidayByIdAsync(int id)
        {
            try
            {
                return await _officialHolidayRepository.GetByIdAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                return await Task.FromException<OfficialHoliday>(ex);
            }
        }

        public async Task<OfficialHoliday> UpdateOfficialHolidayAsync(OfficialHoliday officialHoliday)
        {
            try
            {
                return await _officialHolidayRepository.UpdateAsync(officialHoliday);
            }
            catch (KeyNotFoundException ex)
            {
                return await Task.FromException<OfficialHoliday>(ex);
            }
        }
    }
}
