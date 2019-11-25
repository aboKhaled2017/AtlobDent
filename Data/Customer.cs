using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Data
{
    public class Customer
    {
        [Key]
        public string id { get; set; }
        [Phone]
        [Required]
        public string phone { get; set; }
        public string otherPhone { get; set; }
        public int consumedProducts { get; set; } = 0;
        [ForeignKey("id")]
        public ApplicationUser User { get; set; }
    }
}
