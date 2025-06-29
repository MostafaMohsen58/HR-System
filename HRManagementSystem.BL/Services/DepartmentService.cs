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
            // Map DTO to entity
            var department = _mapper.Map<Department>(departmentDto);
            
            // Call repository
            return await _departmentRepository.AddAsync(department);
        }

        public async Task<int> DeleteDepartmentAsync(int id)
        {
            // Get the department first to check if it exists
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
                throw new KeyNotFoundException($"Department with Id {id} not found.");
                
            // Delete if exists
            return await _departmentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<DepartmentWithEmployeeCountDto>> GetAllDepartmentsAsync()
        {
            // Get all departments from repository
            var departments = await _departmentRepository.GetAllAsync();
            
            // Transform to DTOs with business logic (employee count calculation)
            var departmentDtos = departments.Select(dept => new DepartmentWithEmployeeCountDto
            {
                Id = dept.Id,
                Name = dept.Name,
                EmployeeCount = dept.Employees.Count()
            }).ToList();
            
            return departmentDtos;
        }

        public async Task<DepartmentUpdateDto> GetDepartmentByIdAsync(int id)
        {
            // Get department from repository
            var department = await _departmentRepository.GetByIdAsync(id);
            
            // Check if department exists
            if (department == null)
                throw new KeyNotFoundException($"Department with Id {id} not found.");
                
            // Map to DTO and return
            return _mapper.Map<DepartmentUpdateDto>(department);
        }

        public async Task<DepartmentUpdateDto> UpdateDepartmentAsync(DepartmentUpdateDto departmentDto)
        {
            // Map DTO to entity
            var department = _mapper.Map<Department>(departmentDto);

            // Update entity
            var updatedDepartment = await _departmentRepository.UpdateAsync(department);

            if (updatedDepartment == null)
                throw new KeyNotFoundException($"Department with ID {departmentDto.Id} not found.");

            // Map back to DTO and return
            return _mapper.Map<DepartmentUpdateDto>(updatedDepartment);
        }
    }
}
