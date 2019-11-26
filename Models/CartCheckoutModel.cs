using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Models
{
    public class CartCheckoutModel
    {
        public CartCustomerModel customer { get; set; }
        public List<CartOrderModel> orders { get; set; }
    }
    public class CartCustomerModel
    {
        [Required]
        public string fullName { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
    }
    public class CartOrderModel
    {
        public Guid productId { get; set; }
        public int quantity { get; set; }    
        public byte sizeIndex { get; set; }
    }
}
