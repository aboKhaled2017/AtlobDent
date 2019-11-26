using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent
{
    public enum OrderState
    {
        notReviewed = 0,
        reviewed = 1,
        recieved=2,
        canceled = 3,
        refused=4,
        notFound=5
    }
}
