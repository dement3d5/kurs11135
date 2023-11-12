using System;
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
    public class UserPositionsController : ControllerBase
    {
        private readonly user1Context _context;

        public UserPositionsController(user1Context context)
        {
            _context = context;
        }

        // GET: api/UserPositions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserPosition>>> GetUserPositions()
        {
            return await _context.UserPositions.ToListAsync();
        }

        // GET: api/UserPositions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserPosition>> GetUserPosition(int id)
        {
            var userPosition = await _context.UserPositions.FindAsync(id);

            if (userPosition == null)
            {
                return NotFound();
            }

            return userPosition;
        }

        // PUT: api/UserPositions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserPosition(int id, UserPosition userPosition)
        {
            if (id != userPosition.Id)
            {
                return BadRequest();
            }

            _context.Entry(userPosition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserPositionExists(id))
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

        // POST: api/UserPositions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserPosition>> PostUserPosition(UserPosition userPosition)
        {
            _context.UserPositions.Add(userPosition);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserPosition", new { id = userPosition.Id }, userPosition);
        }

        // DELETE: api/UserPositions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserPosition(int id)
        {
            var userPosition = await _context.UserPositions.FindAsync(id);
            if (userPosition == null)
            {
                return NotFound();
            }

            _context.UserPositions.Remove(userPosition);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserPositionExists(int id)
        {
            return _context.UserPositions.Any(e => e.Id == id);
        }
    }
}
