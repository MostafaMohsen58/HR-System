using AutoMapper;
using HRManagementSystem.BL.DTOs.DepartmentDTO;
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
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<int> AddDepartmentAsync(DepartmentDto departmentDto)
        {
            var department = _mapper.Map<Department>(departmentDto);
            
            return await _departmentRepository.AddAsync(department);
        }

        public async Task<int> DeleteDepartmentAsync(int id)
        {
            var isDeleted = await _departmentRepository.DeleteAsync(id);
            if (isDeleted == 0)
                throw new KeyNotFoundException($"Department with Id {id} not found.");

            return isDeleted;
        }

        public async Task<IEnumerable<DepartmentWithEmployeeCountDto>> GetAllDepartmentsAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();
            
            return departments.Select(dept => new DepartmentWithEmployeeCountDto
            {
                Id = dept.Id,
                Name = dept.Name,
                EmployeeCount = dept.Employees.Count()
            }).ToList();
        }

        public async Task<DepartmentUpdateDto> GetDepartmentByIdAsync(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            
            if (department == null)
                throw new KeyNotFoundException($"Department with Id {id} not found.");
                
            return _mapper.Map<DepartmentUpdateDto>(department);
        }

        public async Task<DepartmentUpdateDto> UpdateDepartmentAsync(DepartmentUpdateDto departmentDto)
        {
            var department = _mapper.Map<Department>(departmentDto);

            var updatedDepartment = await _departmentRepository.UpdateAsync(department);

            if (updatedDepartment == null)
                throw new KeyNotFoundException($"Department with ID {departmentDto.Id} not found.");

            return _mapper.Map<DepartmentUpdateDto>(updatedDepartment);
        }
    }
}
