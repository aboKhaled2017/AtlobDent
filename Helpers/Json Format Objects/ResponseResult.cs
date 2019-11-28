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
    }
    public class SuccessResponseResult:ResponseResult
    {
        public override bool status { get; set; } = true;
        public override string message { get; set; }= "the request has been proccessed successfully";
        public object data { get; set; }
    }
    public class BadResponseResult:ResponseResult
    {
        public override bool status { get; set; } = false;
        public override string message { get; set; } = "unhandled error in server";
    }
    public class NotValidDataResponse:BadResponseResult
    {
        public override string message { get; set; } = "data is not valid";
        public object errorFields { get; set; }
        public int errorsFieldsCount { get; set; }
    }

}
