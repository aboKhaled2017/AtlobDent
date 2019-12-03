using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Data
{
    public class Address
    {
        public Guid id { get; set; }
        [Required]
        public string fullName { get; set; }
        [Required]
        public string country { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string streetInfo { get; set; }
        public string buildingNumber { get; set; }
        [Required]
        public string locationType { get; set; }
        [Required,Phone]
        public string phone { get; set; }
        [DataType(DataType.Text)]
        public string notes { get; set; }
        [Required]
        public string customerId { get; set; }
        [ForeignKey("customerId")]
        [InverseProperty("addresses")]
        public Customer customer { get; set; }
    }
}
