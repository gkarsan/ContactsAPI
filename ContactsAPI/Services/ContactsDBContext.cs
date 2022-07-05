using ContactsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Services
{
    public class ContactsDBContext : DbContext
    {
        public ContactsDBContext(DbContextOptions<ContactsDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Skill>? Skills { get; set; }

        public DbSet<Contact>? Contacts { get; set; }
    }
}
