using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Models
{
    public class ProductModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string prices { get; set; }
        public string images_url { get; set; }
        public string sizes { get; set; }
        public string desc { get; set; }
        //public Guid categoryId { get; set; }
        public Guid companyId { get; set; }
        public string  companyName { get; set; }
        public double version { get; set; }
    }
    public class ExportedProductModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public List<double> prices { get; set; }
        public List<string> images_url { get; set; }
        public List<string> sizes { get; set; }
        public string desc { get; set; }
        //public Guid categoryId { get; set; }
        public Guid companyId { get; set; }
        public string companyName { get; set; }
        public double version { get; set; } 
    }
    public class OnSaleProductModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string prices { get; set; }
        public string images_url { get; set; }
        public string size { get; set; }
        public string desc { get; set; }
        //public Guid categoryId { get; set; }
        public Guid companyId { get; set; }
        public string companyName { get; set; }
        public double version { get; set; }
        public double discount { get; set; }
        public int disPeriod { get; set; }
    }
    public class ExportedOnSaleProductModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public List<double> prices { get; set; }
        public string[] images_url { get; set; }
        public string[] sizes { get; set; }
        public string desc { get; set; }
        //public Guid categoryId { get; set; }
        public Guid companyId { get; set; }
        public string companyName { get; set; }
        public double version { get; set; }
        public int disPeriod { get; set; }
        public List<double> newPrices { get; set; }
    }
}
