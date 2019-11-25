using Atlob_Dent.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent
{
    public static class  CompanyEntityEx
    {
        public static bool AnySearchMatch(this IQueryable<Company> companies,string str)
        {
            return companies.Any(c => c.name.Contains(str, StringComparison.OrdinalIgnoreCase) && c.products.Count > 0);
        }
        public static IEnumerable<Product> SearchFor(this IEnumerable<Company> companies, string str)
        {
           return companies.Where(c => c.name.Contains(str, StringComparison.OrdinalIgnoreCase) && c.products.Count > 0).Select(c => c.products).FirstOrDefault();
        }
    }
}
