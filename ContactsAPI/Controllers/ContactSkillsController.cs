using Microsoft.AspNetCore.Mvc;
using ContactsAPI.Services;
using ContactsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{
    [Route("[controller]")]
    public class ContactSkillsController : ControllerBase
    {
        private readonly ILogger<ContactSkillsController> _logger;
        private readonly ContactsDBContext _context;

        public ContactSkillsController(ILogger<ContactSkillsController> logger, ContactsDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var list = _context.Contacts!.Include(contact => contact.Skills).Where(c => c.Skills.Count>0);
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

        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync(int idUser, int idSkill)
        {
            try {
                var skill = await _context.Skills!.FindAsync(idSkill);
                if (!(skill is Skill))
                    return Problem(detail: $"Skill {idSkill} not found.", statusCode: StatusCodes.Status404NotFound);

                var contact = await _context.Contacts!.FindAsync(idUser);

                if (!(contact is Contact))
                    return Problem(detail: "Contact not found.", statusCode: StatusCodes.Status404NotFound);
            
                contact.Skills.Add(skill);
                await _context.SaveChangesAsync();
            
                return contact
                    is Contact ? Ok(contact) : NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + e.InnerException?.Message);
                return Problem(e.Message);
            }
        }

        [HttpPost("AddWithSkill")]
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

                return contact
                    is Contact ? Ok(contact) : NoContent();

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + e.InnerException?.Message);
                return Problem(e.Message);
            }
        }

        [HttpDelete("Delete")]
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
