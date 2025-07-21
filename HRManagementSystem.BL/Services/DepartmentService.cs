using AutoMapper;
using HRManagementSystem.BL.DTOs.DepartmentDTO;
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
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> AddDepartmentAsync(DepartmentDto departmentDto)
        {
            var department = _mapper.Map<Department>(departmentDto);
            
            var result = await _unitOfWork.DepartmentRepository.AddAsync(department);
            await _unitOfWork.Save();
            return result;
        }

        public async Task<int> DeleteDepartmentAsync(int id)
        {
            var isDeleted = await _unitOfWork.DepartmentRepository.DeleteAsync(id);
            await _unitOfWork.Save();
            if (isDeleted == 0)
                throw new KeyNotFoundException($"Department with Id {id} not found.");

            return isDeleted;
        }

        public async Task<IEnumerable<DepartmentWithEmployeeCountDto>> GetAllDepartmentsAsync()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            
            return departments.Select(dept => new DepartmentWithEmployeeCountDto
            {
                Id = dept.Id,
                Name = dept.Name,
                EmployeeCount = dept.Employees.Count()
            }).ToList();
        }

        public async Task<DepartmentUpdateDto> GetDepartmentByIdAsync(int id)
        {
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            
            if (department == null)
                throw new KeyNotFoundException($"Department with Id {id} not found.");
                
            return _mapper.Map<DepartmentUpdateDto>(department);
        }

        public async Task<DepartmentUpdateDto> UpdateDepartmentAsync(DepartmentUpdateDto departmentDto)
        {
            var department = _mapper.Map<Department>(departmentDto);

            var updatedDepartment = await _unitOfWork.DepartmentRepository.UpdateAsync(department);

            if (updatedDepartment == null)
                throw new KeyNotFoundException($"Department with ID {departmentDto.Id} not found.");

            await _unitOfWork.Save();

            return _mapper.Map<DepartmentUpdateDto>(updatedDepartment);
        }
    }
}
