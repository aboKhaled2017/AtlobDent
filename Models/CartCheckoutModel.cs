using Atlob_Dent.Utilities.CustomeValidation;
using Microsoft.AspNetCore.Mvc;
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
        [Required(ErrorMessage ="full name is required")]
        [StringLength(50,MinimumLength =3,ErrorMessage ="full name is not valid")]
        public string fullName { get; set; }
        [Phone]
        [Required(ErrorMessage = "phone is required")]
        public string phone { get; set; }
        [Required(ErrorMessage ="address is required"),MinLength(5)]
        public string address { get; set; }
    }
    
    public class CartOrderModel
    {
        [Required(ErrorMessage ="product Id is required")]
        [ValidateCartOrder("sizeIndex", "productId")]
        //[IsProductExists]
        public Guid productId { get; set; }
        [Required(ErrorMessage ="quantity is required")]
        [Range(1, Int64.MaxValue, ErrorMessage = "quantity is not valid")]
        public int quantity { get; set; }    
        [Required(ErrorMessage ="size index is required")]
        [Range(0, Byte.MaxValue, ErrorMessage = "not valid number")]
        public byte sizeIndex { get; set; }
    }
}
