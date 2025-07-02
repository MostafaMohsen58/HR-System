using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRManagementSystem.DAL.Data.Context;
using HRManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.DAL.Repositories
{
    public class SettingRepository
    {
        private readonly HRContext _context;

        public SettingRepository(HRContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Setting setting)
        {
            await _context.Setting.AddAsync(setting);
            await _context.SaveChangesAsync();
            return setting.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var setting = await _context.Setting.FindAsync(id);
            if (setting == null)
                return 0;

            _context.Setting.Remove(setting);
            await _context.SaveChangesAsync();
            return setting.Id;
        }

        public async Task<IEnumerable<Setting>> GetAllAsync()
        {
            return await _context.Setting.ToListAsync();
        }

        public async Task<Setting> GetByIdAsync(int id)
        {
            return await _context.Setting.FindAsync(id);
        }

        public async Task<Setting> UpdateAsync(Setting setting)
        {
            var existingsetting = await _context.Setting.FindAsync(setting.Id);
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
