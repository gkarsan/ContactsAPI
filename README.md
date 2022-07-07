# ContactsAPI
It’s a simple API, where a user can get a quick overview over all contacts resources
like person, skills...

# Initial Project Setup
The api uses MS SQL Server database. Before running the first time, ensure the connection string in appsettings.json is set up right for the situation.
Currerntly, if running in docker, it can't use (localdb) and need path to the server, even if running on local. If running in a windows system with local MS SQL Server or Express, it can work with locladb

#API Content
It has 3 controllers: Contacts, Skills and ContactSkill
Entry points
	GET /Contacts/GetAll : Get all contacts (including the corresponding skills)
	GET /Contacts/GetById?{id} : Get one contact (including it's skills)
	POST /Contacts/Add : Add a contact (in body, it can contain initial skill list)
	PUT /Contacts/Update : Update a contact (wip should skill by included, if so how)
	
	GET /Skills/GetAll : Get all skills (including the corresponding contacts?)
	GET /Skills/GetById?{id} : Get one contact (including it's associated contacts?)
	POST /Skills/Add : Add a skill(in body, it can contain initial contact list ???)
	PUT /Skills/AddWithSkill : update a skill (wip should skill by included, if so how)
	
	GET /ContactSkills/GetAll : Get all skills (including the corresponding contacts?)
	TODO GET /ContactSkills/GetById?{id} : Get one contact (including it's associated contacts?)
	POST /ContactSkills/Update?{idContact,idSkill} : add a skill id to a contact id
	POST /ContactSkills/UpdateWithSkill?{id} : add a skill to a contact id (skill detail in body)