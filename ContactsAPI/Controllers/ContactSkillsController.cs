using Microsoft.AspNetCore.Mvc;
using ContactsAPI.Services;
using ContactsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{
    /// <summary>
    /// Contacts-Skills Controller endpoints
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ContactSkillsController : ControllerBase
    {
        private readonly ILogger<ContactSkillsController> _logger;
        private readonly ContactsDBContext _context;
        /// <summary>
        /// Contoler creator. Initialize logger and context
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public ContactSkillsController(ILogger<ContactSkillsController> logger, ContactsDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Get list of contacts-skills relations. Return links between Contacts and Skills
        /// </summary>
        /// <returns>List of {contactID,skillId}</returns>
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var contactWithSkills = await _context.Contacts!
                    .Include(contact => contact.Skills)
                    .Where(c => c.Skills.Count > 0).ToListAsync();
                if (contactWithSkills is null)
                    return NoContent();


                List<ContactSkilsIds> l=new List<ContactSkilsIds>();
                foreach (var contact in contactWithSkills)
                    foreach (var skils in contact.Skills)
                        l.Add(new ContactSkilsIds
                        {
                            ContactId = contact.Id,
                            SkillId = skils.Id
                        });
                        
                if (l != null)
                    return Ok(l);
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
        /// link a contact with a skill
        /// </summary>
        /// <param name="idContact">Contact Id</param>
        /// <param name="idSkill">Skill Id</param>
        /// <returns></returns>
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAsync(int idContact, int idSkill)
        {
            try {
                var skill = await _context.Skills!.FindAsync(idSkill);
                if (!(skill is Skill))
                    return Problem(detail: $"Skill {idSkill} not found.", statusCode: StatusCodes.Status404NotFound);

                var contact = await _context.Contacts!.FindAsync(idContact);

                if (!(contact is Contact))
                    return Problem(detail: $"Contact {idContact} not found.", statusCode: StatusCodes.Status404NotFound);
            
                contact.Skills.Add(skill);
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
        /// link a contact with a skill (providing a new skill details)
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="skill"></param>
        /// <returns></returns>
        [HttpPost("AddWithSkill")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddSkillToUser(int idUser, [FromBody] Skill skill)
        {
            try
            {
                if (!(skill is Skill))
                    return Problem(detail: "Skill not found.", statusCode: StatusCodes.Status404NotFound);
                var skillExists = await _context.Skills!.FindAsync(skill.Id);

                var contact = await _context.Contacts!.FindAsync(idUser);

                if (!(contact is Contact))
                    return Problem(detail: "Contact not found.", statusCode: StatusCodes.Status404NotFound);

                contact.Skills.Add(skill);
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
        /// Delete a relation between a contact and a skill
        /// </summary>
        /// <param name="idContact">Contact Id</param>
        /// <param name="idSkill">Skill Id</param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteByIdAsync(int idContact,int idSkill)
        {
            try {
                var contact = await _context.Contacts!.FindAsync(idContact);
                var foundContact = await _context.Contacts!.Include(contact => contact.Skills).FirstOrDefaultAsync(c => c.Id == idContact);
                if (contact != null)
                {
                    var skillToRemove = contact.Skills!.FirstOrDefault(c => c.Id == idSkill);
                    if(skillToRemove!=null)
                        contact.Skills!.Remove(skillToRemove);
                }
                else
                    return NotFound();
                await _context.SaveChangesAsync();
                return NoContent();
            } catch(Exception e)
            {
                _logger.LogError(e.Message + e.InnerException?.Message);
                return Problem(e.Message);
            }

        }

    }
}
