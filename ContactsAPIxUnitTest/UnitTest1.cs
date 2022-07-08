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
        private WebApplication? app;
        private ContactsDBContext? dbContext;
        private ILogger<ContactsController>? ilog;

        public ContactAPIUnitTests()
        {
            Console.WriteLine("Inside SetUp Constructor");


        }

        private void RunApp()
        {
            //This is same as the initialization done in Statup.cs in the API. not sure how to run it
            string[] args = new string[] { };
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

            // Add DB Context
            builder.Services.AddDbContext<ContactsDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));
            });

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // Test ussing MapEndPoint rather than controller
            //app.MapContactEndpoints();

            app.RunAsync();
        }
        public void Dispose()
        {
            Console.WriteLine("Inside CleanUp or Dispose method");
        }

        [Fact]
        public void TestDBContext()
        {
            RunApp();
            //NOTE: run async!

            dbContext = app!.Services.GetService<ContactsDBContext>();
            LoggerFactory lfactoy = new LoggerFactory();
            ilog = new Logger<ContactsController>(lfactoy);


            Assert.NotNull(dbContext);
            Assert.NotNull(dbContext.Contacts);
            Assert.NotNull(dbContext.Skills);
        }

        [Fact]
        public async Task TestContactController()
        {
            RunApp();

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

            //Contact c = new Contact { Firstname = "UnitTest", Lastname = "TestUser", Adderss = "TestAddress", Email = "", Fullname = "Unit Test", MobilePhoneNumber = "0", Skills = new List<Skill> { new Skill { Name = "UnitTesting", Level = 0 } } };
            ContactNoChild inContact = new ContactNoChild { Firstname = "UnitTest", Lastname = "TestUser", Adderss = "TestAddress", Email = "", Fullname = "Unit Test", MobilePhoneNumber = "0" };
            response = await controller.AddAsync(inContact);
            Assert.True(response is IActionResult);
            Assert.True(response is ObjectResult);
            objectResult = (ObjectResult)response;
            Assert.True(objectResult.Value is Contact);
            Contact? outContact = (Contact)objectResult.Value;
            Assert.True(inContact.Firstname == outContact.Firstname,$"in:{inContact.Firstname}, out:{outContact.Firstname}");
            Assert.NotEqual(inContact.Id, outContact.Id);

            //var c = result.ExecuteResultAsync();
            //Assert.True(response is Contact);
        }

    }
}