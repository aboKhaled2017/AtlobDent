using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Data
{
    public class OnSale
    {
        public OnSale()
        {
             
        }
        [Key]
        public Guid productId { get; set; }
        [Required]
        public double discount { get; set; }
        public int disPeriod { get; set; }
        [ForeignKey("productId")]
        public Product product { get; set; }
    }
}
