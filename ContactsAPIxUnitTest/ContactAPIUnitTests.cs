using ContactsAPI.Controllers;
using ContactsAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using ContactsAPI.Models;
namespace ContactsAPIxUnitTest
{
    public class ContactAPIUnitTests : IDisposable
    {
        private ContactsDBContext? dbContext;
        private ILogger<ContactsController>? ilog;

        public ContactAPIUnitTests()
        {
            Console.WriteLine("Inside SetUp Constructor");


        }

        public void Dispose()
        {
            Console.WriteLine("Inside CleanUp or Dispose method");
        }

        [Fact]
        public async Task TestContactController()
        {
            var Utils = new Utils();
            var app = Utils.PrepareApp();
            app.RunAsync();


            dbContext = app!.Services.GetService<ContactsDBContext>();
            LoggerFactory lfactoy = new LoggerFactory();
            ilog = new Logger<ContactsController>(lfactoy);


            Assert.NotNull(ilog);

            ContactsController controller = new ContactsController(ilog, dbContext);
            var response = await controller.GetAllAsync();
            Assert.True(response is IActionResult);
            Assert.True(response is ObjectResult);
            ObjectResult objectResult = (ObjectResult)response;
            Assert.True(objectResult.Value is List<Contact>);

            List<Contact>? l = (List<Contact>?)objectResult.Value;
            Assert.NotNull(l);
            Assert.True(l.Count > 0);

            //Contact c = new Contact { Firstname = "Test", Lastname = "User", Adderss = "TestAddress", Email = "", Fullname = "Unit Test", MobilePhoneNumber = "0", Skills = new List<Skill> { new Skill { Name = "UnitTesting", Level = 0 } } };
            ContactNoChild inContact = new ContactNoChild { Firstname = "Test", Lastname = "User", Adderss = "TestAddress", Email = "", Fullname = "Unit Test", MobilePhoneNumber = "0" };
            response = await controller.AddAsync(inContact);
            Assert.True(response is IActionResult);
            Assert.True(response is ObjectResult);
            objectResult = (ObjectResult)response;
            Assert.True(objectResult.Value is Contact);
            Contact? outContact = (Contact)objectResult.Value;
            Assert.True(inContact.Firstname == outContact.Firstname,$"in:{inContact.Firstname}, out:{outContact.Firstname}");
            Assert.NotEqual(inContact.Id, outContact.Id);

        }

    }
}