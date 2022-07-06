using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactsAPI.Models;
using ContactsAPI.Services;

namespace ContactsAPI.Controllers
{
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly ContactsDBContext _context;

        public ContactsController(ILogger<ContactsController> logger, ContactsDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAsync()
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
                _logger.LogError(e.Message);
                return Problem(e.Message);

            }
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetByID(int id)
        {
            try
            {
                var foundModel = await _context.Contacts!.Include(contact => contact.Skills).FirstOrDefaultAsync(c => c.Id == id);
                return foundModel
                    is Contact
                    ? Ok(foundModel)
                    : NotFound(id);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Problem(e.Message);

            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddContact([FromBody] Contact c)
        {
            try
            {
                await _context.Contacts!.AddAsync(c);
                await _context.SaveChangesAsync();
                return Ok(c);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message + ex.InnerException?.Message);
            }

        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] Contact c)
        {
            try
            {
                var skill = await _context.Contacts!.FindAsync(c.Id); //TO ad include Skills
                if (skill != null)
                {
                    skill.UpdateFrom(c);
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
            var product = await _context.Contacts!.FindAsync(id);
            if (product != null)
                _context.Contacts.Remove(product);
            else
                return NotFound();
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
