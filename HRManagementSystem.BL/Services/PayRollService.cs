using AutoMapper;
using HRManagementSystem.BL.DTOs.AttendanceDTOs;
using HRManagementSystem.BL.DTOs.PayRoll;
using HRManagementSystem.BL.Interfaces;
using HRManagementSystem.BL.Utilities;
using HRManagementSystem.DAL.Interfaces;
using HRManagementSystem.DAL.Models;
using HRManagementSystem.DAL.Models.Enums;
using HRManagementSystem.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HRManagementSystem.BL.Services
{
    public class PayRollService : IPayRollService
    {
        private readonly IPayRollRepository _payRollRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IOfficialHolidayRepository _officialHolidayRepository;
        private readonly ISettingRepository _settingRepository;
        private readonly IMapper _mapper;
        public PayRollService(IPayRollRepository payRollRepository, IEmployeeRepository employeeRepository
            , IOfficialHolidayRepository officialHolidayRepository
            , IMapper mapper
            , ISettingRepository settingRepository)
        {
            _payRollRepository = payRollRepository;
            _employeeRepository = employeeRepository;
            _officialHolidayRepository = officialHolidayRepository;
            _mapper = mapper;
            _settingRepository = settingRepository;
        }
        public async Task<int> AddPayRollAsync(int month, int year, string employeeId, DateTime checkIn, DateTime checkOut)
        {
            var existingPayRoll = await _payRollRepository.GetByMonthAndYearAsync(month, year, employeeId);

            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");
            }

            if (existingPayRoll == null)
            {
                //create a new payroll entry
                var payRoll =await CalculateNetSalary(employee, month, year, checkIn, checkOut,21,1);
                
                return await _payRollRepository.AddAsync(payRoll);
            }
            else 
            {
                if (existingPayRoll.AbsentDays == 0)
                    existingPayRoll.AbsentDays = 0;
                else
                    --existingPayRoll.AbsentDays;

                ++existingPayRoll.PresentDays;
                //update the existing payroll entry
                var payRoll =await CalculateNetSalary(employee, month,year,checkIn,checkOut, existingPayRoll.AbsentDays, existingPayRoll.PresentDays);

                
                existingPayRoll.NetSalary += payRoll.NetSalary;
                existingPayRoll.TotalAddition += payRoll.TotalAddition;
                existingPayRoll.TotalDeduction += payRoll.TotalDeduction;
                existingPayRoll.DeductionInHours += payRoll.DeductionInHours;
                existingPayRoll.ExtraHours += payRoll.ExtraHours;

                return await _payRollRepository.UpdateAsync(existingPayRoll).ContinueWith(t => t.Result?.Id ?? 0);
            }

        }

        public async Task<int> UpdatePayRoll(int oldMonth, int oldYear, int month , int year, string employeeId, DateTime oldCheckIn, DateTime oldCheckOut,DateTime checkIn,DateTime checkOut)
        {
            if(oldCheckIn != checkIn || oldCheckOut != checkOut)
            {
                var existingPayRoll = await _payRollRepository.GetByMonthAndYearAsync(oldMonth, oldYear, employeeId);

                if (existingPayRoll == null)
                    throw new KeyNotFoundException($"PayRoll for month {month} and year {year} for employee {employeeId} not found.");

                var employee = await _employeeRepository.GetByIdAsync(employeeId);

                if (employee == null)
                    throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

                if (oldMonth == month && oldYear == year)
                {
                    
                    var oldSalarypayRoll =await CalculateNetSalary(employee, oldMonth, oldYear, oldCheckIn, oldCheckOut, existingPayRoll.AbsentDays, existingPayRoll.PresentDays);
                    var newSalaryPayRoll =await CalculateNetSalary(employee, month, year, checkIn, checkOut, existingPayRoll.AbsentDays, existingPayRoll.PresentDays);

                    
                    existingPayRoll.NetSalary -= oldSalarypayRoll.NetSalary; // remove the old salary
                    existingPayRoll.NetSalary += newSalaryPayRoll.NetSalary; // add the new salary

                    existingPayRoll.TotalAddition -= oldSalarypayRoll.TotalAddition;
                    existingPayRoll.TotalAddition += newSalaryPayRoll.TotalAddition;

                    existingPayRoll.TotalDeduction -= oldSalarypayRoll.TotalDeduction;
                    existingPayRoll.TotalDeduction += newSalaryPayRoll.TotalDeduction;

                    existingPayRoll.DeductionInHours -= oldSalarypayRoll.DeductionInHours;
                    existingPayRoll.DeductionInHours += newSalaryPayRoll.DeductionInHours;

                    existingPayRoll.ExtraHours -= oldSalarypayRoll.ExtraHours;
                    existingPayRoll.ExtraHours += newSalaryPayRoll.ExtraHours;

                    return await _payRollRepository.UpdateAsync(existingPayRoll).ContinueWith(t => t.Result?.Id ?? 0);
                }
                else //case of update record in previous month
                {
                    var oldSalarypayRoll =await CalculateNetSalary(employee, oldMonth, oldYear, oldCheckIn, oldCheckOut, existingPayRoll.AbsentDays, existingPayRoll.PresentDays);
                    
                    existingPayRoll.AbsentDays++;
                    existingPayRoll.PresentDays--;
                    existingPayRoll.NetSalary -= oldSalarypayRoll.NetSalary; 
                    existingPayRoll.TotalAddition -= oldSalarypayRoll.TotalAddition;
                    existingPayRoll.TotalDeduction -= oldSalarypayRoll.TotalDeduction;
                    existingPayRoll.DeductionInHours -= oldSalarypayRoll.DeductionInHours;
                    existingPayRoll.ExtraHours -= oldSalarypayRoll.ExtraHours;

                   await _payRollRepository.UpdateAsync(existingPayRoll).ContinueWith(t => t.Result?.Id ?? 0);


                    //create a new payroll entry for the new month
                    await AddPayRollAsync(month, year, employeeId, checkIn, checkOut).ContinueWith(t => t.Result);
                }
            }
            return 0;
        }

        public async Task<List<PayRoll>> GetAllPayRollsAsync()
        {
            return await _payRollRepository.GetAllAsync();
        }
        public async Task<int> DeletePayRollAsync(int month, int year, string employeeId, DateTime checkIn, DateTime checkOut)
        {
            var existingPayRoll = await _payRollRepository.GetByMonthAndYearAsync(month, year, employeeId);
            if (existingPayRoll == null)
            {
                throw new KeyNotFoundException($"PayRoll for month {month} and year {year} for employee {employeeId} not found.");
            }

            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");
            }

            ++existingPayRoll.AbsentDays;
            --existingPayRoll.PresentDays;

            if (existingPayRoll.AbsentDays == 22)
            {
                await _payRollRepository.DeleteAsync(existingPayRoll.Id);
                return existingPayRoll.Id;
            }

            var payRoll =await CalculateNetSalary(employee, month, year, checkIn, checkOut, existingPayRoll.AbsentDays, existingPayRoll.PresentDays);

            
            existingPayRoll.NetSalary -= payRoll.NetSalary;
            existingPayRoll.TotalAddition -= payRoll.TotalAddition;
            existingPayRoll.TotalDeduction -= payRoll.TotalDeduction;
            existingPayRoll.DeductionInHours -= payRoll.DeductionInHours;
            existingPayRoll.ExtraHours -= payRoll.ExtraHours;

            return await _payRollRepository.UpdateAsync(existingPayRoll).ContinueWith(t => t.Result?.Id ?? 0);
        }


        public async Task<PaginatedList<PayRollDto>> GetPaginatedAttendancesAsync(
            int pageIndex = 1,
            int pageSize = 5,
            string? searchTerm = null,
            int? month =null,
            int? year = null)
        {
            try
            {
                var payRollQuery = _payRollRepository.GetAllQueryable();
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    payRollQuery = payRollQuery
                    .Where(a =>
                        a.Employee.FullName.Contains(searchTerm)
                    );
                }

                if(month.HasValue)
                {
                    payRollQuery = payRollQuery.Where(p => p.Month == month.Value);
                }

                if(year.HasValue)
                {
                    payRollQuery = payRollQuery.Where(p => p.Year == year.Value);
                }

                var paginatedPayRolls = await PaginatedList<PayRoll>.CreateAsync(payRollQuery, pageIndex, pageSize);

                var mappedItems = _mapper.Map<List<PayRollDto>>(paginatedPayRolls.Items);

                return new PaginatedList<PayRollDto>(
                    mappedItems,
                    paginatedPayRolls.TotalItems,
                    paginatedPayRolls.PageIndex,
                    paginatedPayRolls.PageSize
                );
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not retrieve paginated payRoll records.", ex);
            }
        }


        private async Task<PayRoll> CalculateNetSalary(ApplicationUser employee,int month,int year,DateTime checkIn,DateTime checkOut, int absentDays, int presentDays)
        {

            var startTime = employee.StartTime.TimeOfDay;
            var endTime = employee.EndTime.TimeOfDay;
            //var totalHoursWorked = (checkOut - checkIn).TotalHours;


            var valueOfMinute = (employee.Salary) / ((decimal)(endTime - startTime).TotalMinutes * 22);

            //case of calculate holiday salary
            if (presentDays == absentDays)
            {
                var salaryForHolidays = (decimal)endTime.Subtract(startTime).TotalMinutes * valueOfMinute * presentDays;
                return new PayRoll
                {
                    Month = month,
                    Year = year,
                    EmployeeId = employee.Id,
                    BasicSalary = employee.Salary,
                    NetSalary = salaryForHolidays,
                    TotalAddition = 0,
                    TotalDeduction = 0,
                    DeductionInHours = 0, 
                    ExtraHours = 0 
                };
            }
            else
            {
                var workedMinutes = endTime.Subtract(startTime).TotalMinutes;
                var nativeSalary = (decimal)workedMinutes * valueOfMinute; // salary of employee for the worked minutes without any deductions or additions

                var deductedMinutes = (checkIn.TimeOfDay > startTime) ? (checkIn.TimeOfDay - startTime).TotalMinutes : 0;
                var addedMinutes = (checkOut.TimeOfDay > endTime) ? (checkOut.TimeOfDay - endTime).TotalMinutes : 0;

                //get type(Hour - Pound) from settings
                var type = await _settingRepository.Get();

                decimal additionalSalary, deductedSalary, total;
                if (type.Type == SettingType.Hour)
                {
                    additionalSalary = (decimal)addedMinutes * (valueOfMinute * type.OverTime); 
                    deductedSalary = (decimal)deductedMinutes * (valueOfMinute * type.Deduction); 
                    total = nativeSalary + additionalSalary - deductedSalary; 
                }
                else
                {
                    additionalSalary = (decimal)addedMinutes * (type.OverTime / 60);
                    deductedSalary = (decimal)deductedMinutes * (type.Deduction / 60); 
                    total = nativeSalary + additionalSalary - deductedSalary; // total salary after addition and deduction
                }

                return new PayRoll
                {
                    Month = month,
                    Year = year,
                    AbsentDays = absentDays,    //can be deleted
                    PresentDays = presentDays, //can be deleted
                    EmployeeId = employee.Id,
                    BasicSalary = employee.Salary,
                    NetSalary = total,
                    TotalAddition = additionalSalary,
                    TotalDeduction = deductedSalary,
                    DeductionInHours = (deductedMinutes / 60),
                    ExtraHours = (addedMinutes / 60)
                };

            }
            
        }

        public async Task FinalizeHolidaySalariesAsync(int month, int year)
        {
            if (DateTime.Today.Day != DateTime.DaysInMonth(year, month))
            {
                return;
            }

            var employees = await _employeeRepository.GetAllAsync();
            var allHolidays = await _officialHolidayRepository.GetAllAsync();
            var monthHolidays = allHolidays.Where(h => h.Date.Month == month && h.Date.Year == year).ToList();

            if (!monthHolidays.Any())
                return;

            foreach (var employee in employees)
            {
                var payRoll = await _payRollRepository.GetByMonthAndYearAsync(month, year, employee.Id);
                if (payRoll == null)
                    continue;

                if (payRoll.IsHolidaySalaryCalculated)
                    continue;

                // Calculate only holidays
                var holidayPay =await CalculateNetSalary(employee, month, year, DateTime.Today, DateTime.Today, monthHolidays.Count, monthHolidays.Count);

                payRoll.NetSalary += holidayPay.NetSalary;
                payRoll.AbsentDays -= monthHolidays.Count;
                payRoll.PresentDays += monthHolidays.Count;
                payRoll.IsHolidaySalaryCalculated = true;

                await _payRollRepository.UpdateAsync(payRoll);
                Console.WriteLine("service done successfully for employee: " + employee.Id);
            }
        }
    }
}
