using AutoMapper;
using HRManagementSystem.BL.DTOs.Setting;
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
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IMapper _mapper;
        public SettingService(ISettingRepository settingService, IMapper mapper)
        {
            _settingRepository = settingService;
            _mapper = mapper;
        }

        public async Task<int> AddSettingAsync(AddSettingDto Setting)
        {
            var setting = _mapper.Map<Setting>(Setting);
            return await _settingRepository.AddAsync(setting);
        }

        public async Task<EditSettingDto> GetSettingAsync()
        {
            var setting = await _settingRepository.Get();

            if (setting == null)
                throw new KeyNotFoundException($"setting not found.");

            return _mapper.Map<EditSettingDto>(setting);
        }
        public async Task<EditSettingDto> UpdateSettingAsync(EditSettingDto Setting)
        {
            var department = _mapper.Map<Setting>(Setting);

            var updatedDepartment = await _settingRepository.UpdateAsync(department);

            if (updatedDepartment == null)
                throw new KeyNotFoundException($" ID  not found.");

            return _mapper.Map<EditSettingDto>(updatedDepartment);
        }
    }
}
