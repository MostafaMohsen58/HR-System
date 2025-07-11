using AutoMapper;
using HRManagementSystem.BL.DTOs.AuthDTO;
using HRManagementSystem.BL.Helpers;
using HRManagementSystem.BL.Interfaces;
using HRManagementSystem.DAL.Models;
using HRManagementSystem.DAL.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPermissionService _permissionService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<Role> _roleManger;
        private readonly IMapper _mapper;

        public UserService(
      UserManager<ApplicationUser> userManager,
      SignInManager<ApplicationUser> signInManager,
      RoleManager<Role> roleManager,
      IPermissionService permissionService,
      IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManger = roleManager;
            _permissionService = permissionService;
            _mapper = mapper;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterEmployeeDto model, string role)
        {
            //ApplicationUser userInDb = new ApplicationUser()
            //{
            //    Email = model.Email,
            //    UserName = model.Email.Split('@')[0],
            //    Address = model.Address,
            //    FullName = model.FullName,
            //    NationalId = model.NationalId,
            //    Nationality= model.Nationality,
            //    Salary= model.Salary,
            //    Gender= model.Gender,
            //    DateOfBirth = model.DateOfBirth,
            //    ContractDate = model.ContractDate,
            //    StartTime = model.StartTime,
            //    EndTime = model.EndTime
            //};

            ApplicationUser userInDb = _mapper.Map<ApplicationUser>(model);

            IdentityResult identityResult = await _userManager.CreateAsync(userInDb, model.Password);

            if (identityResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(userInDb, role);
            }

            return identityResult;
        }



        public async Task<AuthDto> LoginUserAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            
            if (user != null)
            {
                bool found = await _userManager.CheckPasswordAsync(user, model.Password);

                if (found)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    var permissions = await _permissionService.GetUserPermissionsAsync(user.Id);

                    var permissionsJson = System.Text.Json.JsonSerializer.Serialize(permissions);

                    var jwtSecurityToken = await CreateJwtToken(user, permissionsJson);
                    var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                    return new AuthDto
                    {
                        ExpiresOn = jwtSecurityToken.ValidTo,
                        IsAuthenticated = true,
                        Roles = roles.ToList(),
                        Permissions = permissions,
                        Token = token,
                        FulllName = user.FullName,
                        Message = string.Empty
                    };
                }
            }

            return new AuthDto
            {
                IsAuthenticated = false,
                Message = "Email or password is incorrect"
            };
        }



        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user, string permissionsJson)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim(ClaimTypes.Role, role));

            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("Uid", user.Id),
                    new Claim("Permissions", permissionsJson) 
                }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ylsOukyEWbHPzMPH4eT0fHvD2JWagme2sQWBW+m+P38="));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(10),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }


        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            Role role = new Role()
            {
                Name = roleName
            };
            IdentityResult identityResult = await _roleManger.CreateAsync(role);

            return identityResult;
        }
        public async Task<IdentityResult> AddRoleToUserAsync(string userId, string roleName)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                IdentityResult identityResult = await _userManager.AddToRoleAsync(user, roleName);
                return identityResult;
            }
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }

        public IEnumerable<RoleDto> GetRolesAsync()
        {
            var roles = _roleManger.Roles.Select(r => new RoleDto()
            {
                RoleName = r.Name
            }).AsEnumerable();

            return roles;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

      
        public async Task<List<UserViewDto>> GetAllUsersAsync()
        {
            //var excludedRoles = new[] { "Hr", "User" };

            var users = _userManager.Users.ToList();
            var userList = new List<UserViewDto>();

            foreach (var user in users)
            {
                // Exclude users who have "HR" or "User" role
                //if (roles.Any(r => excludedRoles.Contains(r)))
                //    continue;
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserViewDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }

            return userList;
        }

        public async Task<UserViewDto?> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            return new UserViewDto
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles.ToList()
            };
        }

        public async Task<IdentityResult> CreateUserAsync(UserDto model)
        {
            var user = _mapper.Map<ApplicationUser>(model);
            SetDefaultUserValues(user);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded && !string.IsNullOrWhiteSpace(model.Role))
            {
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            return result;
        }

        private void SetDefaultUserValues(ApplicationUser user)
        {
            user.Nationality ??= DefaultUserValues.Nationality;
            user.NationalId ??= DefaultUserValues.NationalId;
            user.Address ??= DefaultUserValues.Address;
            user.Salary = user.Salary == 0 ? DefaultUserValues.Salary : user.Salary;
            user.DateOfBirth = user.DateOfBirth == default ? DefaultUserValues.DateOfBirth : user.DateOfBirth;
            user.ContractDate = user.ContractDate == default ? DefaultUserValues.ContractDate : user.ContractDate;
            user.StartTime = user.StartTime == default ? DefaultUserValues.StartTime : user.StartTime;
            user.EndTime = user.EndTime == default ? DefaultUserValues.EndTime : user.EndTime;
            user.DepartmentId ??= DefaultUserValues.DefaultDepartmentId;
            user.Gender = Gender.Unknown;
        }

        public async Task<IdentityResult> UpdateUserAsync(string id, UserDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            UpdateUserProperties(user, dto);

            if (!string.IsNullOrWhiteSpace(dto.Role))
            {
                var oldRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, oldRoles);
                await _userManager.AddToRoleAsync(user, dto.Role);
            }

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, dto.Password);
            }

            return await _userManager.UpdateAsync(user);
        }

        private void UpdateUserProperties(ApplicationUser user, UserDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.FullName))
                user.FullName = dto.FullName;

            if (!string.IsNullOrWhiteSpace(dto.UserName))
                user.UserName = dto.UserName;

            if (!string.IsNullOrWhiteSpace(dto.Email))
                user.Email = dto.Email;
        }

        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user == null
                ? IdentityResult.Failed(new IdentityError { Description = "User not found" })
                : await _userManager.DeleteAsync(user);
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }
    }
}