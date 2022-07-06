using Microsoft.AspNetCore.Mvc;
using ContactsAPI.Services;
using ContactsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{
    public class ContactSkillsController : Controller
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
                _logger.LogError(e.Message);
                return Problem(e.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> AddSkillToUser(int idUser, int idSkill)
        {
            var skill = await _context.Skills!.FindAsync(idSkill);
            if (!(skill is Skill))
                return Problem(detail: "Skill not found.", statusCode: StatusCodes.Status404NotFound);

            var contact = await _context.Contacts!.FindAsync(idUser);

            if (!(contact is Contact))
                return Problem(detail: "Contact not found.", statusCode: StatusCodes.Status404NotFound);
            
            contact.Skills.Add(skill);
            await _context.SaveChangesAsync();
            
            return contact
                is Contact ? Ok(contact) : NoContent();

        }

        [HttpPut("UpdateWithSkill")]
        public async Task<IActionResult> AddSkillToUser(int idUser, [FromBody] Skill skill)
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
    }
}
