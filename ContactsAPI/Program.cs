using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.EntityFrameworkCore;
using ContactsAPI.Controllers;
using ContactsAPI.Services;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// Add DB Context and associate connectionString name
builder.Services.AddDbContext<ContactsDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));
});

//builder.Services.AddControllers();
// prevent cycling errors due to many to many references
builder.Services.AddControllers().AddJsonOptions(option => {
    option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    //option.JsonSerializerOptions.MaxDepth = 2;
});


builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Addes xml documentation from project XML doc
builder.Services.AddSwaggerGen(c => 
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Contacts API",
            Description = "It’s a simple API, where a user can get a quick overview over all contacts resources like person, skills...",
            Version = "v1",
            Contact = new OpenApiContact
            {
                Name = "Daniel Perret",
                Email = "dapcom@bluewin.ch"
            }
        });
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory,xmlFile);
        c.IncludeXmlComments(xmlPath);
    });


   
    

var app = builder.Build();

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
// app.MapContactEndpoints();

app.Run();
