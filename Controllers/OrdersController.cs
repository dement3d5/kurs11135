using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kurs1135.DB;
using Kurs1135.Models;

namespace Kurs1135.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly user17_dbContext _context;

        public OrdersController(user17_dbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpPost("get")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("SaveOrder")]
        public async Task<ActionResult<Order>> PostOrder([FromBody] Order order)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            /*_context.Entry(order.Status).;
            _context.Entry(order.User).State = EntityState.Unchanged;
            _context.Entry(order.Product).State = EntityState.Unchanged;*/
            order.Status = _context.OrderStatuses.FirstOrDefault(s => s.Id == order.StatusId);
            order.User = _context.Users.FirstOrDefault(s => s.Id == order.UserId);
            order.Product = _context.Products.FirstOrDefault(s => s.Id == order.ProductId);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            if (order == null)
            {
                return NoContent();
            }
            return order;
            
        }
        // DELETE: api/Orders/5
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteOrder([FromBody] int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
