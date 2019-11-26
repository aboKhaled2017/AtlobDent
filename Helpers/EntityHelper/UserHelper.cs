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
        public static ApplicationUser createCustomerUserByPhone(string phone)
        {
                var user = new ApplicationUser { 
            SecurityStamp=Guid.NewGuid().ToString(),
            UserName=phone,
            PhoneNumber=phone
            };
            _userManger.CreateAsync(user,phone);
            _userManger.AddToRoleAsync(user, GlobalVariables.CustomerRole);
            return user;
        }
        public async static Task RollBackUser(ApplicationUser user)
        {
           await _userManger.DeleteAsync(user);
        }
    }
}