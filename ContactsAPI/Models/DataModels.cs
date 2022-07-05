using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ContactsAPI.Models
    {
    [Table("Contacts", Schema = "dbo")] //, Schema="api"
    public class Contact
    {
        public Contact()
        {
            this.Skills = new HashSet<Skill>();
        }

        [Key]
        [Column("contactId")]
        public int Id { get; set; }
        [Required, MinLength(1, ErrorMessage = "Field cannot be empty."), MaxLength(255)]
        public string Firstname { get; set; } = "";
        [Required]
        public string Lastname { get; set; } = "";
        [Required]
        public string Fullname { get; set; } = "";
        [Required]
        public string Adderss { get; set; } = "";
        [Required]
        public string Email { get; set; } = "";
        [Required]
        public string MobilePhoneNumber { get; set; } = "";
        public virtual ICollection<Skill> Skills { get; set; }
        public bool UpdateFrom(Contact c)
        {

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
            if (c.Skills != null)
                this.Skills = c.Skills; //TODO

            return true;
        }

    }

    public class Skill
    {
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
        public bool UpdateFrom(Skill s)
        {

            if (s.Name != null)
                this.Name = s.Name;

            //if (s.Level != null)
            this.Level = s.Level;

            return true;
        }
    }
}

