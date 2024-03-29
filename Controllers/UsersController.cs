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
    public class UsersController : ControllerBase
    {
        private readonly user1Context _context;
        private int id4put;

        public UsersController(user1Context context)
        {
            _context = context;
        }
        // GET: api/Users
        [HttpPost("get")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("put")]
        public async Task<IActionResult> PutPUser([FromBody] User user)
        {
            id4put = user.Id;
            if (id4put != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id4put))
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

        [HttpPost("Auth")]
        public async Task<ActionResult<User>> AuthUser([FromBody] User user)
        {
            var users = await _context.Users.FirstOrDefaultAsync(s => s.Login == user.Login && s.Password == user.Password);

            return users ?? new User();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody]User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }
        [HttpPost("SaveUser")]
        public async Task<ActionResult<User>> PostUsers([FromBody] User user)
        {
            if (_context.Users == null)
            {
                return Problem("");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        [HttpPost("CheckExistingUser")]
        public async Task<ActionResult<bool>> CheckExistingUser([FromBody] string login)
        {
            var existingUser = await _context.Users.AnyAsync(u => u.Login == login);
            return existingUser;
        }

        [HttpPost("CheckExistingPhone")]
        public async Task<ActionResult<bool>> CheckExistingPhone([FromBody] string phone)
        {
            var existingPhone = await _context.Users.AnyAsync(u => u.Organization == phone);
            return existingPhone;
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
