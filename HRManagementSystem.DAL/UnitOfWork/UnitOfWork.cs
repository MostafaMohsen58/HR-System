using HRManagementSystem.DAL.Data.Context;
using HRManagementSystem.DAL.Interfaces;
using HRManagementSystem.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HRContext _context;
        public UnitOfWork(HRContext context)
        {
            _context = context;
        }

        private IAttendanceRepository _attendanceRepository;
        public IAttendanceRepository AttendanceRepository
        {
            get
            {
                if( _attendanceRepository == null )
                    _attendanceRepository=new AttendanceRepository(_context);

                return _attendanceRepository;
            }
        }
        private IDepartmentRepository _DepartmentRepository;
        public IDepartmentRepository DepartmentRepository
        {
            get
            {
                if(_DepartmentRepository == null)
                    _DepartmentRepository=new DepartmentRepository(_context);

                return _DepartmentRepository;
            }
        }
        private IEmployeeRepository _EmployeeRepository;
        private IOfficialHolidayRepository _officialHolidayRepository;
        private IPayRollRepository _payRollRepository;
        private IPermissionRepository _PermissionRepository;
        private ISettingRepository _SettingRepository;
        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                if( _EmployeeRepository == null )
                    _EmployeeRepository=new EmployeeRepository(_context);
                return _EmployeeRepository;
            }
        }

        public IOfficialHolidayRepository officialHolidayRepository
        {
            get
            {
                if( _officialHolidayRepository == null )
                    _officialHolidayRepository=new OfficialHolidayRepository(_context);
                return _officialHolidayRepository;
            }
        }


        public IPayRollRepository payRollRepository
        {
            get
            {
                if(_payRollRepository == null )
                    _payRollRepository=new PayRollRepository(_context);
                return _payRollRepository ;
            }
        }

        public IPermissionRepository PermissionRepository
        {
            get
            {
                if(_PermissionRepository == null )
                    _PermissionRepository=new PermissionRepository(_context);
                return _PermissionRepository;
            }
        }

        public ISettingRepository SettingRepository
        {
            get
            {
                if(_SettingRepository == null )
                    _SettingRepository=new SettingRepository(_context);
                return _SettingRepository;
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
