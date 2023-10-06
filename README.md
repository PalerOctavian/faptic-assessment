# faptic-assessment

## Run service:

From root of the project

``` docker-compose up -d ```

Navigate to:

``` http://localhost:5201/faptic-service/swagger/index.html ```

## FapticService

  * Clean architecture
  * Versioned REST API
  * Documentation using Swagger
  * Exception filter
  * CORS enabled (in this case allowing all)
  * CQRS using the mediator design pattern
  * Docker and docker-compose support
  * EntityFramework and Postgres for the database
  * Mappings with AutoMapper
  * Loggings using Serilog
  * Separated on environments
  * Environment variables substitution
  * FluentValidation for endpoints input validation
  * Repository pattern for data access
  * Refit and XUnit for endpoint testing
  * XUnit, FluentAssertions, and AutoFixture for unit testing

### FapticService

  Application entry point. It contains all the configurations for the service, startup logic, and variables for each environment.

### FapticService.API

  It contains the controllers, request and response objects used by the mediator for CQRS, and the DTOs with their mappings.

### FapticService.Domain

  It contains all the contracts for repositories and services, models, and custom exceptions related to the domain. By separating this layer, we keep our service's core "definition" loosely coupled with the rest of the application.

### FapticService.Business

  It is the implementation of our domain, containing all the business logic and the handlers for the CQRS queries and commands.

### FapticService.Common

  Project for extension methods, constants, and any other stuff that could be used in many places of the application.

### FapticService.EntityFramework

  It contains all the implementation for the data layer (context, entity configuration, migrations, repositories logic). Separated for easy changing of the database provider.


