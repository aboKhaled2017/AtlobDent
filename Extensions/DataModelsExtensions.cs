using Atlob_Dent.Data;
using Atlob_Dent.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent
{
    public static class DataModelsExtensions
    {
        #region product model helper functions
        public static ExportedProductModel GetExportedProductModel(this ProductModel productModel, HttpContext httpContext)
        {
            var exModel = new ExportedProductModel
            {
                id = productModel.id,
                name = productModel.name,
                desc = productModel.desc,
                //categoryId = productModel.categoryId,
                companyId = productModel.companyId,
                companyName = productModel.companyName,
                version = productModel.version,
                prices = new List<double>()
            };
            var images = productModel.images_url.Replace("[", "").Replace("]", "").Replace("'", "");
            var sizes = productModel.size.Replace("[", "").Replace("]", "").Replace("'", "");
            var prices = productModel.prices.Replace("[", "").Replace("]", "");
            exModel.images_url = images.Split(',');
            prices.Split(',').ToList().ForEach(price =>
            {
                exModel.prices.Add(double.Parse(price));
            });
            for (int i = 0; i < exModel.images_url.Length; i++)
            {
                exModel.images_url[i] = string.Format("{0}://{1}/images/products/{2}", httpContext.Request.Scheme, httpContext.Request.Host, exModel.images_url[i].Trim());
            }
            exModel.sizes = sizes.Split(',');
            return exModel;
        }
        public static IEnumerable<ExportedProductModel> GetExportedProductModels(this IEnumerable<ProductModel> productsModels, HttpContext httpContext)
        {
            foreach (var productModel in productsModels)
            {
                yield return productModel.GetExportedProductModel(httpContext);
            }
        }
        #endregion
        #region onsale product model helper functions
        public static ExportedOnSaleProductModel GetExportedOnSaleProductModel(this OnSaleProductModel onSaleProductModel, HttpContext httpContext)
        {
            var exModel = new ExportedOnSaleProductModel
            {
                id = onSaleProductModel.id,
                name = onSaleProductModel.name,
                desc = onSaleProductModel.desc,
                //categoryId = productModel.categoryId,
                companyId = onSaleProductModel.companyId,
                companyName = onSaleProductModel.companyName,
                version = onSaleProductModel.version,
                prices = new List<double>(),
                disPeriod= onSaleProductModel.disPeriod,
                newPrices=new List<double>()
            };
            var images = onSaleProductModel.images_url.Replace("[", "").Replace("]", "").Replace("'", "");
            var sizes = onSaleProductModel.size.Replace("[", "").Replace("]", "").Replace("'", "");
            var prices = onSaleProductModel.prices.Replace("[", "").Replace("]", "");
            exModel.images_url = images.Split(',');
            prices.Split(',').ToList().ForEach(price =>
            {
                exModel.prices.Add(double.Parse(price));
                exModel.newPrices.Add((double.Parse(price))*(onSaleProductModel.discount/100));
            });
            for (int i = 0; i < exModel.images_url.Length; i++)
            {
                exModel.images_url[i] = string.Format("{0}://{1}/images/products/{2}", httpContext.Request.Scheme, httpContext.Request.Host, exModel.images_url[i].Trim());
            }
            exModel.sizes = sizes.Split(',');
            return exModel;
        }
        public static IEnumerable<ExportedOnSaleProductModel> GetExportedOnSaleProductModels(this IEnumerable<OnSaleProductModel> onSaleProductsModels, HttpContext httpContext)
        {
            foreach (var onSaleProductModel in onSaleProductsModels)
            {
                yield return onSaleProductModel.GetExportedOnSaleProductModel(httpContext);
            }
        }
        #endregion
        public static IEnumerable<CompanyModel> _selectCompaniesModels(this IEnumerable<Company> companies, HttpContext httpContext)
        {
            return companies
                 .Select(c => new CompanyModel
                 {
                     id = c.id,
                     name = c.name,
                     imgsrc = string.Format("{0}://{1}/images/companies/{2}",
                     httpContext.Request.Scheme, httpContext.Request.Host, c.imgsrc.Trim())
                 });
        }
    }
}
