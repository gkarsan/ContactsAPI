# ContactsAPI
It’s a simple API, where a user can get a quick overview over all contacts resources
like person, skills...

# Initial Project Setup
The api uses MS SQL Server database. Before running the first time, ensure the connection string in appsettings.json is set up right for the situation.
Currerntly, if running in docker, it can't use (localdb) and need path to the server, even if running on local. If running in a windows system with local MS SQL Server or Express, it can work with locladb

# API Content
It has 3 controllers: Contacts, Skills and ContactSkills
Entry points
	GET /Contacts/GetAll : Get all contacts (including the corresponding skills)
	GET /Contacts/GetAllNoChild : Get all contacts (without the corresponding skills)
	GET /Contacts/GetById{id} : Get one contact (including it's skills)
	POST /Contacts/Add : Add a contact (in body, it can contain initial skill list)
	POST /Contacts/AddNoChild : Add a contact (in body, it cannot contain initial skill list)
	PUT /Contacts/Update : Update a contact (wip should skill by included, if so how)
	DELETE /Contacts/Delete{id} : delete a contact (and any reference to skills, but not skills (check))
	
	GET /Skills/GetAll : Get all skills (including the corresponding contacts?)
	GET /Skills/GetAllNoChild : Get all skills (including the corresponding contacts?)
	GET /Skills/GetById{id} : Get one contact (including it's associated contacts?)
	POST /Skills/Add : Add a skill(in body, it can contain initial contact list)
	POST /Skills/AddNoChild : Add a skill(in body, it cannot contain initial contact list ???)
	PUT /Skills/AddWithSkill : update a skill (wip should skill by included, if so how)
	DELETE /Skills/Delete{id} : delete a skill (and any reference to contacts, but not contacts (check))
	
	GET /ContactSkills/GetAll : Get all skills (including the corresponding contacts)
	GET /ContactSkills/GetById{id} : Get one contact (without it's associated contacts)
	POST /ContactSkills/Update{idContact,idSkill} : add a skill id to a contact id
	POST /ContactSkills/UpdateWithSkill{id} : add a skill to a contact id (skill detail in body)
	DELETE /ContactSkills/Delete{id} : delete a skill to contact relation (but not relatd contacts or skills)
		
# Sample data
Add Contact (without skill)
{
	"id":0,
	"firstname":"John",
	..
}
Add Contact with skills
{
}
		
# Notes
Developped with Visual Studio 22
.Net Core 6
Entity Famework 6
SwaggerDoc, custom documentation added, including XMLComments fom code
the Solution contains a xUnit test projects, not complete (having issues with app getting disposed too soon for succesive tests...
authorisation/authentication:MS Identity included but not enabled. !May cause issue as linked with my account..
the project contain a query (queryall.sql) usefull to interogate all contacts with linked skills in diectly in database
