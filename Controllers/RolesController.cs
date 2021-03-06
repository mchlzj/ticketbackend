using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticketsystem_backend.Data;
using ticketsystem_backend.Models;

namespace ticketsystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly TicketSystemDbContext _context;

        public RolesController(TicketSystemDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns list of all roles
        /// </summary>
        /// <returns></returns>
        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        /// <summary>
        /// Returns a role according to the send roleId
        /// </summary>
        /// <param name="id">roleId</param>
        /// <returns></returns>
        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        //// PUT: api/Roles/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutRole(int id, Role role)
        //{
        //    if (id != role.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(role).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!RoleExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        /// <summary>
        /// Create new role
        /// </summary>
        /// <param name="role">Role-Model including Name</param>
        /// <returns></returns>
        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(Role role)
        {
            // add new role to database
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            // return new role
            return CreatedAtAction("GetRole", new { id = role.Id }, role);
        }

        //// DELETE: api/Roles/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteRole(int id)
        //{
        //    var role = await _context.Roles.FindAsync(id);
        //    if (role == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Roles.Remove(role);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        /// <summary>
        /// Returns true if Role exist
        /// </summary>
        /// <param name="id">RoleId</param>
        /// <returns></returns>
        //private bool RoleExists(int id)
        //{
        //    return _context.Roles.Any(e => e.Id == id);
        //}
    }
}
