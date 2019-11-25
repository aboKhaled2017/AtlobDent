using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Data
{
    public class Comment
    {
        public Comment()
        { 
        }
        [Required]
        public Guid id { get; set; }
        [Required]
        public string message { get; set; }
        [Phone]
        [Required]
        public string phone { get; set; }
        /*[ForeignKey("phone")]
        public Customer customer { get; set; }*/
    }
}
