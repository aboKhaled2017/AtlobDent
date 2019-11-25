using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Models
{
    public class MainCategoryModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public IEnumerable<SubCategoryModel> childs { get; set; }
    }
    public class SubCategoryModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
    }
}
