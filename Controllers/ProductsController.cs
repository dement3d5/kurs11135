﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kurs1135.Models;
using Kurs1135.DB;

namespace Kurs1135.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly user1Context _context;
        private int id4put;

        public ProductsController(user1Context context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpPost("get")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {

            return await _context.Products.Include("Image")
            .Include(s => s.Category).ToListAsync();
              

        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("put")]
        public async Task<IActionResult> PutProduct([FromBody]Product product)
        {
            id4put = product.Id;
            if (id4put != product.Id)
            {
                return BadRequest();
            }
            
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id4put))
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


        [HttpPost("update")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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




        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("SaveProduct")]
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
            if(_context.Products == null)
            {
                return NotFound();
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            if(product == null)
            {
                return NoContent(); 
            }    
            return product;
            //return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteProduct([FromBody]int id)
        {
            try
            {
                var orderProducts = await _context.OrderProducts.Where(op => op.ProductId == id).ToListAsync();
                _context.OrderProducts.RemoveRange(orderProducts);
                await _context.SaveChangesAsync();

                var productToRemove = await _context.Products.FindAsync(id);
                if (productToRemove != null)
                {
                    _context.Products.Remove(productToRemove);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { message = "Товар успешно удален" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { message = "Произошла ошибка при удалении товара" });
            }
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
