using Atlob_Dent.DataSeeds;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent
{
    public static class MainConfigHelper
    {
        public static void IntializeServiceHelper(IServiceProvider serviceProvider)
        {
            ServiceHelper.IntializeServiceProvider(serviceProvider);
        }
        public static void SeedDefaultedData()
        {
            DBHelper.ResetData(ServiceHelper.GetDbContext()).Wait();
            InitialSeeds.SeedRequireds().Wait();
            Seeder.SeedToAllTables();
        }
    }
}
