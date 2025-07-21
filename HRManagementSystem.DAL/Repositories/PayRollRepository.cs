using HRManagementSystem.DAL.Data.Context;
using HRManagementSystem.DAL.Interfaces;
using HRManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.DAL.Repositories
{
    public class PayRollRepository : IPayRollRepository
    {
        private readonly HRContext _context;
        public PayRollRepository(HRContext context)
        {
            _context = context;
        }
        public async Task<int> AddAsync(PayRoll payroll)
        {
            await _context.PayRoll.AddAsync(payroll);
            //return await _context.SaveChangesAsync().ContinueWith(t => payroll.Id);

            return payroll.Id; 
        }

        public async Task<PayRoll?> GetByMonthAndYearAsync(int month, int year, string employeeId)
        {
            return await _context.PayRoll
                .FirstOrDefaultAsync(p => p.Month == month && p.Year == year && p.EmployeeId == employeeId);
        }

        public async Task<PayRoll?> UpdateAsync(PayRoll payroll)
        {
            var existingPayroll = await _context.PayRoll.FindAsync(payroll.Id);

            if (existingPayroll == null)
                return null;

            existingPayroll.PresentDays = payroll.PresentDays;
            existingPayroll.AbsentDays = payroll.AbsentDays;
            existingPayroll.DeductionInHours = payroll.DeductionInHours;
            existingPayroll.ExtraHours = payroll.ExtraHours;
            existingPayroll.NetSalary = payroll.NetSalary;
            existingPayroll.TotalAddition = payroll.TotalAddition;
            existingPayroll.TotalDeduction = payroll.TotalDeduction;

            return existingPayroll;
        }
        public async Task<List<PayRoll>> GetAllAsync()
        {
            return await _context.PayRoll.ToListAsync();
        }

        public IQueryable<PayRoll> GetAllQueryable()
        {
            return _context.PayRoll
                .Include(p => p.Employee)
                .ThenInclude(e => e.Department)
                .AsQueryable();
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _context.PayRoll.Where(a => a.Id == id).ExecuteDeleteAsync();
        }
    }
}
