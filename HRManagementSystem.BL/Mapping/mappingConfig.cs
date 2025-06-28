using AutoMapper;
using HRManagementSystem.BL.DTOs.AuthDTO;
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
        }
    }
}
