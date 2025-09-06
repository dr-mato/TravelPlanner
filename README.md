# Travel Planner Backend

This repository contains the **backend implementation** of the capstone project:  
**Travel Planner – An AI-powered personalized trip planning tool**.  

> **Note:** This repository includes only the backend. The frontend (developed with React and TypeScript) is maintained separately and is not part of this repository.  

---

## Project Overview

The Travel Planner system assists users in planning personalized trips by combining artificial intelligence with real-time travel data. The backend is responsible for:  

- Generating personalized recommendations through AI (OpenAI)  
- Retrieving real-time data such as flights and hotels from external APIs  
- Managing business logic, request handling, and data storage  
- Providing RESTful endpoints for the frontend application  

The backend is built with a focus on **Clean Architecture**, maintainability, and modularity.  

---

## System Architecture

The backend follows a **three-layer Clean Architecture**:  

- **Presentation Layer** – ASP.NET Core API controllers, responsible for handling HTTP requests and responses  
- **Application Layer** – Business logic, services, DTOs, and request/response models  
- **Infrastructure Layer** – Database access, external API integrations (OpenAI, Amadeus, Booking.com, Google Maps)  

### Design Principles and Patterns
- Repository Pattern for database access abstraction  
- Dependency Injection for service management  
- Modular API implementations to support multiple data providers  
- Strategy and method overloading within the `OpenAIService` to support different AI request types  

---

## External Integrations

- **OpenAI API** – Personalized destination and itinerary recommendations  
- **Amadeus API** – Flight search and booking data  
- **Booking COM, Skyscanner from RAPIDAPI** – Hotel search and availability  
- **Visual Crossing API** – Weather data

---

## Technologies Used

- C# .NET 8  
- ASP.NET Core Web API  
- Entity Framework Core  
- SQL Database (configurable for local or production use)  
- Swagger for API documentation  

---

## Installation and Setup

### Prerequisites
- .NET 8 SDK  
- SQL Server (or SQLite for local development)  
- API keys for OpenAI, Amadeus, RapidAPI, and VisualCrossing

### Setup Instructions
```bash
# Clone repository
git clone https://github.com/dr-mato/TravelPlanner.git
cd TravelPlanner

# Restore dependencies
dotnet restore

# Apply migrations
dotnet ef database update

# Run project
dotnet run
