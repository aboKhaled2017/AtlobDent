using Atlob_Dent.Data;
using Microsoft.AspNetCore.Identity;
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
    }
}
