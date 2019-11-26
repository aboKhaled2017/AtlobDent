using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Data
{
    public class Order
    {
        public Order()
        { 
        }
        [Required]
        public Guid id { get; set; }
        /*[Required]
        public string fullname { get; set; }
        [Phone]
        [Required]
        public string phone { get; set; }*/
        [Required]
        public string address { get; set; }
        [Required]
        public int quantity { get; set; }
        public byte sizeIndex { get; set; }
        public DateTime orderDate { get; set; } = DateTime.Now;
        public OrderState state { get; set; } = OrderState.notReviewed;
        [Required]
        public Guid productId { get; set; }
        [ForeignKey("productId")]
        public Product product { get; set; }
        [Required]
        public string customerId { get; set; }
        [ForeignKey("customerId")]
        public Customer customer { get; set; }

    }
}
