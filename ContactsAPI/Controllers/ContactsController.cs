using Microsoft.EntityFrameworkCore;
using ContactsAPI.Models;
using ContactsAPI.Services;

namespace ContactsAPI.Controllers
{
    public static class ContactEndpointsClass
    {
        private const string routeRoot= "/api/Contact";
        public static void MapContactEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/Contact", async (ContactsDBContext db) =>
            {
                return await db.Contacts!.ToListAsync();
            })
            .WithName("GetAllContacts")
            .Produces<List<Contact>>(StatusCodes.Status200OK);

            routes.MapGet(routeRoot+"/{id}", async (int Id, ContactsDBContext db) =>
            {
                return await db.Contacts!.FindAsync(Id)
                    is Contact model
                        ? Results.Ok(model)
                        : Results.NotFound();
            })
            .WithName("GetContactById")
            .Produces<Contact>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            routes.MapPut(routeRoot + "/{id}", async (int Id, Contact contact, ContactsDBContext db) =>
            {
                var foundModel = await db.Contacts!.FindAsync(Id);

                if (foundModel is null)
                {
                    return Results.NotFound();
                }
                //update model properties here
                foundModel.UpdateFrom(contact);

                await db.SaveChangesAsync();

                return Results.NoContent();
            })
            .WithName("UpdateContact")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status204NoContent);

            routes.MapPost("/Contact", async (Contact contact, ContactsDBContext db) =>
            {
                db.Contacts!.Add(contact);
                await db.SaveChangesAsync();
                return Results.Created($"/Contacts/{contact.Id}", contact);
            })
            .WithName("CreateContact")
            .Produces<Contact>(StatusCodes.Status201Created);

            routes.MapDelete("/Contact/{id}", async (int Id, ContactsDBContext db) =>
            {
                if (await db.Contacts!.FindAsync(Id) is Contact contact)
                {
                    db.Contacts.Remove(contact);
                    await db.SaveChangesAsync();
                    return Results.Ok(contact);
                }

                return Results.NotFound();
            })
            .WithName("DeleteContact")
            .Produces<Contact>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }

}
