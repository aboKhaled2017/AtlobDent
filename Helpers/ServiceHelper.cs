using Atlob_Dent.Data;
using Atlob_Dent.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent
{
    public static class ServiceHelper
    {
        private static IServiceProvider _serviceProvider { get; set; }
        private static IServiceScope _serviceScope{get;set;}
        public static void IntializeServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serviceScope=_serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        }
        public static Atlob_dent_Context GetDbContext()
        {
            return _serviceScope.ServiceProvider.GetService<Atlob_dent_Context>();
        }
        public static IHostingEnvironment GetHostingEnv()
        {
            return _serviceScope.ServiceProvider.GetService<IHostingEnvironment>();
        }
        public static ILogger<T> GetLogger<T>()
        {
            return _serviceScope.ServiceProvider.GetService<ILogger<T>>();
        }
        public static HttpContext GetHttpContext()
        {
            return _serviceScope.ServiceProvider.GetService<HttpContext>();
        }
        public static UserManager<ApplicationUser> GetUserManager()
        {
            return _serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
        }
        public static RoleManager<IdentityRole> GetRoleManager()
        {
            return _serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
        }
        public static SignInManager<ApplicationUser> GetSignInManager()
        {
            return _serviceScope.ServiceProvider.GetService<SignInManager<ApplicationUser>>();
        }
        public static IConfiguration GetConfiguration()
        {
            return _serviceScope.ServiceProvider.GetService<IConfiguration>();
        }
        public static TransactionHelper GetTransactionHelper()
        {
            return _serviceScope.ServiceProvider.GetService<TransactionHelper>();
        }
    }
}
