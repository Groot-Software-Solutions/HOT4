# Introduction 
This project is a migration to c# & refactoring with the goal to have clean readable code. 

# Getting Started 
1.	Installation process 
Classes use dependency injection so you need a DI host and the call the lines below in config services.
	services.AddApplication();
	services.AddInfrastrusture();
Run Actions using mediator.Send(new ActionRequiredCommand(Request))
	
2.	Software dependencies 
[Mediatr] for query & request methodoligy 
[OneOf] for proper return options
[Dapper] for SQL data 
[Serilog] for logging
[Various MS Packages] for DI and Config Handling 

# Build and Test
TODO: Describe and show how to build your code and run the tests. 
[Unit Test to be Intergrated soon]

# Contribute
Current Work TODO: 
- Add All tables to dbcontext
- Make SMS Hanlder processes
- Make a hot.core equivalent workflow for recharges


# Project Specific Instructions
[Adding Data tables to DB Context]
	- Create Entity class 
	- Create table interface in dbtables folder in application 
	- Add Table specific functions & stored procedures to interface
	- Implement interface in DbTables folder in infrastructure
	- Add interface to DbContext 
	- Link interface to implemenation as singleton in Infrastructure dependancy injection class

# Code Guidelines 
[Coding Style]
- Be clear & specific with naming but not verbose i.e Loading Recent Messages = [GetRecent] in Messages Class but [GettingRecentMessages] if in a different class.
- Do not repeat yourself(DRY) if find code that is similiar make a method or abstract class and parameterize everything. 
- Classes and methods be readable. 
	- Just cause it looks cool doesn't mean it should be done. 3 lines of code anyone can understand is better than a one-liner everyone is going to have to google.
	- Remember code is for humans to read, machines only get to see assembly from compiler.  
- Long methods and classes usually mean your are doing too much in one place. 4 short methods are better than 1 long one especially if you name the correctly.
- Code must depend on interfaces & not implementations to allow for substitutions. The unit tests will thanks you later
- Anticipate failure and plan for it, unhandled exceptions should be unexepected not a design choice. Errors dont always happen as you expect them,	If you can not handle 
	your errors make sure others know they have too.

[Code Locations]
Put it in:
Domain - If it defines what something is for everyone
Application - If it defines how to do something
Infrastructure - If it actually does something
Definitions for external or specific items should be as closest scope possible. i.e. external API models should be in a folder next to external API client
