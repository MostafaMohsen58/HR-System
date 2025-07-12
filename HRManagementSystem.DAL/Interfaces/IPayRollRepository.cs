using HRManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HRManagementSystem.DAL.Interfaces
{
    public interface IPayRollRepository
    {
        Task<int> AddAsync(PayRoll payroll);
        Task<PayRoll?> UpdateAsync(PayRoll payroll);
        Task<PayRoll?> GetByMonthAndYearAsync(int month, int year, string employeeId);
        Task<List<PayRoll>> GetAllAsync();
        IQueryable<PayRoll> GetAllQueryable();

    }
}
