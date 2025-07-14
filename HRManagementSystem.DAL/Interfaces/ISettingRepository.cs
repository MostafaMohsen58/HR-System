using HRManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.DAL.Interfaces
{
    public interface ISettingRepository
    {
        Task<int> AddAsync(Setting s);
        Task<Setting> Get();
        Task<Setting> UpdateAsync(Setting setting);
    }
}
