using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atlob_Dent.Data;
using Atlob_Dent.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace Atlob_Dent.Helpers
{
    public class ProductHelper 
    {
        private readonly static Atlob_dent_Context _context = ServiceHelper.GetDbContext();
        
    }
}