using Atlob_Dent.Data;
using Atlob_Dent.Helpers;
using Atlob_Dent.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent
{
    public static class  productEntityEx
    {
        #region searching products helper functions
        /// <summary>
        /// check if at least one product name match the string 
        /// </summary>
        /// <param name="products"></param>
        /// <param name="str">the matching string</param>
        /// <returns></returns>
        public static bool AnySearchMatch(this IQueryable<Product> products, string str)
        {
            return products.Any(p => p.name.Contains(str, StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// search for products whose name matching particular string
        /// </summary>
        /// <param name="products"></param>
        /// <param name="str">the matching string</param>
        /// <returns></returns>
        public static IQueryable<Product> SearchFor(this IQueryable<Product> products, string str)
        {
            return products.Where(p => p.name.Contains(str, StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        #region select the product model formate to be exported to clients apps
        /// <summary>
        /// select the output product format to be sent to client by json
        /// </summary>
        /// <param name="productsModels"></param>
        /// <returns></returns>
        public static IQueryable<ProductModel> _selectCommonProductsStamp(this IQueryable<Product> productsModels)
        {
            return productsModels
                .Select(p => new ProductModel
                {
                    id = p.id,
                    name = p.name,
                    desc = p.desc,
                    prices = p.prices,
                    version = p.version,
                    //categoryId = p.categoryId,
                    companyId = p.companyId,
                    companyName = p.company.name,
                    images_url = p.images_url,
                    sizes = p.sizes
                });
        }
        public static IQueryable<ProductModel> _selectCommonProductsStamp(this IEnumerable<Product> productsModels)
        {
            return productsModels.AsQueryable()._selectCommonProductsStamp();
        }
        #endregion

        #region helper extension functions for getting similar products
        /// <summary>
        /// general similarities
        /// get all products that existes in target product category
        /// products may contains differents version for that same one
        /// products my be made by different companies for the same one
        /// </summary>
        /// <param name="product">target product</param>
        /// <returns></returns>
        public static IQueryable<Product> _SimilarProductsByType(this Product product)
        {
            return product.category
                 .products
                 .Where(p => p.id != product.id).AsQueryable();
        }
        /// <summary>
        /// get the products has different versions of target product
        /// the resulted product will has be under the same company and category
        /// </summary>
        /// <param name="product">traget product</param>
        /// <returns></returns>
        public static IEnumerable<Product> _SimilarProductsByVersion(this Product product)
        {
            var _context = ServiceHelper.GetDbContext();
            return _context.Products
                .Where(
                  p => p.id != product.id &&
                  p.name == product.name &&
                  p.categoryId == product.categoryId &&
                  p.companyId == product.companyId);
        }
        /// <summary>
        /// get the same product made by different companies
        /// the resulted products will have the same category
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static IEnumerable<Product> _SimilarProductsByCompany(this Product product)
        {
            var _context = ServiceHelper.GetDbContext();
            return _context.Products
                .Where(
                  p => p.id != product.id &&
                  p.name == product.name &&
                  p.categoryId == product.categoryId);
        }
        #endregion

        #region onSale products helper function
        /// <summary>
        /// select the onSale products format to be sent to client by json
        /// </summary>
        /// <param name="onSaleProducts"></param>
        /// <returns></returns>
        public static IQueryable<OnSaleProductModel> _selectCommonOnSaleProductsStamp(this IQueryable<OnSale> onSaleProducts)
        {
            return onSaleProducts
                .Select(p => new OnSaleProductModel
                {
                    id = p.productId,
                    name = p.product.name,
                    desc = p.product.desc,
                    prices = p.product.prices,
                    version = p.product.version,
                    //categoryId = p.categoryId,
                    companyId = p.product.companyId,
                    companyName = p.product.company.name,
                    images_url = p.product.images_url,
                    size = p.product.sizes,
                    discount=p.discount,
                    disPeriod=p.disPeriod
                });
        }
        public static IQueryable<OnSaleProductModel> _selectCommonOnSaleProductsStamp(this IEnumerable<OnSale> onSaleProducts)
        {
            return onSaleProducts.AsQueryable()._selectCommonOnSaleProductsStamp();
        }
        #endregion

        #region shared commont selected data
        public static PagingResponse<ExportedProductModel> GetResponsePages(this IOrderedQueryable<Product> prods, int? total, int skip, int take, HttpContext httpContext)
        {
            var data= prods
                .Skip(skip)
                 .Take(take)
                 ._selectCommonProductsStamp()
                 .GetExportedProductModels(httpContext).ToList();
            return new PagingResponse<ExportedProductModel>
            {
                count = data.Count,
                total = total??data.Count,
                data = data
            };
        }
        public static PagingResponse<ExportedOnSaleProductModel> GetResponsePages(this IOrderedQueryable<OnSale> prods, int total, int skip, int take, HttpContext httpContext)
        {
            var data = prods
                .Skip(skip)
                 .Take(take)
                 ._selectCommonOnSaleProductsStamp()
                 .GetExportedOnSaleProductModels(httpContext).ToList();
            return new PagingResponse<ExportedOnSaleProductModel>
            {
                count = data.Count,
                total = total,
                data = data
            };
        }
        #endregion

        #region converting product property value to other format
        public static List<string> ConvertToListOfStringValues(this string prop)
        {
            var values = new List<string>();
            prop.Replace("[", "").Replace("]", "").Replace("'", "").Split(',').ToList()
            .ForEach(val => {
                values.Add(val);
            });
            return values;
        }
        public static List<string> ConvertToListOfStringValues(this string prop,Func<string,string>format)
        {
            var values = new List<string>();
            prop.Replace("[", "").Replace("]", "").Replace("'", "").Split(',').ToList()
            .ForEach(val => {
                values.Add(format(val));
            });
            return values;
        }
        public static List<double> ConvertToListOfDoubleValues(this string prop)
        {
            var values = new List<double>();
            prop.Replace("[", "").Replace("]", "").Replace("'", "").Split(',').ToList()
            .ForEach(val => {
                values.Add(double.Parse(val));
            });
            return values;
        }
        #endregion
    }
}
