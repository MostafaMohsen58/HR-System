using HRManagementSystem.BL.DTOs.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.Interfaces
{
    public interface ISettingService
    {
        Task<int> AddSettingAsync(AddSettingDto Setting);
        Task<EditSettingDto> GetSettingAsync();
        Task<EditSettingDto> UpdateSettingAsync(EditSettingDto Setting);
    }
}
