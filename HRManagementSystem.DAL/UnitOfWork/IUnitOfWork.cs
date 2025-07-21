using HRManagementSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IAttendanceRepository AttendanceRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IOfficialHolidayRepository officialHolidayRepository { get; }
        IPayRollRepository payRollRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        ISettingRepository SettingRepository { get; }
        Task Save();

    }
}
