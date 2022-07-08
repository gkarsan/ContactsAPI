using ContactsAPI.Controllers;
using ContactsAPI.Models;
using ContactsAPI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsAPIxUnitTest
{
    public class DBContextTests
    {

        private ContactsDBContext? dbContext;
        private ILogger<ContactsController>? ilog;

        public DBContextTests()
        {
            Console.WriteLine("Inside SetUp Constructor");

        }

        public void Dispose()
        {
            Console.WriteLine("Inside CleanUp or Dispose method");
        }

        [Fact]
        public void TestDBContext()
        {
            var Utils = new Utils();
            var app = Utils.PrepareApp();
            app.RunAsync();

            dbContext = app!.Services.GetService<ContactsDBContext>();
            LoggerFactory lfactoy = new LoggerFactory();
            ilog = new Logger<ContactsController>(lfactoy);


            Assert.NotNull(dbContext);
            Assert.NotNull(dbContext.Contacts);
            Assert.NotNull(dbContext.Skills);
            //Adding a contact in database
            dbContext.Contacts!.AddAsync(new Contact
            {
                Firstname = "UnitTest",
                Lastname = "TestUser",
                Adderss = "TestAddress",
                Email = "",
                Fullname = "Unit Test",
                MobilePhoneNumber = "0",
                Skills = new List<Skill> { new Skill { Name = "UnitTesting", Level = 0 } }
            });

        }



    }
}
