using Atlob_Dent.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent
{
    public static class UserIdentityExtensions
    {
        public async static Task<bool> UserIdentityExists(this UserManager<ApplicationUser> userManager,ApplicationUser user,string password)
        {
            return user != null &&
                await userManager.CheckPasswordAsync(user,password);
        }
        public async static Task<bool> UserIdentityExists(this UserManager<ApplicationUser> userManager, ApplicationUser user, string password, string role)
        {
            return user != null &&
                await userManager.IsInRoleAsync(user,role) &&
                await userManager.CheckPasswordAsync(user, password);
        }
        public async static Task<string> GeneratePasswordResetCodeAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            var code = "";
            do
            {
                for (int i = 0; i < 5; i++)
                {
                    code += new Random().Next(0, 9);
                }
            }
            while (userManager.Users.Any(u => u.confirmCode == code.ToString()));
            user.confirmCode = code.ToString();
            await userManager.UpdateAsync(user);
            return code;
        }
        public async static Task<IdentityResult> ResetPasswordByCodeAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user, string code,string password)
        {
            var confirmedUser = await userManager.Users.FirstOrDefaultAsync(u => u.confirmCode == code);
            var result = new IdentityResult();
            var codeError = new IdentityError { Code = "confirmCode", Description = "this reset password code is not valid" };
            if (confirmedUser == null)
            {
                result.Errors.Append(codeError);
                return result;
            }
            confirmedUser.confirmCode = null;
            result=await userManager.RemovePasswordAsync(confirmedUser);
            if (!result.Succeeded) return result;
            result=await userManager.AddPasswordAsync(user, password);
            if (!result.Succeeded) return result;
            return await userManager.UpdateAsync(confirmedUser);
        }
        public async static Task<string> GenerateEmailConfirmationCodeAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            var code = "";
            do
            {
                for(int i = 0; i < 5; i++)
                {
                    code += new Random().Next(0, 9);
                }                
            }
            while (userManager.Users.Any(u => u.confirmCode == code.ToString()));
            user.confirmCode = code.ToString();
            await userManager.UpdateAsync(user);
            return code;
        }
        public async static Task<IdentityResult> ConfirmEmailByCodeAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user,string code)
        {
            var confirmedUser =await userManager.Users.FirstOrDefaultAsync(u => u.confirmCode == code);
            var result = new IdentityResult();
            var codeError = new IdentityError { Code = "confirmCode", Description = "this confirm code is not valid" };
            if (confirmedUser == null)
            {
                result.Errors.Append(codeError);
                return result;
            }
            confirmedUser.EmailConfirmed = true;
            confirmedUser.confirmCode = null;
            return await userManager.UpdateAsync(confirmedUser);
        }
    }   
}
