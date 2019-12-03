using Atlob_Dent.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
 
using System.Linq;
 

namespace Atlob_Dent
{
    public static class GlobalProperties
    {
        //private static readonly RoleManager<IdentityRole> _roleManager = ServiceHelper.GetRoleManager();
        private static readonly UserManager<ApplicationUser> _userManager = ServiceHelper.GetUserManager();        
        private static readonly Atlob_dent_Context _context = ServiceHelper.GetDbContext();
        public static readonly IConfiguration configuration = ServiceHelper.GetConfiguration();
        private static ApplicationUser _getMainAdminUser()
        {
            string mainAdminUserName = configuration
                .GetSection(GlobalVariables.AdminSectionNameOfConfigFile)
                .GetValue<string>("userName");
            return _userManager.Users.FirstOrDefault(a => a.UserName == mainAdminUserName);
        }
        public static ApplicationUser MainAdminUser { get { return _getMainAdminUser(); } set { } }
        public static Admin MainAdmin { get { return _context.Admins.Find(MainAdminUser.Id); } set { } }
        
    }
}
