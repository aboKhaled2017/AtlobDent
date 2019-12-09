using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Models
{
    public class TokenModel
    {
        public string token { get; set; }
        public long expiry { get; set; }
    }
    public class AuthTokensModel
    {
        public TokenModel accessToken { get; set; }
        public TokenModel refreshToken { get; set; }
    }
}
