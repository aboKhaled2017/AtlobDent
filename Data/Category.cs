using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Data
{
    public class Category
    {
        public Category()
        {
            SubCategories = new HashSet<Category>();
            products = new HashSet<Product>();
        }
        [Required]
        public Guid id { get; set; }
        [Required]
        public string name { get; set; }
        public Guid? superId { get; set; }
        [ForeignKey("superId")]
        [InverseProperty("SubCategories")]
        public Category SuperCategory { get; set; }
        [InverseProperty("SuperCategory")]
        public ICollection<Category> SubCategories { get; set; }
        [InverseProperty("category")]
        public ICollection<Product> products { get; set; }

    }
}
