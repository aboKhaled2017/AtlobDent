using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atlob_Dent.Controllers;
using Atlob_Dent.Data;
using Atlob_Dent.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Atlob_Dent.CUDControllers
{
    [Route("api/CRUD/Product/[action]")]
    [ApiController]
    public class CRUDProductController : MainController
    {
        public CRUDProductController(Atlob_dent_Context context, ILogger<MainController> logger) : base(context, logger)
        {
        }
        /// <summary>
        /// increment product seen by 1
        /// </summary>
        /// <example>domain/api/CRUD/Product/IncrementSeen [post method]</example>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> IncrementSeen(Guid id)
        {
            try
            {
                var TargetProduct = _context.Products.Find(id);
                if (TargetProduct == null)
                    return NotFound(new BadResponseResult{ message=string.Format("the id ${0} is not found",id)});
                TargetProduct.seen += 1;
                _context.Entry(TargetProduct).Property(p => p.seen).IsModified = true;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return BadRequest(new BadResponseResult());
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        public bool IsProductExists(Guid productId)
        {
            return _context.Products.Any(p => p.id == productId);
        }
    }
}
