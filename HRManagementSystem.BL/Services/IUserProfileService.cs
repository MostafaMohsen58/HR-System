using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HRManagementSystem.BL.DTOs.UserProfile;
using HRManagementSystem.DAL.Data.Context;
using HRManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.BL.Services
{
    public interface IUserProfileService
    {
        Task<UserProfileDto> GetUserProfileAsync(string userId);
    }
    public class UserProfileService : IUserProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly HRContext _context;

        public UserProfileService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            HRContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _context = context;
        }

        public async Task<UserProfileDto> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

            var dto = _mapper.Map<UserProfileDto>(user);

            // جلب Role (واحدة فقط)
            var roles = await _userManager.GetRolesAsync(user);
            dto.Role = roles.FirstOrDefault();

            // جلب Permissions من الـ Claims (لو كانت كذلك)
            var claims = await _userManager.GetClaimsAsync(user);
            dto.Permissions = claims
                .Where(c => c.Type == "Permission")
                .Select(c => c.Value)
                .ToList();

            // ملخص الغياب والخصومات الشهرية من جدول PayRoll
            var payrolls = await _context.PayRoll
                .Where(p => p.EmployeeId == userId)
                .ToListAsync();

            dto.MonthlySummaries = payrolls.Select(p => new MonthlyAttendanceSummaryDto
            {
                Month = p.Month,
                Year = p.Year,
                AbsentDays = p.AbsentDays,
                DeductionAmount = CalculateDeduction(user.Salary, p.AbsentDays)
            }).ToList();

            return dto;
        }

        private decimal CalculateDeduction(decimal salary, int absentDays)
        {
            // نفترض المرتب الشهري 30 يوم
            decimal dailyRate = salary / 30;
            return dailyRate * absentDays;
        }
    }

}
