using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRManagementSystem.DAL.Data.Context;
using HRManagementSystem.DAL.Interfaces;
using HRManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.DAL.Repositories
{
    public class SettingRepository:ISettingRepository
    {
        private readonly HRContext _context;

        public SettingRepository(HRContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Setting setting)
        {
            var s =await _context.Setting.CountAsync();
            if (s > 0)  return 0; 
            await _context.Setting.AddAsync(setting);
            await _context.SaveChangesAsync();
            return setting.Id;
        }

        public async Task<Setting> GetByIdAsync(int id=1)
        {
            return await _context.Setting.FindAsync(id);
        }

        public async Task<Setting> UpdateAsync(Setting setting)
        {
            var existingsetting = await _context.Setting.Where(x=>x.Id==1).FirstOrDefaultAsync();
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
