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

namespace Atlob_Dent.CRUDControllers
{
    [Route("api/CRUD/[controller]")]
    [ApiController]
    public class OrdersController : MainController
    {
        public OrdersController(Atlob_dent_Context context) : base(context)
        {
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
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
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(CartCheckoutModel cartCheckoutModel)
        {
            try
            {
                var result =await CustomerHelper.RegisterCustomerIfNotExists(cartCheckoutModel.customer,cartCheckoutModel.orders.Count);
                result.Item2.RollBackAction();
                /*if (ModelState.IsValid)
                {
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();
                    ModelState.Values
                    return CreatedAtAction("GetOrder", new { id = order.id }, order);
                }*/
                return BadRequest(new NotValidDataResponse {errorFields=ModelState.Values,errorsFieldsCount=ModelState.ErrorCount});
            }
            catch
            {
                return BadRequest(new BadResponseResult {});
            }
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
