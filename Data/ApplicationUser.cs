using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Data
{
    public class ApplicationUser:IdentityUser
    {
        [MaxLength(8)]
        public string confirmCode { get; set; }
    }
}
