using Atlob_Dent.Data;
using Atlob_Dent.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent
{
    public static class HelperExtensionMethods
    {
        public static IQueryable<ExportedProductModel> GetCommonMostllySelectedData(this IOrderedQueryable<Product> prods, int skip, int take,HttpContext httpContext)
        {
            return prods
                .Skip(skip)
                 .Take(take)
                 ._selectCommonProductsStamp()
                 .GetExportedProductModels(httpContext).AsQueryable();
        }
    }
}
