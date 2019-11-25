using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomized_MainAppConfiguration(this IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            MainConfigHelper.IntializeServiceHelper(serviceProvider);           
            return app;
        }
        public static IApplicationBuilder UseDefaultSeededData(this IApplicationBuilder app)
        {
            ServiceHelper.GetDbContext().Database.Migrate();
            MainConfigHelper.SeedDefaultedData();
            return app;
        }
    }
}
