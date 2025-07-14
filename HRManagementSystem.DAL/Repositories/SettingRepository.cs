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
    public class SettingRepository : ISettingRepository
    {
        private readonly HRContext _context;

        public SettingRepository(HRContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Setting setting)
        {
            var s = await _context.Setting.CountAsync();
            if (s > 0) return 0;
            await _context.Setting.AddAsync(setting);
            await _context.SaveChangesAsync();
            return setting.Id;
        }

        public async Task<Setting> Get()
        {
            return await _context.Setting.SingleOrDefaultAsync();
        }

        public async Task<Setting> UpdateAsync(Setting setting)
        {
            var existingsetting = await _context.Setting.SingleOrDefaultAsync();
            if (existingsetting == null)
                return null;

            existingsetting.Type = setting.Type;
            existingsetting.FirstHoliday = setting.FirstHoliday;
            existingsetting.SecondHoliday = setting.SecondHoliday;
            existingsetting.OverTime = setting.OverTime;
            existingsetting.Deduction = setting.Deduction;


            await _context.SaveChangesAsync();
            return existingsetting;
        }
    }
}
