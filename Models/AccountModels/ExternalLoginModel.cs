using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Models.AccountModels
{
    public class ExternalLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
