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
        public Customer()
        {
            addresses = new HashSet<Address>();
        }
        [Key]
        public string id { get; set; }
        [Required]
        public string fullName { get; set; }
        [Phone]
        [Required]
        public string phone { get; set; }
        public int consumedProducts { get; set; } = 0;
        [ForeignKey("id")]
        public ApplicationUser User { get; set; }
        [InverseProperty("customer")]
        public ICollection<Address> addresses { get; set; }
        /*[InverseProperty("customer")]
        public ICollection<Order> orders { get; set; }*/
    }
}
