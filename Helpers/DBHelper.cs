using Atlob_Dent.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent
{
    public static class DBHelper
    {
        public static async Task ResetData(Atlob_dent_Context context)
        {
            var userManager = ServiceHelper.GetUserManager();
            var customerUsers =await  userManager.GetUsersInRoleAsync(GlobalVariables.CustomerRole);
            foreach (var user in  customerUsers)
            {
               await userManager.DeleteAsync(user);
            }
            context.Comments.RemoveRange(context.Comments);
            context.SaveChanges();
            /*context.Customers.RemoveRange(context.Customers);
            context.SaveChanges();
            context.Orders.RemoveRange(context.Orders);
            context.SaveChanges();
            context.Products.RemoveRange(context.Products);
            context.SaveChanges();*/
            context.Companies.RemoveRange(context.Companies);
            context.SaveChanges();
            context.Categories.RemoveRange(context.Categories);
            context.SaveChanges();                      
        }
    }
}
