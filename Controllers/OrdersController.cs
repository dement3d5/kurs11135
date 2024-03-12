﻿using System;
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
        private readonly user1Context _context;
        private int id4put;

        public OrdersController(user1Context context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpPost("get")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.AsNoTracking()
            .Include(s => s.Status)
            .Include(s => s.User)
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .ToListAsync();


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

        // GET: api/Orders/get/{date}
        [HttpGet("get/{date}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByDate(string date)
        {
            DateTime parsedDate;
            if (!DateTime.TryParse(date, out parsedDate))
            {
                return BadRequest("Invalid date format");
            }

            var orders = await _context.Orders
                .Where(o => o.CreateAt.HasValue && o.CreateAt.Value.Date == parsedDate.Date)
                .ToListAsync();

            return orders;
        }


        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("put")]
        public async Task<IActionResult> PutOrder([FromBody] Order order)
        {
            id4put = order.Id;
            if (id4put != order.Id)
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
                if (!OrderExists(id4put))
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
                return Problem("Entity set 'user1Context.Orders'  is null.");
            }
            foreach (var orderProduct in order.OrderProducts)
            {
                _context.Entry(orderProduct).State = EntityState.Added;
            }

            /*_context.Entry(order.Status).;
            _context.Entry(order.User).State = EntityState.Unchanged;
            _context.Entry(order.Product).State = EntityState.Unchanged;*/
            order.Status = _context.OrderStatuses.FirstOrDefault(s => s.Id == order.StatusId);
            order.User = _context.Users.FirstOrDefault(s => s.Id == order.UserId);
            //order.Product = _context.Products.FirstOrDefault(s => s.Id == order.ProductId);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            if (order == null)
            {
                return NoContent();
            }
            return CreatedAtAction("GetOrder", new { id = order.Id }, order);

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