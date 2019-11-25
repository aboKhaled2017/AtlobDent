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
            DBHelper.ResetData(ServiceHelper.GetDbContext());
            Seeder.SeedToAllTables();
        }
    }
}
