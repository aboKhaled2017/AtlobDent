using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Data
{
    public class Admin
    {
        [Key]
        public string id { get; set; }
        [Required]
        public string fullName { get; set; }
        [ForeignKey("id")]
        public ApplicationUser User { get; set; }
    }
}
