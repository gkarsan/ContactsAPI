using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactsAPI.Models;
using ContactsAPI.Services;

namespace MyRestApiVS22.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkillsController : ControllerBase
    {

        private readonly ILogger<SkillsController> _logger;
        private readonly ContactsDBContext _context;

        public SkillsController(ILogger<SkillsController> logger, ContactsDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAsync()
        {
            _logger.Log(LogLevel.Debug, "Received GetAll request");
            _context.Database.EnsureCreated();
            var skills = await _context.Skills!.ToListAsync();
            if (skills != null)
                return Ok(skills);
            else
                return NoContent();
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetByID(int id)
        {
            _context.Database.EnsureCreated();
            var skill = await _context.Skills!.FindAsync(id);
            if (skill != null)
                return Ok(skill);
            else
                return NotFound();
        }
        [HttpPost("Add")]
        public async Task<IActionResult> AddPoduct(Skill s)
        {
            try
            {
                await _context.Skills!.AddAsync(s);
                await _context.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message + ex.InnerException?.Message);
            }

        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdatePoduct(Skill s)
        {
            try
            {
                var skill = await _context.Skills!.FindAsync(s.Id);
                if (skill != null)
                {
                    skill.UpdateFrom(s);
                }

                //context.Products.Update(p);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message + ex.InnerException?.Message);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteByID(int id)
        {
            var product = await _context.Skills!.FindAsync(id);
            if (product != null)
                _context.Skills.Remove(product);
            else
                return NotFound();
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
