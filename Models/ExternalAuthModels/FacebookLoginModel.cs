using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Models.ExternalAuthModels
{
    public class FacebookLoginModel
    {
        [Required]
        [StringLength(255)]
        public string facebookToken { get; set; }
    }
    public class FacebookUserModel
    {
        public string id { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string email { get; set; }
        public string pictureSrc { get; set; } 
    }

}
