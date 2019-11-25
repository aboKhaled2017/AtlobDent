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
    [Route("api/setting/[action]")]
    [ApiController]
    public class SettingController : MainController
    {
        public SettingController(Atlob_dent_Context context, ILogger<MainController> logger) : base(context, logger)
        {
        }
        /// <summary>
        /// get the main config setting of application
        /// these setting is controlled by 
        /// adminstrator
        /// </summary>
        /// <example>domain/api/setting/config</example>
        /// <returns></returns>
        public IActionResult Config()
        {
            var configureApi = new {
            product=new { 
            pageLength=10,
            mostBought_by=6,
            newllyCreated_by=6,
            },
            };
            return Ok(configureApi);
        }
    }
}
