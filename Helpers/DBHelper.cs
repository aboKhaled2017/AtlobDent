using Atlob_Dent.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent
{
    public static class DBHelper
    {
        public static void ResetData(Atlob_dentEntities context)
        {
            context.Comments.RemoveRange(context.Comments);
            context.SaveChanges();
            context.Users.RemoveRange(context.Users);
            context.SaveChanges();
            /*context.Orders.RemoveRange(context.Orders);
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
