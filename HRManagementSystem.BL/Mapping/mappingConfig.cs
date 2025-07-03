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

namespace HRManagementSystem.BL.Mapping
{
    public class mappingConfig:Profile
    {
        public mappingConfig()
        {
            CreateMap<RegisterEmployeeDto,ApplicationUser>().AfterMap((src, dest) =>
            {
                dest.UserName = src.Email.Split('@')[0];
            });
            CreateMap<DepartmentDto, Department>();
            CreateMap<Department, DepartmentWithEmployeeCountDto>();
            CreateMap<DepartmentUpdateDto, Department>().ReverseMap();
            
            // Add OfficialHoliday mappings
            CreateMap<OfficialHolidayDto, OfficialHoliday>();
            CreateMap<OfficialHolidayUpdateDto, OfficialHoliday>().ReverseMap();
            
            CreateMap<UserDto, ApplicationUser>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            // Add Attendance mappings
            CreateMap<AttendanceDto, Attendance>();
            CreateMap<AttendanceUpdateDto, Attendance>().ReverseMap();
        }
    }
}
