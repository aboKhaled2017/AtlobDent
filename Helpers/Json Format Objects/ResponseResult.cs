using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Helpers
{
    public class ResponseResult
    {
        public virtual bool status { get; set; } = true;
        public virtual string message { get; set; }
        public object data { get; set; }
    }
    public class BadResponseResult
    {
        public virtual bool status { get; set; } = false;
        public virtual string message { get; set; } = "unhandled error in server";
    }

}
