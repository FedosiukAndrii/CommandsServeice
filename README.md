# Commands Service

## Purpose
Commands Service is a .NET microservice responsible for managing command data and exposing HTTP endpoints for creating and reading commands. It also participates in an event-driven architecture by consuming messages from a message bus and synchronizing platform-related data from other services.

## Main Technologies Used
- **.NET 8 / ASP.NET Core Web API** for building REST endpoints and service hosting.
- **Entity Framework Core (InMemory provider)** for data access and lightweight in-memory persistence.
- **AutoMapper** for mapping between domain models and DTOs.
- **RabbitMQ Client** for asynchronous event consumption through a message bus subscriber.
- **gRPC Client (Grpc.Net.Client + Protobuf)** for synchronous communication with external services.
- **Swagger / Swashbuckle** for API documentation and testing in development mode.
- **Docker** for containerized build and deployment.

## Architecture Notes
- Uses dependency injection for repositories, event processing, event dispatching, and clients.
- Includes asynchronous event handling and event type dispatching.
- Exposes controllers for command and platform resources.
