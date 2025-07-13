using AutoMapper;
using HRManagementSystem.BL.DTOs.EmployeeDTO;
using HRManagementSystem.BL.Interfaces;
using HRManagementSystem.DAL.Interfaces;

namespace HRManagementSystem.BL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ViewEmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ViewEmployeeDto>>(employees);
        }

        public async Task<ViewEmployeeDto> GetEmployeeByIdAsync(string id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return employee != null ? _mapper.Map<ViewEmployeeDto>(employee) : null;
        }

        public async Task<bool> UpdateEmployeeAsync(UpdateEmployeeDto dto)
        {
            var employee = await _employeeRepository.GetByIdAsync(dto.Id);
            if (employee == null)
                return false;

            _mapper.Map(dto, employee);

            if (dto.DepartmentId.HasValue)
            {
                var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId.Value);
                if (department == null)
                    return false;

                employee.Department = department;
                employee.DepartmentId = department.Id;
            }

            return await _employeeRepository.UpdateAsync(employee);
        }

        public async Task<bool> DeleteEmployeeAsync(string id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return false;

            return await _employeeRepository.DeleteAsync(employee);
        }

        public async Task<IEnumerable<ViewEmployeeDto>> GetEmployeesByDepartmentIdAsync(int departmentId)
        {
            var employees = await _employeeRepository.GetEmployeesByDepartmentIdAsync(departmentId);
            return _mapper.Map<IEnumerable<ViewEmployeeDto>>(employees);
        }

        public async Task<IEnumerable<ViewEmployeeDto>> GetEmployeesByDepartmentNameAsync(string departmentName)
        {
            var employees = await _employeeRepository.GetEmployeesByDepartmentNameAsync(departmentName);
            return _mapper.Map<IEnumerable<ViewEmployeeDto>>(employees);
        }
    }
}
