using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Models.AccountModels
{
    public class SigningInResponseModel
    {
        public UserResponseModel user { get; set; }
        public TokenModel accessToken { get; set; }
    }
    public class UserResponseModel
    {
        public string id { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string imgSrc { get; set; }
        public string name { get; set; }
        public bool hasPassword { get; set; } = true;
    }
}
