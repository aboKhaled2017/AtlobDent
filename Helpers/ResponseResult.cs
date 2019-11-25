using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Helpers
{
    public class ResponseResult
    {
        public bool status { get; set; } = true;
        public string message { get; set; }
        public object data { get; set; }
    }
}
