using Atlob_Dent.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        public static Atlob_dentEntities GetDbContext()
        {
            return _serviceScope.ServiceProvider.GetService<Atlob_dentEntities>();
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
    }
}
