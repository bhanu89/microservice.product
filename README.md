# microservice.product

## Purpose
The purpose of this microservice is to perform simple CRUD operations on `Products` while showcasing some of the technical skills of the author. To this degree, third party library usage has been minimized and custom implementations have been written.

The objective is not simply to produce an application that meets the criteria of the assessment but to develop an application with design patterns that can be easily understood and extended by other developers with varying degrees of experience.

## Highlights
The following features of this application can be highlighted - 
1. Docker containerized
2. CQRS pattern combined with Domain driven design principles
4. In-memory caching
5. Sliding rate limiter for requests
6. Concurrency check for updates
7. Product's price uses Money pattern

## Details
### Application
This application is built in .NET 8, uses Dapper for ORM and Postgresql for storage. 
The migration to setup the schema is located in a file named `Seed.sql`. It creates two tables `Product` and `Currency` in `retail` schema. 
Once the Postgres database has been setup on the local environment, the connection string parameters located in `appsettings.development.json` need to be changed per local database config.
The application could be run as Docker container and it would bring up the Swagger page with available endpoints.

### Missing Pieces
Due to time constraints, some pieces of the code could not be completed. The author had these items in mind for the next steps
1. Unit tests
2. Integration tests
3. Custom middleware for http status code handling
4. CRUD endpoints for Currency
5. Enhancements to `GetProducts` query to support more search criteria