using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Models.ServicesModels
{
    public class EmailConfigModel
    {
        public string from { get; set; }
        public string password { get; set; }
        public bool writeAsFile { get; set; }
    }
}
