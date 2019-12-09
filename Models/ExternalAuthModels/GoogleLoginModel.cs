using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Models.ExternalAuthModels
{
    public class GoogleLoginModel
    {
        [Required]
        [MinLength(50)]
        public string googleToken { get; set; }
    }
    public class GoogleUserModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string pictureSrc { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;
    }

}
