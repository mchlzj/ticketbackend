﻿using System;
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
    public class ModulesController : ControllerBase
    {
        private readonly TicketSystemDbContext _context;

        public ModulesController(TicketSystemDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// returns a list of all Modules
        /// </summary>
        /// <returns></returns>
        // GET: api/Modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Module>>> GetModules()
        {
            return await _context.Modules
                .Include(m => m.Responsible)
                .ToListAsync();
        }

        /// <summary>
        /// Returns a Module according to the send ModuleId
        /// </summary>
        /// <param name="id">ModuleId</param>
        /// <returns></returns>
        // GET: api/Modules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Module>> GetModule(int id)
        {
            var @module = await _context.Modules.Where(m => m.Id == id)
                .Include(m => m.Responsible)
                .FirstOrDefaultAsync();

            if (@module == null)
            {
                return NotFound();
            }

            return @module;
        }

        //// PUT: api/Modules/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutModule(int id, Module @module)
        //{
        //    if (id != @module.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(@module).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ModuleExists(id))
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
        /// Creates a new Course/Module
        /// </summary>
        /// <param name="moduleVM">Module-Model including Name and ResponsibleUserId</param>
        /// <returns></returns>
        // POST: api/Modules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Module>> PostModule(CreateModuleVM moduleVM)
        {
            // Get ResponsibleUser
            User responsible = _context.Users.Where(u => u.Id == moduleVM.ResponsibleUserId).FirstOrDefault();

            // Create new Module
            Module module = new Module
            {
                Name = moduleVM.Name,
                Responsible = responsible
            };

            // Add Module to Database
            _context.Modules.Add(module);
            await _context.SaveChangesAsync();

            // Return new Module
            return CreatedAtAction("GetModule", new { id = module.Id }, module);
        }

        //// DELETE: api/Modules/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteModule(int id)
        //{
        //    var @module = await _context.Modules.FindAsync(id);
        //    if (@module == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Modules.Remove(@module);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        /// <summary>
        /// Returns true if Module exist
        /// </summary>
        /// <param name="id">ModuleId</param>
        /// <returns></returns>
        private bool ModuleExists(int id)
        {
            return _context.Modules.Any(e => e.Id == id);
        }
    }
}
