using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRManagementSystem.BL.DTOs.DepartmentDTO;
using HRManagementSystem.BL.DTOs.SEttingDTO;

namespace HRManagementSystem.BL.Interfaces
{
    public interface ISettingService
    {
        Task<int> AddSettingAsync(AddSettingDTO Setting);
        Task<EditSettingDTO> GetSettingByIdAsync(int id);
        Task<EditSettingDTO> UpdateSettingAsync(EditSettingDTO Setting);
    }
}
