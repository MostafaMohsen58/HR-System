using HRManagementSystem.BL.DTOs.EmployeeDTO;

namespace HRManagementSystem.BL.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<ViewEmployeeDto>> GetAllEmployeesAsync();
        Task<ViewEmployeeDto> GetEmployeeByIdAsync(string id);
        Task<bool> UpdateEmployeeAsync(UpdateEmployeeDto dto);
        Task<bool> DeleteEmployeeAsync(string id);
        Task<IEnumerable<ViewEmployeeDto>> GetEmployeesByDepartmentIdAsync(int departmentId);
        Task<IEnumerable<ViewEmployeeDto>> GetEmployeesByDepartmentNameAsync(string departmentName);


        Task<int> GetTotalEmployeesCountAsync();

        Task<List<EmployeesByDepartmentDto>> GetEmployeesByDepartmentAsync();

        Task<List<GenderDistributionDto>> GetGenderDistributionAsync();

        Task<decimal> GetAverageSalaryAsync();

        Task<List<AgeGroupDto>> GetEmployeesGroupedByAgeAsync();

        Task<IEnumerable<NationalityDistributionDto>> GetNationalityDistributionAsync();

        //Task<decimal> GetAverageDailyAttendanceAsync();
        


    }
}

