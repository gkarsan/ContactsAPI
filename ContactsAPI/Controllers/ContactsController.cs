using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactsAPI.Models;
using ContactsAPI.Services;

namespace ContactsAPI.Controllers
{
    /// <summary>
    /// Contacts Controller endpoints
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly ContactsDBContext _context;

        /// <summary>
        /// Contact Controller Constructor. Initialize logger and dbcontext
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public ContactsController(ILogger<ContactsController> logger, ContactsDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Get Contacts List, including skills
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
                var list = await _context.Contacts!.Include(contact => contact.Skills).ToListAsync();

                if (list != null)
                    return Ok(list);
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
        /// Get all contacts without skills
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
                var list = await _context.Contacts!.ToListAsync();
                
                if (list != null)
                    return Ok(list);
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
        /// Get one contact details by id (including skills)
        /// </summary>
        /// <param name="id">Contact Id</param>
        /// <returns></returns>
        [HttpGet("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var foundContact = await _context.Contacts!.Include(contact => contact.Skills).FirstOrDefaultAsync(c => c.Id == id);
                return foundContact
                    is Contact
                    ? Ok(foundContact)
                    : NotFound($"No contact with id {id} was found.");

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Problem(e.Message);

            }
        }

        /// <summary>
        /// Add a new contact. 
        /// </summary>
        /// <param name="c">Contact</param>
        /// <returns></returns>
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAsync([FromBody] ContactNoChild c)
        {
            try
            {
                var existingContact = await _context.Contacts!.Include(contact => contact.Skills)
                    .FirstOrDefaultAsync(contact => contact.Firstname == c.Firstname & contact.Lastname== c.Lastname );
                if (existingContact is Contact) 
                   return BadRequest($"A contact same name alerrady exists: {existingContact!.Id}.");


                //we retrieve the updated contact so we can return it with it's id updated
                Contact contact = c.toContact();

                await _context.Contacts!.AddAsync(contact);
                await _context.SaveChangesAsync();
                return Ok(contact);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + e.InnerException?.Message);
                return Problem(e.Message);
            }

        }

        /// <summary>
        /// Add a new contact. Skills can also be included. 
        /// </summary>
        /// <param name="c">Contact</param>
        /// <returns></returns>
        [HttpPost("AddWithSkills")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddWithSkillsAsync([FromBody] Contact c)
        {
            try
            {
                // if same skill and level exists use existing skill id instead of creating one
                /*foreach(Skill skill in c.Skills)
                {
                    // Include(skill => skill.Contacts).
                    var existingskill = await _context.Skills!.FirstOrDefaultAsync(s=>s.Name == skill.Name & s.Level==skill.Level) ;
                    if (existingskill is Skill)
                        skill = existingskill;

                }*/
                await _context.Contacts!.AddAsync(c);
                await _context.SaveChangesAsync();
                return Ok(c);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + e.InnerException?.Message);
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Update an existing contact info 
        /// </summary>
        /// <param name="c">Contact</param>
        /// <returns></returns>
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync([FromBody] ContactNoChild c)
        {
            try
            {
                var contact = await _context.Contacts!.FindAsync(c.Id); //TO ad include Skills
                if (contact != null)
                {
                    contact.UpdateFrom(c);
                }
                else
                    return NotFound($"Contact with {c.Id} not found.");

                //context.Products.Update(p);
                await _context.SaveChangesAsync();
                return Ok(c);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + e.InnerException?.Message);
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Delete a contact by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            try
            {
                var contact = await _context.Contacts!.FindAsync(id);
                if (contact != null)
                    _context.Contacts.Remove(contact);
                else
                    return NotFound($"Contact with {id} not found.");
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
