# ContactsAPI
It’s a simple API, where a user can get a quick overview over all contacts resources
like person, skills...

# Initial Project Setup
The api uses MS SQL Server database. 
Before running the first time, ensure the connection string in appsettings.json is set up right for the situation.
Currerntly, if running in docker, it can't use (localdb) and need path to the server, even if running on local. If running in a windows system with local MS SQL Server or Express, it can work with locladb
Database : ContactDB (or as mentioned in the connectionstring in parameter InitialCataglog
it is ceated the first time, so use need to be authorized to create database
Sample connection string if run in IISExperss on local:
```
  "ConnectionStrings": {
    "ConnectionString": "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=ContactDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
  },
```
sample If run in Docker and/or to an extenal sqlexpress server using sql user:
```
  "ConnectionStrings": {
    "ConnectionString": "Server=hostserver\\sqlexpress;User ID=sa;Password=mYsAb@DpASSW0D;Database=ContactDB;Connect Timeout=5;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;"
  },
```

# [github](https://github.com/gkarsan/ContactsAPI)

# API Content
It has 3 controllers: Contacts, Skills and ContactSkills
Entry points
	GET /Contacts/GetAll : Get all contacts (including the corresponding skills)
	GET /Contacts/GetAllNoChild : Get all contacts (without the corresponding skills)
	GET /Contacts/GetById{id} : Get one contact (including it's skills)
	POST /Contacts/Add : Add a contact (in body, it can contain initial skill list)
	POST /Contacts/AddWithSkills : Add a contact (in body, it cannot contain initial skill list)
	PUT /Contacts/Update : Update a contact (wip should skill by included, if so how)
	DELETE /Contacts/Delete{id} : delete a contact (and any reference to skills, but not skills (check))
	
	GET /Skills/GetAll : Get all skills (including the corresponding contacts)
	GET /Skills/GetAllNoChild : Get all skills (without the corresponding contacts)
	GET /Skills/GetById{id} : Get one contact (including it's associated contacts)
	POST /Skills/Add : Add a skill(in body, it can optionally contain initial contact list)
	PUT /Skills/Update : update a skill (wip should skill by included, if so how)
	DELETE /Skills/Delete{id} : delete a skill (and any reference to contacts, but not contacts (check))
	
	GET /ContactSkills/GetAll : Get all skills (including the corresponding contacts)
	GET /ContactSkills/GetById{id} : Get one contact (without it's associated contacts)
	POST /ContactSkills/Update{idContact,idSkill} : add a skill id to a contact id
	POST /ContactSkills/UpdateWithSkill{id} : add a skill to a contact id (skill detail in body)
	DELETE /ContactSkills/Delete{id} : delete a skill to contact relation (but not relatd contacts or skills)

#  Some debatable choices I made
	Provide 2 endpoints for listing Contacts and Skills: 
		GetAll and GetAllNoChild: one with only contact data, one with embedded skills
	Provide 2 endpoints for adding Contacts and Skills: 
		Add and AddWith[Skills/Contact]: one with only contact/skill data, one with embedded skills/contact
	Provide additional Entripoint to manage rrelations:
		/ContactSkills
		
	
# Test data sample
Add Contact (without skill)
```
{
  "id": 0,
  "firstname": "John",
  "lastname": "Doe",
  "fullname": "Mr John Doe",
  "adderss": "2 steet Avenue",
  "email": "john@somewhee.com",
  "mobilePhoneNumber": "+12345"
}
```
Add Contact with skills (id is ignored at creation and can be omited)
```
{
  "id": 0,
  "firstname": "John",
  "lastname": "Doe",
  "fullname": "Dr John Doe",
  "adderss": "2 steet Avenue",
  "email": "john2@somewhee.com",
  "mobilePhoneNumber": "+12345",
  "skills": [
    {
      "id": 0,
      "name": "Doing Stuff",
      "level": 5
    },
    {
      "id": 0,
      "name": "Undoing Stuff",
      "level": 6
    }
  ]
}
```
Update contact (use existing id from creation, 
unchanged properties can be ommited
```
{
  "id": 27,
  "firstname": "John",
  "lastname": "Doe",
  "fullname": "Mr John Doe",
  "adderss": "2 steet Avenue",
  "email": "john@somewhee.com",
  "mobilePhoneNumber": "+12345"
}
```
Add skill
```
{
  "id": 0,
  "name": "Testing",
  "level": 5
}		
```
Add Skill with Contacts
```
{
  "id": 0,
  "name": "Testing",
  "level": 5,
  "Contacts": [
	{
		"id": 0,
		"firstname": "John",
		"lastname": "Doe2",
		"fullname": "Mr John Doe",
		"adderss": "2 steet Avenue",
		"email": "john@somewhee.com",
		"mobilePhoneNumber": "+12345"
	}
	]
}		
```

# Notes
Developped with Visual Studio 22
.Net Core 6
Entity Famework 6 (Code first)
SwaggerDoc, custom documentation added, including XMLComments fom code
the Solution contains a xUnit test project with some test, which can be run from VisualStudio
authorisation/authentication:MS Identity included but not enabled. !May cause issue as linked with my account..
the project contain a query (queryall.sql) usefull to interogate all contacts with linked skills in diectly in database

