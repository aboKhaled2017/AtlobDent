using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atlob_Dent.Data;
using Atlob_Dent.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atlob_Dent.Controllers
{
    public class MainController : ControllerBase
    {
        public readonly ILogger<MainController> _logger;
        public readonly Atlob_dentEntities _context;
        public MainController(Atlob_dentEntities context, ILogger<MainController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public void logError(string mess)
        {
            _logger.LogError("=========================================================================="+mess);
        }
       
    }
}
