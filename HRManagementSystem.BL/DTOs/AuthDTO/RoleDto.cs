﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.DTOs.AuthDTO
{
    public class RoleDto
    {
        public string RoleName { get; set; }

        public List<PermissionDto> Permissions { get; set; }
    }
}
