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
        [Key,Phone]
        public string phone { get; set; }
        [Required]
        public string fullname { get; set; }
        [Required]
        public string role { get; set; } = "manager";
    }
}
