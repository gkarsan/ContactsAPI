using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ContactsAPI.Models
    {

    /// <summary>
    /// Data model for table Contacts
    /// </summary>
    [Table("Contacts")] //, Schema="api"
    public class Contact
    {
        public Contact()
        {
            this.Skills = new HashSet<Skill>();
        }

        public Contact(ContactNoChild c)
        {
            Id = c.Id;
            Firstname = c.Firstname;
            Lastname = c.Lastname;
            Fullname = c.Fullname;
            Adderss = c.Adderss;
            Email = c.Email;
            MobilePhoneNumber = c.MobilePhoneNumber;
            this.Skills = new HashSet<Skill>();
        }

        [Key]
        [Column("contactId")]
        public int Id { get; set; }
        [Required, MinLength(1, ErrorMessage = "Field cannot be empty."), MaxLength(255)]

        public string Firstname { get; set; } = "";
        [Required, MinLength(1, ErrorMessage = "Field cannot be empty."), MaxLength(255)]
        public string Lastname { get; set; } = "";
        public string Fullname { get; set; } = "";
        public string Adderss { get; set; } = "";
        public string Email { get; set; } = "";
        public string MobilePhoneNumber { get; set; } = "";
        public virtual ICollection<Skill> Skills { get; set; }
        public bool UpdateFrom(ContactNoChild c)
        {
            this.Id = c.Id;
            if (c.Firstname != null)
                this.Firstname = c.Firstname;
            if (c.Lastname != null)
                this.Lastname = c.Lastname;
            if (c.Lastname != null)
                this.Fullname = c.Fullname;
            if (c.Adderss != null)
                this.Adderss = c.Adderss;
            if (c.Email != null)
                this.Email = c.Email;
            if (c.MobilePhoneNumber != null)
                this.MobilePhoneNumber = c.MobilePhoneNumber;

            return true;
        }

    }

    /// <summary>
    /// Model for a contact without the associated links, not used for database model, but in some api input
    /// </summary>
    // TODO use inheritence
    [NotMapped]
    public class ContactNoChild
    {
        public ContactNoChild() { }
        public ContactNoChild(int id, string firstname, string lastname, string fullname, string adderss, string email, string mobilePhoneNumber)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Fullname = fullname;
            Adderss = adderss;
            Email = email;
            MobilePhoneNumber = mobilePhoneNumber;
        }

        public int Id { get; set; }
        public string Firstname { get; set; } = "";
        public string Lastname { get; set; } = "";
        public string Fullname { get; set; } = "";
        public string Adderss { get; set; } = "";
        public string Email { get; set; } = "";
        public string MobilePhoneNumber { get; set; } = "";

        public Contact toContact()
        {
            return new Contact { Id = this.Id, Firstname = this.Firstname, Lastname = this.Lastname, Fullname = this.Fullname, Adderss = this.Adderss, Email = this.Email, MobilePhoneNumber = this.MobilePhoneNumber };
        }
    }

    /// <summary>
    /// Represent a link between a contact and a skill. Contains only Ids
    /// </summary>
    [NotMapped]
    public class ContactSkilsIds
    {
        public int ContactId { get; set; }
        public int SkillId { get; set; }

    }

    /// <summary>
    /// Model fo skills
    /// </summary>
    [Table("Skills")] //, Schema="api"
    public class Skill
    {
        /// <summary>
        /// Creator
        /// </summary>
        public Skill()
        {
            this.Contacts = new HashSet<Contact>();
        }
        [Key]
        [Column("SkillId")]
        public int Id { get; set; }
        [Required, MinLength(1, ErrorMessage = "Field cannot be empty."), MaxLength(255)]
        public string Name { get; set; } = "";
        [Required]
        public int Level { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }

        /// <summary>
        /// Update skill with another skill values (except the id) 
        /// </summary>
        /// <param name="s"></param>
        /// <returns>bool</returns>
        public bool UpdateFrom(Skill s)
        {
            if (s.Name != null)
                this.Name = s.Name;
            this.Level = s.Level;
            return true;
        }
    }

    /// <summary>
    /// Model for a skill without related clients
    /// </summary>
    [NotMapped]
    public class SkillNoChild
    {
        [Key]
        [Column("SkillId")]
        public int Id { get; set; }
        [Required, MinLength(1, ErrorMessage = "Field cannot be empty."), MaxLength(255)]
        public string Name { get; set; } = "";
        [Required]
        public int Level { get; set; }

    }
}

