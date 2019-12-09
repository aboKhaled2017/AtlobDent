using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Atlob_Dent.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
namespace Atlob_Dent.Helpers
{
    public static class UserHelper
    {
        private static UserManager<ApplicationUser> _userManger = ServiceHelper.GetUserManager();
        public static async Task<ApplicationUser> createCustomerUserByPhone(string phone)
        {
            var identityResult = new IdentityResult();
                var user = new ApplicationUser { 
            SecurityStamp=Guid.NewGuid().ToString(),
            UserName=phone,
            PhoneNumber=phone
            };
            identityResult = await _userManger.CreateAsync(user, "Customer@123");
            if(identityResult.Succeeded)
                identityResult= await _userManger.AddToRoleAsync(user, GlobalVariables.CustomerRole);
            return identityResult.Succeeded?user:null;
        }
        public static async Task<ApplicationUser> createCustomerUserByEmail(string email)
        {
            var identityResult = new IdentityResult();
            var user = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                Email = email,
                UserName = email
            };
            identityResult = await _userManger.CreateAsync(user, "Customer@123");
            if (identityResult.Succeeded)
                identityResult = await _userManger.AddToRoleAsync(user, GlobalVariables.CustomerRole);
            return identityResult.Succeeded ? user : null;
        }
    }
}