# CDNMiddleware
RESTful API create for a fictional company, CDN - Complete Developer Network to contain list of freelancers.
Contain CRUD operation for User.

## Framework/Tools
* Net Core 6
* Entity Framework - Code First
* Linq
* MySql
* Dependency Injection

## Project Structure
The solution contain 3 projects based on functionality.
1. API  
   Contain end point for Restful API
2. Application  
   Contain services for manipulating data access.
3. Data Access  
   Contain database queries and connection.

## Try it out 
*  In RDBMS database, Create database CDNMiddleware. 
*  Run dotnet ef --startup-project ../CDNMiddleware.Api database update --context CDNMiddlewareDbContext to update database.
*  OR Run dotnet ef --startup-project ../CDNMiddleware.Api/ migrations script --idempotent -o script.sql --context CDNMiddlewareDbContext. Copy all from script.sql and run the query.
*  I'm using second way to update the database if the project have more than one environment. I want to avoid from accidental update Production database.
*  Run CDNMiddleware.Api
*  Try Api using http://localhost:port/swagger/index.html
