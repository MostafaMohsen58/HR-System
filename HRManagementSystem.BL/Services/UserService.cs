using HRManagementSystem.BL.DTOs;
using HRManagementSystem.BL.Interfaces;
using HRManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDto model)
        {
            ApplicationUser userInDb = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                Address = model.Address
            };

            IdentityResult identityResult = await _userManager.CreateAsync(userInDb, model.Password);

            if (identityResult.Succeeded)
            {
                //should be dynamic passed in parameter
                await _userManager.AddToRoleAsync(userInDb, "User");
                await _signInManager.SignInAsync(userInDb, false);//create cookie without remember me option
            }
            
            return identityResult;
        }


        public async Task<SignInResult> LoginUserAsync(LoginDto model)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                //check password
                bool found = await _userManager.CheckPasswordAsync(user, model.Password);
                if (found)
                {
                    //create token instead of cookie

                    await _signInManager.SignInAsync(user, model.RememberMe);
                    return SignInResult.Success;
                }
            }
            return SignInResult.Failed;
        }

        

        public async Task SignOutUserAsync()
        {
            await _signInManager.SignOutAsync();
            //delete token if exists
        }
    }
}
