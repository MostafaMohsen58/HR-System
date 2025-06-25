using HRManagementSystem.BL.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterDto model);
        Task<SignInResult> LoginUserAsync(LoginDto model);
        Task SignOutUserAsync();
        //EditProfileViewModel GetUserById(string userId);
        //Task<IdentityResult> UpdateUserAsync(EditProfileViewModel model);
        //Task<IdentityResult> DeleteUserAsync(string userId);
        //List<EditProfileViewModel> GetAllUsers();

        //UserProfileViewModel GetUserProfileById(string userId);
        //Task<IdentityResult> UpdateUserProfileAsync(UserProfileViewModel model);
        //Task<IdentityResult> UpdateUserProfilePersonalInfo(PersonalInformation model);
        //Task<IdentityResult> UpdateUserProfileAccountInfo(AccountInformation model, string userId);
    }
}
