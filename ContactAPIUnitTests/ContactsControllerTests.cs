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

namespace ContactAPIUnitTests
{
    public class Tests
    {
        private WebApplication? app ;

        [SetUp]
        public void Setup()
        {
            //WE mimick the initialization done in Statup.cs
            string[] args = new string[] { } ;
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
            //NOTE: run async!
            
        }

        [Test]
        public async Task Test1Async()
        {

            ContactsDBContext? db= app!.Services.GetService<ContactsDBContext>();
            LoggerFactory lfactoy = new LoggerFactory();

            ILogger<ContactsController> ilog = new Logger<ContactsController>(lfactoy); 
            ContactsController controller = new ContactsController(ilog, db);
            var result = await controller.GetAllAsync();
            Assert.That(result is IActionResult);
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            ObjectResult objectResult = (ObjectResult)result;
            Assert.That(objectResult.Value, Is.InstanceOf <List<Contact>>());


            //Contact c = new Contact { Firstname = "UnitTest", Lastname = "TestUser", Adderss = "TestAddress", Email = "", Fullname = "Unit Test", MobilePhoneNumber = "0", Skills = new List<Skill> { new Skill { Name = "UnitTesting", Level = 0 } } };
            ContactNoChild c = new ContactNoChild { Firstname = "UnitTest", Lastname = "TestUser", Adderss = "TestAddress", Email = "", Fullname = "Unit Test", MobilePhoneNumber = "0" };
            result = await controller.AddAsync(c);
            Assert.That(result is IActionResult);
        }
    }
}