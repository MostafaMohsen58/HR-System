using AutoMapper;
using HRManagementSystem.BL.DTOs.EmployeeDTO;
using HRManagementSystem.BL.Interfaces;
using HRManagementSystem.DAL.Interfaces;
using HRManagementSystem.DAL.UnitOfWork;

namespace HRManagementSystem.BL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public EmployeeService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<IEnumerable<ViewEmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            var filteredEmployee = await _userService.GetAllOnlyUsersAsync(employees);
            return filteredEmployee;
        }

        public async Task<ViewEmployeeDto> GetEmployeeByIdAsync(string id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            return employee != null ? _mapper.Map<ViewEmployeeDto>(employee) : null;
        }

        public async Task<bool> UpdateEmployeeAsync(UpdateEmployeeDto dto)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(dto.Id);
            if (employee == null)
                return false;

            _mapper.Map(dto, employee);

            if (dto.DepartmentId.HasValue)
            {
                var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(dto.DepartmentId.Value);
                if (department == null)
                    return false;

                employee.Department = department;
                employee.DepartmentId = department.Id;
            }
            _unitOfWork.EmployeeRepository.UpdateAsync(employee);
            await _unitOfWork.Save();
            return true;

        }

        public async Task<bool> DeleteEmployeeAsync(string id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            if (employee == null)
                return false;

            _unitOfWork.EmployeeRepository.DeleteAsync(employee);
            await _unitOfWork.Save();
            return true;
        }

        public async Task<IEnumerable<ViewEmployeeDto>> GetEmployeesByDepartmentIdAsync(int departmentId)
        {
            var employees = await _unitOfWork.EmployeeRepository.GetEmployeesByDepartmentIdAsync(departmentId);
            var filteredEmployee = await _userService.GetAllOnlyUsersAsync(employees);
            return filteredEmployee;
        }

        public async Task<IEnumerable<ViewEmployeeDto>> GetEmployeesByDepartmentNameAsync(string departmentName)
        {
            var employees = await _unitOfWork.EmployeeRepository.GetEmployeesByDepartmentNameAsync(departmentName);
            return _mapper.Map<IEnumerable<ViewEmployeeDto>>(employees);
        }
        public async Task<int> GetTotalEmployeesCountAsync()
        {
            return await _unitOfWork.EmployeeRepository.GetTotalCountAsync();
        }

        public async Task<List<EmployeesByDepartmentDto>> GetEmployeesByDepartmentAsync()
        {
            var groups = await _unitOfWork.EmployeeRepository.GroupEmployeesByDepartmentAsync();

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
            var groups = await _unitOfWork.EmployeeRepository.GroupEmployeesByGenderAsync();

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
            return await _unitOfWork.EmployeeRepository.GetAverageSalaryAsync();
        }

        public async Task<List<AgeGroupDto>> GetEmployeesGroupedByAgeAsync()
        {
            var groups = await _unitOfWork.EmployeeRepository.GroupEmployeesByAgeGroupAsync();

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
            var groups = await _unitOfWork.EmployeeRepository.GroupEmployeesByNationalityAsync();

            return groups
                .Select(g => new NationalityDistributionDto
                {
                    Nationality = g.Key,
                    Count = g.Count()
                })
                .ToList();
        }




    }
}
