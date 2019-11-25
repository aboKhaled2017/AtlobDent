using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Models
{
    public class CompanyModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string imgsrc { get; set; }
    }
    public class CompanyModelComparer : IEqualityComparer<CompanyModel>
    {
        
        public bool Equals(CompanyModel x, CompanyModel y)
        {
            return x.id == y.id;
        }

        public int GetHashCode(CompanyModel obj)
        {
            return obj.GetHashCode();
        }
    }
}
