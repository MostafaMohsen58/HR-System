using AutoMapper;
using HRManagementSystem.BL.DTOs.AttendanceDTOs;
using HRManagementSystem.BL.DTOs.PayRoll;
using HRManagementSystem.BL.Interfaces;
using HRManagementSystem.BL.Utilities;
using HRManagementSystem.DAL.Interfaces;
using HRManagementSystem.DAL.Models;
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
        private readonly IMapper _mapper;
        public PayRollService(IPayRollRepository payRollRepository, IEmployeeRepository employeeRepository
            , IOfficialHolidayRepository officialHolidayRepository
            , IMapper mapper)
        {
            _payRollRepository = payRollRepository;
            _employeeRepository = employeeRepository;
            _officialHolidayRepository = officialHolidayRepository;
            _mapper = mapper;
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
                var payRoll = CalculateNetSalary(employee, month, year, checkIn, checkOut,21,1);
                
                return await _payRollRepository.AddAsync(payRoll);
            }
            else 
            {
                //update the existing payroll entry
                var payRoll = CalculateNetSalary(employee, month,year,checkIn,checkOut, --existingPayRoll.AbsentDays, ++existingPayRoll.PresentDays);

                existingPayRoll.AbsentDays = payRoll.AbsentDays; // should delete it not need for these 2 lines because it's already updated
                existingPayRoll.PresentDays = payRoll.PresentDays;
                existingPayRoll.NetSalary += payRoll.NetSalary;
                existingPayRoll.TotalAddition += payRoll.TotalAddition;
                existingPayRoll.TotalDeduction += payRoll.TotalDeduction;
                existingPayRoll.DeductionInHours += payRoll.DeductionInHours;
                existingPayRoll.ExtraHours += payRoll.ExtraHours;

                return await _payRollRepository.UpdateAsync(existingPayRoll).ContinueWith(t => t.Result?.Id ?? 0);
            }

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

            var payRoll = CalculateNetSalary(employee, month, year, checkIn, checkOut, ++existingPayRoll.AbsentDays, --existingPayRoll.PresentDays);

            existingPayRoll.AbsentDays = payRoll.AbsentDays; 
            existingPayRoll.PresentDays = payRoll.PresentDays;
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


        private PayRoll CalculateNetSalary(ApplicationUser employee,int month,int year,DateTime checkIn,DateTime checkOut, int absentDays, int presentDays)
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
                var workedMinutes = endTime.Subtract(checkIn.TimeOfDay).TotalMinutes;
                var nativeSalary = (decimal)workedMinutes * valueOfMinute; // salary of employee for the worked minutes without any deductions or additions

                var deductedMinutes = (checkIn.TimeOfDay > startTime) ? (checkIn.TimeOfDay - startTime).TotalMinutes : 0;
                var addedMinutes = (checkOut.TimeOfDay > endTime) ? (checkOut.TimeOfDay - endTime).TotalMinutes : 0;

                //get type(Hour - Pound) from settings
                //call repo to get the type and then calculate salary based on it deduction and addition
                var type = "Hour"; // this should be fetched from settings or configuration
                decimal additionalSalary, deductedSalary, total;
                if (type == "Hour")
                {
                    additionalSalary = (decimal)addedMinutes * (valueOfMinute * 2); // i make additional hour equal 2 hours
                    deductedSalary = (decimal)deductedMinutes * (valueOfMinute * (decimal)0.5); // i make deducted hour equal 2 hours
                    total = nativeSalary + additionalSalary - deductedSalary; // total salary after addition and deduction
                }
                else
                {
                    additionalSalary = (decimal)addedMinutes * (1000 / 60); // get the value pound from settings i assume 1 hour = 1000 pounds
                    deductedSalary = (decimal)addedMinutes * (500 / 60); // get the value pound from settings i assume 1 hour = 500 pounds
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
                    DeductionInHours = (int)(deductedMinutes / 60), // assuming deduction is in hours
                    ExtraHours = (int)(addedMinutes / 60) // assuming extra hours is in hours
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
                var holidayPay = CalculateNetSalary(employee, month, year, DateTime.Today, DateTime.Today, monthHolidays.Count, monthHolidays.Count);

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
