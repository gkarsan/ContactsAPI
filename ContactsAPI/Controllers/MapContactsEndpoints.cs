using Microsoft.EntityFrameworkCore;
using ContactsAPI.Models;
using ContactsAPI.Services;

namespace ContactsAPI.Controllers
{
    public static class ContactEndpointsClass
    {
        private const string routeRoot= "/api/Contacts";
        public static void MapContactEndpoints(this IEndpointRouteBuilder routes)
        {
            // Get All
            routes.MapGet(routeRoot, async (ContactsDBContext db) =>
            {
                var list= await db.Contacts!.Include(contact => contact.Skills).ToListAsync();
                
                if (list!=null)
                    return list;
                else
                    return list;
            })
            .WithName("GetAllContacts")
            .Produces<List<Contact>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);

            // Get by ID
            routes.MapGet(routeRoot+"/{Id}", async (int Id, ContactsDBContext db) =>
            {

                var foundModel = await db.Contacts!.Include(contact => contact.Skills).FirstOrDefaultAsync(c => c.Id == Id);
                return foundModel
                    is Contact        
                    ? Results.Ok(foundModel)
                    : Results.NotFound(Id);
            })
            .WithName("GetContactById")
            .Produces<Contact>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            // Update
            routes.MapPut(routeRoot + "/{Id}", async (int Id, Contact contact, ContactsDBContext db) =>
            {
                var foundModel = await db.Contacts!.FindAsync(Id);
                var foundModel2 = await db.Contacts!.Include(contact => contact.Skills).FirstOrDefaultAsync(c => c.Id == Id);

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
            
            // Create Contact
            routes.MapPost(routeRoot, async (Contact contact, ContactsDBContext db) =>
            {
                try
                {
                    db.Contacts!.Add(contact);
                    await db.SaveChangesAsync();
                    return Results.Created($"/Contacts/{contact.Id}", contact);
                } catch(Exception e)
                {
                    return Results.Problem(e.Message);
                }
            })
            .WithName("CreateContact")
            .Produces<Contact>(StatusCodes.Status201Created);

            routes.MapDelete(routeRoot+"/{Id}", async (int Id, ContactsDBContext db) =>
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
