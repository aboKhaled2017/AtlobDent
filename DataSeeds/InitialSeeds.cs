using Atlob_Dent.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.DataSeeds
{
    public static class InitialSeeds
    {
        private static readonly RoleManager<IdentityRole> _roleManager = ServiceHelper.GetRoleManager();
        private static readonly UserManager<ApplicationUser> _userManager = ServiceHelper.GetUserManager();
        private static readonly IConfiguration _configuration = ServiceHelper.GetConfiguration();
        private static readonly Atlob_dent_Context _context = ServiceHelper.GetDbContext();
        public static async Task SeedRequireds()
        {
           await SeedRoles(new List<string>{GlobalVariables.AdminRole,GlobalVariables.CustomerRole});
           await SeedUsersAsAdmins();
        }
        private static async Task SeedRoles(List<string>rolesNam)
        {
            foreach (var name in rolesNam)
            {           
                if (! await _roleManager.RoleExistsAsync(name))
                {
                    var role = new IdentityRole(name);
                    await _roleManager.CreateAsync(role);
                }
            }
        }
        private static async Task SeedUsersAsAdmins()
        {
            if (_context.Admins.Any()) return;
            var adminDataSection = _configuration.GetSection(GlobalVariables.AdminSectionNameOfConfigFile);
            ApplicationUser userAdmin = new ApplicationUser {
            Email=adminDataSection.GetValue<string>("email"),
            UserName= adminDataSection.GetValue<string>("userName"),
            SecurityStamp=Guid.NewGuid().ToString()
            };
           await _userManager.AddPasswordAsync(userAdmin, "Admin@123");
           await _userManager.CreateAsync(userAdmin);
           await _userManager.AddToRoleAsync(userAdmin,GlobalVariables.AdminRole);
           var admin = new Admin { id = userAdmin.Id, fullName = adminDataSection.GetValue<string>("fullName")};
            _context.Admins.Add(admin);
            _context.SaveChanges();
        }
    }
}
