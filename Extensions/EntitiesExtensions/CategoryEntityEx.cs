using Atlob_Dent.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent
{
    public static class  CategoryEntityEx
    {
        public static bool AnySearchMatch(this List<Category> categories,string str)
        {
            return categories.Any(c => c.name.Contains(str,StringComparison.OrdinalIgnoreCase)&&c._HasAnyProducts());
        }
        public static IEnumerable<Product> SearchFor(this IEnumerable<Category> categories, string str)
        {
            return categories
                .Where(c => c.name.Contains(str, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(c => c.products.Count)
                .ThenByDescending(c => c.SubCategories.Count)
                .Select(c=>c._allProductsOfChilds())
                .FirstOrDefault();
        }
        public static bool _HasAnyProducts(this Category category)
        {
            if (category.SubCategories.Count==0)
                return category.products.Count > 0;
            foreach (var c in category.SubCategories)
            {
                if (c._HasAnyProducts()) return true;
            }
            return false;
        }
        /// <summary>
        /// get all products of category and products of it's child
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static IEnumerable<Product> _allProductsOfChilds(this Category category)
        {
            var products = new List<Product>();
            if (category.SubCategories.Count() == 0)
                return ServiceHelper.GetDbContext().Products.Where(p => p.categoryId == category.id);
            foreach (var c in category.SubCategories)
            {
                products.AddRange(c._allProductsOfChilds());
            }
            return products;

        }
        /// <summary>
        /// get all companies that participated in mading the category products
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static IQueryable<Company> _maderCompanies(this Category category)
        {
            return category.products.Select(p => p.company).AsQueryable();
        }
        /// <summary>
        /// check if any of parents of specified category match specified id
        /// </summary>
        /// <param name="category"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool AnyParentCategoryHasId(this Category category, Guid id)
        {
            if (category.superId == null) return false;
            if (category.superId == id) return true;
            if (category.SuperCategory != null)
                return category.SuperCategory.AnyParentCategoryHasId(id);
            else
            {
                return ServiceHelper.GetDbContext()
                    .Categories.FirstOrDefault(c => c.id == category.superId)
                    .AnyParentCategoryHasId(id);
            }
        }
    }
}
