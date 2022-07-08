using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactsAPI.Models;
using ContactsAPI.Services;


namespace ContactsAPI.Controllers
{
    /// <summary>
    /// Skill Controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class SkillsController : ControllerBase
    {

        private readonly ILogger<SkillsController> _logger;
        private readonly ContactsDBContext _context;

        /// <summary>
        /// Contact Controller Constructor. Initialize logger and context
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public SkillsController(ILogger<SkillsController> logger, ContactsDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Get all skills List, with related contacts
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var skills = await _context.Skills!.Include(skill => skill.Contacts).ToListAsync();

                if (skills != null)
                    return Ok(skills);
                else
                    return NoContent();

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + e.InnerException?.Message);
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Get all skills List, without related contacts
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllNoChild")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllNoChildAsync()
        {
            try
            {
                var skills = await _context.Skills!.ToListAsync();
                
                if (skills != null)
                    return Ok(skills);
                else
                    return NoContent();

            } catch(Exception e) {
                _logger.LogError(e.Message + e.InnerException?.Message);
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Get one skill details by id 
        /// </summary>
        /// <param name="id">Skill id</param>
        /// <returns></returns>
        [HttpGet("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var skill = await _context.Skills!.Include(skill => skill.Contacts).FirstOrDefaultAsync(s=>s.Id==id);
                //var skill = await _context.Skills!.FindAsync(id);
                if (skill != null)
                    return Ok(skill);
                else
                    return NotFound($"No skill with id {id} was found.");
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message} {e.InnerException?.Message}");
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Add a new skill.  
        /// </summary>
        /// <param name="s">Skill</param>
        /// <returns></returns>
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAsync(SkillNoChild s)
        {
            try
            {
                //checking first if a skill with same name and lefel already exists
                var existingSkill = await _context.Skills!.FirstOrDefaultAsync(skill => skill.Name == s.Name & skill.Level == s.Level);
                if (existingSkill is Skill)
                    return BadRequest($"A skill with same name and level alerrady exists: {existingSkill!.Id}.");

                Skill newSkill = new Skill() { Name = s.Name, Level = s.Level };
                await _context.Skills!.AddAsync(newSkill);
                await _context.SaveChangesAsync();
                return Ok(new SkillNoChild { Id = newSkill.Id, Name = newSkill.Name, Level = newSkill.Level });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + e.InnerException?.Message);
                return Problem(e.Message);
            }

        }

        /// <summary>
        /// Add a new skill with optional new contacts.  
        /// </summary>
        /// <param name="s">Skill</param>
        /// <returns></returns>
        [HttpPost("AddWithContacts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddWithContactAsync(Skill s)
        {
            try
            {
                //checking first if a skill with same name and lefel already exists
                var existingSkill = await _context.Skills!.FirstOrDefaultAsync(skill => skill.Name == s.Name & skill.Level == s.Level);
                if (existingSkill is Skill)
                    return BadRequest($"A skill with same name and level alerrady exists: {existingSkill!.Id}.");

                await _context.Skills!.AddAsync(s);
                await _context.SaveChangesAsync();
                return Ok(s);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + e.InnerException?.Message);
                return Problem(e.Message);
            }

        }

        /// <summary>
        /// Update an existing skill
        /// </summary>
        /// <param name="s">Skill</param>
        /// <returns></returns>
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(Skill s)
        {
            try
            {
                var skill = await _context.Skills!.FindAsync(s.Id);
                if (skill != null)
                {
                    skill.UpdateFrom(s);
                }
                else
                    return NotFound($"Skill not found.");

                await _context.SaveChangesAsync();
                return Ok(s);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + e.InnerException?.Message);
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Delete a skill by id
        /// </summary>
        /// <param name="id">Skill id</param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteByID(int id)
        {
            try
            {
                var product = await _context.Skills!.FindAsync(id);
                if (product != null)
                    _context.Skills.Remove(product);
                else
                    return NotFound($"Skill with {id} not found.");
                await _context.SaveChangesAsync();
                return Ok($"Contact with {id} removed.");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + e.InnerException?.Message);
                return Problem(e.Message);
            }
        }
    }

}
