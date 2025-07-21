using AutoMapper;
using HRManagementSystem.BL.DTOs.Setting;
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
    public class SettingService : ISettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SettingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> AddSettingAsync(AddSettingDto Setting)
        {
            var setting = _mapper.Map<Setting>(Setting);
            var result= await _unitOfWork.SettingRepository.AddAsync(setting);
            await _unitOfWork.Save();
            return result;
        }

        public async Task<EditSettingDto> GetSettingAsync()
        {
            var setting = await _unitOfWork.SettingRepository.Get();

            if (setting == null)
                throw new KeyNotFoundException($"setting not found.");

            return _mapper.Map<EditSettingDto>(setting);
        }
        public async Task<EditSettingDto> UpdateSettingAsync(EditSettingDto Setting)
        {
            var setting = _mapper.Map<Setting>(Setting);

            var updatedSetting = await _unitOfWork.SettingRepository.UpdateAsync(setting);

            if (updatedSetting == null)
                throw new KeyNotFoundException($" ID  not found.");
            await _unitOfWork.Save();

            return _mapper.Map<EditSettingDto>(updatedSetting);
        }
    }
}
