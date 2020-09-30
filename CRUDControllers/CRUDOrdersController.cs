using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Atlob_Dent.Data;
using Atlob_Dent.Controllers;
using Microsoft.Extensions.Logging;
using Atlob_Dent.Models;
using Atlob_Dent.Helpers;
using Atlob_Dent.Services;
using Microsoft.AspNetCore.Authorization;

namespace Atlob_Dent.CRUDControllers
{
    [Route("api/CRUD/Orders")]
    [ApiController]
    public class CRUDOrdersController : MainController
    {
        public CRUDOrdersController(Atlob_dent_Context context) : base(context)
        {
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
               var a = User;
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(Guid id, Order order)
        {
            if (id != order.id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        /// <summary>
        /// to make order
        /// </summary>
        /// <example>domain/api/CRUD/Orders</example>
        /// <param name="cartCheckoutModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder([FromBody]CartCheckoutModel cartCheckoutModel)
        {
           // bool isModelValid = !TryValidateModel(cartCheckoutModel, nameof(CartCheckoutModel));
            if (!ModelState.IsValid)
                return BadRequest(new NotValidDataResponse());
            var makeOrdersResponseResult =await new OrdersService(_context, cartCheckoutModel).ProcessOrderRequest();
            if(makeOrdersResponseResult.status)
                return Ok(makeOrdersResponseResult);
                return BadRequest(makeOrdersResponseResult);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private bool OrderExists(Guid id)
        {
            return _context.Orders.Any(e => e.id == id);
        }
    }
}
