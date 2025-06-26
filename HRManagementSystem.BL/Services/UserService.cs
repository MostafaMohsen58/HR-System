using HRManagementSystem.BL.DTOs;
using HRManagementSystem.BL.Interfaces;
using HRManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManger;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManger = roleManager;
        }
        public async Task<IdentityResult> RegisterUserAsync(RegisterEmployeeDto model, string role)
        {
            ApplicationUser userInDb = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                Address = model.Address,
                FullName = model.FullName,
                NationalId = model.NationalId,
                Nationality= model.Nationality,
                Salary= model.Salary,
                Gender= model.Gender,
                DateOfBirth = model.DateOfBirth,
                ContractDate = model.ContractDate,
                StartTime = model.StartTime,
                EndTime = model.EndTime
            };

            IdentityResult identityResult = await _userManager.CreateAsync(userInDb, model.Password);

            if (identityResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(userInDb, role);
            }

            return identityResult;
        }

        

        public async Task<AuthDto> LoginUserAsync(LoginDto model)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                //check password
                bool found = await _userManager.CheckPasswordAsync(user, model.Password);
                if (found)
                {
                    //create token instead of cookie
                    var jwtSecurityToken = await CreateJwtToken(user);

                    var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                    return new AuthDto
                    {
                        ExpiresOn = jwtSecurityToken.ValidTo,
                        IsAuthenticated = true,
                        Roles = new List<string> { "User" },
                        Token = token,
                        FulllName = user.FullName,
                        Message = string.Empty
                    };
                }
            }
            return new AuthDto
            {
                IsAuthenticated = false,
                Message = "Email or password is Incorrect"
            };
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim(ClaimTypes.Role, role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("Uid",user.Id)
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
            IdentityRole role = new IdentityRole()
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

        public async Task<IEnumerable<RoleDto>> GetRolesAsync()
        {
            var roles = _roleManger.Roles.Select(r => new RoleDto()
            {
                RoleName = r.Name
            }).AsEnumerable();

            return roles;
        }
    }
}
