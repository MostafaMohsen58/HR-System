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
    public class OfficialHolidayRepository : IOfficialHolidayRepository
    {
        private readonly HRContext _context;
        
        public OfficialHolidayRepository(HRContext context)
        {
            _context = context;
        }
        
        public async Task<int> AddAsync(OfficialHoliday officialHoliday)
        {
            await _context.OfficialHoliday.AddAsync(officialHoliday);
            await _context.SaveChangesAsync();
            return officialHoliday.Id;
        }

        public async Task<OfficialHoliday> GetByIdAsync(int id)
        {
            return await _context.OfficialHoliday.FindAsync(id);
        }

        public async Task<OfficialHoliday> UpdateAsync(OfficialHoliday officialHoliday)
        {
            var existingHoliday = await _context.OfficialHoliday.FindAsync(officialHoliday.Id);
            if (existingHoliday == null)
                return null;
                
            existingHoliday.Date = officialHoliday.Date;
            existingHoliday.Name = officialHoliday.Name;

            await _context.SaveChangesAsync();
            return existingHoliday;
        }
        
        public async Task<int> DeleteAsync(int id)
        {
            var officialHoliday = await _context.OfficialHoliday.FindAsync(id);
            if (officialHoliday == null)
                return 0; 
                
            _context.OfficialHoliday.Remove(officialHoliday);
            await _context.SaveChangesAsync();
            return officialHoliday.Id;
        }
        
        public async Task<IEnumerable<OfficialHoliday>> GetAllAsync()
        {
            return await _context.OfficialHoliday.ToListAsync();
        }
    }
}
