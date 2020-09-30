using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Models.AccountModels
{
    public class ResetPasswordModel
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required,MinLength(3)]
        public string code { get; set; }
    }
}
