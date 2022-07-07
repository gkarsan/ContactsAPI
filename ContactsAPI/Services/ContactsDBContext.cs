using ContactsAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace ContactsAPI.Services
{
    /// <summary>
    /// DB Context for the Contacst Challenge API
    /// </summary>
    public class ContactsDBContext : DbContext
    {
        public ContactsDBContext(DbContextOptions<ContactsDBContext> options) : base(options)
        {
            
            Database.EnsureCreated();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            //base.OnModelCreating(modelBuilder);

            /*modelBuilder.Entity<Contact>()
                .HasMany<Skill>(c => c.Skills)
                .WithMany(s => s.Contacts)
                .Map(cs =>
                {
                    cs.MapLeftKey("contactId");
                    cs.MapRightKey("skillId");
                    cs.ToTable("ContactSkill");
                });*/
        }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Skill>? Skills { get; set; }

        public DbSet<Contact>? Contacts { get; set; }
    }
}
