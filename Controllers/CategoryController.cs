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
    [Route("api/Atlob_category/[action]")]
    [ApiController]
    ///<summary>
    ///provide all APIs releated to category
    /// </summary>
    public class CategoryController : MainController
    {
        public CategoryController(Atlob_dentEntities context, ILogger<MainController> logger) : base(context, logger)
        {
        }
        /// <summary>
        /// get all intial data for mainPage of mobile
        ///data are {categories,companies}
        /// </summary>
        /// <example>domain/api/Atlob_category/AppHeaderData</example>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AppHeaderData()
        {
            var categories = _context.Categories
               .Where(c => c.superId == null)
               .Include(c => c.products)
               .ThenInclude(prod => prod.company)
            .Select(c => new MainCategoryModel
            {
                id = c.id,
                name = c.name,
                childs = c.SubCategories
                   .Select(sub => new SubCategoryModel
                   {
                       id = sub.id,
                       name = sub.name,
                   })
            })
            .ToList();
            return Ok(new MainAppDataModel {categories=categories});         
        }        
    }
}
