using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent
{
    public static class GlobalVariables
    {       
        public static string AdminRole { get; set; } = "Admin";
        public static string CustomerRole { get; set; } = "Customer";
        public static string AdminSectionNameOfConfigFile { get; set; } = "AdminData";
        
    }
}
