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
    [Route("api/Atlob_company/[action]")]
    [ApiController]
    public class CompanyController : MainController
    {
        public CompanyController(Atlob_dent_Context context, ILogger<MainController> logger) : base(context, logger)
        {
        }
        /// <summary>
        /// get all companies data
        /// </summary>
        /// <example>dmain/api/Atlob_company/all</example>
        /// <returns>list of companies</returns>
        public  IActionResult All()
        {
            try
            {                
                return Ok(
                     _context.Companies
                    ._selectCompaniesModels(HttpContext)
                    .ToList());
            }
            catch
            {
                return BadRequest("error in server");
            }
        }
        /// <summary>
        /// get all companies that participated in making target product
        /// </summary>
        /// <param name="name">product name</param>
        /// <example>dmain/api/Atlob_company/MakerForProduct/72c8b403-615e-0000-870e-fa7621b8b735</example>
        /// <returns></returns>
        [HttpGet("{name}")]
        public async Task<IActionResult> MakerForProduct(string name)
        {
            try
            {
                return Ok(
                       (await
                      _context.Products
                      .Where(p=>p.name==name)
                      .Include(p=>p.company)
                      .Select(p=>p.company)
                      .Distinct().ToListAsync())
                      ._selectCompaniesModels(HttpContext)                                         
                      );
            }
            catch
            {
                return BadRequest("error in server");
            }
        }
        /// <summary>
        /// get all companies has been made products for specified category
        /// </summary>
        /// <example>dmain/api/Atlob_company/MakerForCategory/72c8b403-615e-5522-870e-fa7621b8b735</example>
        /// <param name="id">category id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> MakerForCategory(Guid id)
        {
            try
            {
                return Ok(
                         (await _context.Products
                        .Where(p => p.categoryId == id || p.category.AnyParentCategoryHasId(id))
                        .Select(p => p.company)
                        .Distinct()
                        .ToListAsync())
                        ._selectCompaniesModels(HttpContext)                                                                   
                      );           
            }
            catch
            {
                return BadRequest("error in server");
            }
        }
    }
}
