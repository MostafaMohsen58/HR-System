using AutoMapper;
using HRManagementSystem.BL.DTOs.AttendanceDTOs;
using HRManagementSystem.BL.DTOs.AuthDTO;
using HRManagementSystem.BL.DTOs.DepartmentDTO;
using HRManagementSystem.BL.DTOs.OfficialHoliday;
using HRManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HRManagementSystem.BL.DTOs.AuthDTO;
using HRManagementSystem.BL.DTOs.DepartmentDTO;
using HRManagementSystem.BL.DTOs.EmployeeDTO;
using HRManagementSystem.BL.DTOs.OfficialHoliday;
using HRManagementSystem.DAL.Models;
using HRManagementSystem.DAL.Models.Enums;
using Microsoft.Data.SqlClient;

namespace HRManagementSystem.BL.Mapping
{
    public class mappingConfig:Profile
    {
        public mappingConfig()
        {
            CreateMap<RegisterEmployeeDto,ApplicationUser>().AfterMap((src, dest) =>
            {
                dest.UserName = src.Email.Split('@')[0];
                dest.DepartmentId = src.DepartmentId;
                //dest.PhoneNumber = src.PhoneNumber;
                dest.DateOfBirth = src.DateOfBirth;
            });
            CreateMap<DepartmentDto, Department>();
            CreateMap<Department, DepartmentWithEmployeeCountDto>();
            CreateMap<DepartmentUpdateDto, Department>().ReverseMap();

            // View mapping
            CreateMap<ApplicationUser, ViewEmployeeDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.BaseSalary, opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));


            // Update mapping
            //CreateMap<UpdateEmployeeDto, ApplicationUser>().ReverseMap();
           CreateMap<UpdateEmployeeDto, ApplicationUser>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.BaseSalary))
                .AfterMap((src, dest) =>
                  {
                      if (!string.IsNullOrEmpty(src.Password))
                      {
                          var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<ApplicationUser>();
                          dest.PasswordHash = hasher.HashPassword(dest, src.Password);
                      }
                  }).ReverseMap();


            // Add OfficialHoliday mappings
            CreateMap<OfficialHolidayDto, OfficialHoliday>();
            CreateMap<OfficialHolidayUpdateDto, OfficialHoliday>().ReverseMap();
            
            CreateMap<UserDto, ApplicationUser>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            // Add Attendance mappings
            CreateMap<AttendanceDto, Attendance>();
            CreateMap<AttendanceUpdateDto, Attendance>();
            CreateMap<Attendance, AttendanceUpdateDto>()
                .ForMember(dest => dest.EmployeeName,
                            opt => opt.MapFrom(src => src.Employee.FullName))
                .ForMember(dest => dest.DepartmentName,
                            opt => opt.MapFrom(src => src.Employee.Department.Name))
                .ForMember(dest => dest.departmentId,
                            opt => opt.MapFrom(src => src.Employee.Department.Id));
        }
    }
}
