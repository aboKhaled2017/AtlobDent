using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlob_Dent.Data
{
    public class Company
    {
        public Company()
        {
            products = new HashSet<Product>();
        }
        [Required]
        public Guid id { get; set; }
        [Required]
        public string name { get; set; }
        public string imgsrc { get; set; }
        [InverseProperty("company")]
        public ICollection<Product> products { get; set; }
    }
}
