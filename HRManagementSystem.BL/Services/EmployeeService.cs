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
        private readonly IUserService _userService;
        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper,
            IUserService userService)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<IEnumerable<ViewEmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var filteredEmployee = await _userService.GetAllOnlyUsersAsync(employees);
            return filteredEmployee;
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
        public async Task<int> GetTotalEmployeesCountAsync()
        {
            return await _employeeRepository.GetTotalCountAsync();
        }

        public async Task<List<EmployeesByDepartmentDto>> GetEmployeesByDepartmentAsync()
        {
            var groups = await _employeeRepository.GroupEmployeesByDepartmentAsync();

            var result = groups
                .Select(g => new EmployeesByDepartmentDto
                {
                    DepartmentName = g.Key,
                    EmployeeCount = g.Count()
                })
                .ToList();

            return result;
        }

        public async Task<List<GenderDistributionDto>> GetGenderDistributionAsync()
        {
            var groups = await _employeeRepository.GroupEmployeesByGenderAsync();

            var result = groups
                .Select(g => new GenderDistributionDto
                {
                    Gender = g.Key,
                    Count = g.Count()
                })
                .ToList();

            return result;
        }

        public async Task<decimal> GetAverageSalaryAsync()
        {
            return await _employeeRepository.GetAverageSalaryAsync();
        }

        public async Task<List<AgeGroupDto>> GetEmployeesGroupedByAgeAsync()
        {
            var groups = await _employeeRepository.GroupEmployeesByAgeGroupAsync();

            var result = groups
                .Select(g => new AgeGroupDto
                {
                    AgeGroup = g.Key,
                    Count = g.Count()
                })
                .ToList();

            return result;
        }

        public async Task<IEnumerable<NationalityDistributionDto>> GetNationalityDistributionAsync()
        {
            var groups = await _employeeRepository.GroupEmployeesByNationalityAsync();

            return groups
                .Select(g => new NationalityDistributionDto
                {
                    Nationality = g.Key,
                    Count = g.Count()
                })
                .ToList();
        }
        //public async Task<decimal> GetAverageDailyAttendanceAsync()
        //{
        //    return await _employeeRepository.GetAverageDailyAttendanceAsync();
        //}




    }
}
