using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRManagementSystem.DAL.Models;




namespace HRManagementSystem.DAL.Interfaces
{
    public interface IEmployeeRepository
    {

        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser> GetByIdAsync(string id);
        Task<bool> UpdateAsync(ApplicationUser user);
        Task<bool> DeleteAsync(ApplicationUser user);
        Task<IEnumerable<ApplicationUser>> GetEmployeesByDepartmentIdAsync(int departmentId);
        Task<IEnumerable<ApplicationUser>> GetEmployeesByDepartmentNameAsync(string departmentName);



        //Dashboard
        Task<int> GetTotalCountAsync();
        Task<List<IGrouping<string, ApplicationUser>>> GroupEmployeesByDepartmentAsync();
        Task<List<IGrouping<string, ApplicationUser>>> GroupEmployeesByGenderAsync();
        Task<decimal> GetAverageSalaryAsync();
        Task<List<IGrouping<string, ApplicationUser>>> GroupEmployeesByAgeGroupAsync();
        Task<List<IGrouping<string, ApplicationUser>>> GroupEmployeesByNationalityAsync();

        //Task<decimal> GetAverageDailyAttendanceAsync();









    }
}
