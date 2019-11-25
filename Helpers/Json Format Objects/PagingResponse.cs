using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Helpers
{
    public class PagingResponse<T>
    {
        public int total { get; set; }
        public List<T> data { get; set; }
        public int count { get; set; }
    }
}
