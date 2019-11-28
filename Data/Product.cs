using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Data
{
    public class Product
    {
        public Product()
        {
             
        }
        [Required]
        public Guid id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string prices { get; set; }
        [Required]
        public string images_url { get; set; }
        [Required]
        public string sizes { get; set; } 
        [DataType(DataType.Text)]
        public string desc { get; set; }
        [Required]
        public Guid categoryId { get; set; }
        [ForeignKey("categoryId")]
        [InverseProperty("products")]
        public Category category { get; set; }
        [Required]
        public Guid companyId { get; set; }
        [ForeignKey("companyId")]
        [InverseProperty("products")]
        public Company company { get; set; }
        public double version { get; set; } = 1;
        public long seen { get; set; } = 0;
        public long ordersCount { get; set; } = 0;
        public long consumedCount { get; set; } = 0;
        [DataType(DataType.Date)]
        public DateTime createdDate { get; set; } = DateTime.Now;
    }
}
