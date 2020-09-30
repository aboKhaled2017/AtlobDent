using Atlob_Dent.Utilities.CustomeValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Models.AccountModels
{
    public class RegisterModel
    {
        [Required,StringLength(50,MinimumLength =7)]
        public string fullName { get; set; }
        [Required,Phone]
        [CheckIfUserPropValueIsExixts("phone", UserPropertyType.phone)]
        public string phone { get; set; }

        [Required]
        [EmailAddress]
        [CheckIfUserPropValueIsExixts("email",UserPropertyType.email)]
        public string email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
