using AutoMapper;
using HRManagementSystem.BL.DTOs.DepartmentDTO;
using HRManagementSystem.BL.DTOs.SEttingDTO;
using HRManagementSystem.BL.Interfaces;
using HRManagementSystem.DAL.Interfaces;
using HRManagementSystem.DAL.Models;
using HRManagementSystem.DAL.Repositories;

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

        public async Task<int> AddSettingAsync(AddSettingDTO Setting)
        {
            var setting = _mapper.Map<Setting>(Setting);
            return await _settingRepository.AddAsync(setting);
        }

        public async Task<EditSettingDTO> GetSettingByIdAsync(int id)
        {
            var setting = await _settingRepository.GetByIdAsync(id);

            if (setting == null)
                throw new KeyNotFoundException($"setting with Id {id} not found.");

            return _mapper.Map<EditSettingDTO>(setting);
        }
        public async Task<EditSettingDTO> UpdateSettingAsync(EditSettingDTO Setting)
        {
            var department = _mapper.Map<Setting>(Setting);

            var updatedDepartment = await _settingRepository.UpdateAsync(department);

            if (updatedDepartment == null)
                throw new KeyNotFoundException($" ID  not found.");

            return _mapper.Map<EditSettingDTO>(updatedDepartment);
        }
    }
}
