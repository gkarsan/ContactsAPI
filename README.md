# ContactsAPI
It’s a simple API, where a user can get a quick overview over all contacts resources
like person, skills...

# Initial Project Setup
The api uses MS SQL Server database. Before running the first time, ensure the connection string in appsettings.json is set up right for the situation.
Currerntly, if running in docker, it can't use (localdb) and need path to the server, even if running on local. If running in a windows system with local MS SQL Server or Express, it can work with locladb