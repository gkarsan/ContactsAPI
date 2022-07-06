using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactsAPI.Models;
using ContactsAPI.Services;


namespace ContactsAPI.Controllers
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
        /// <summary>
        /// EndPoint to get all Skills
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAsync()
        {   try
            {
                var skills = await _context.Skills!.ToListAsync();
                
                if (skills != null)
                    return Ok(skills);
                else
                    return NoContent();

            } catch(Exception e) {
                _logger.LogError(e.Message);
                return Problem(e.Message);
            }
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetByID(int id)
        {
            var skill = await _context.Skills!.FindAsync(id);
            if (skill != null)
                return Ok(skill);
            else
                return NotFound(id);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddSkill(Skill s)
        {
            try
            {
                await _context.Skills!.AddAsync(s);
                await _context.SaveChangesAsync();
                return Ok(s);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message + ex.InnerException?.Message);
            }

        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateSkill(Skill s)
        {
            try
            {
                var skill = await _context.Skills!.FindAsync(s.Id);
                if (skill != null)
                {
                    skill.UpdateFrom(s);
                }

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
