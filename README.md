# 🛒 E-Commerce API

A production-ready E-Commerce RESTful API built with **ASP.NET Core Web API** following **Clean Architecture principles**.  
The project implements real-world backend concepts including authentication, authorization, caching, payment processing, order management, and scalable data access patterns.

---

## 🚀 Features

### 🔐 Authentication & Authorization

- ASP.NET Core Identity integration
- JWT Bearer Authentication
- Role-Based Authorization
- Secure user registration and login
- Claims-based user information extraction

Implemented:
- Users
- Roles
- Authentication Flow
- Protected Endpoints

---

## 🏗️ Clean Architecture

The solution follows a layered architecture:

```
ECommerce.API
│
├── Core
│   ├── Entities
│   ├── Interfaces
│   ├── Specifications
│   └── Common
│
├── Application
│   ├── Services
│   ├── DTOs
│   ├── Mapping Profiles
│   └── Business Logic
│
├── Infrastructure
│   ├── Database
│   ├── Identity
│   ├── Repositories
│   ├── Redis
│   └── External Services
│
└── API
    ├── Controllers
    ├── Middleware
    └── Configuration
```

---

# 📦 Product Management

The API provides complete product catalog management.

Features:

- Create Products
- Retrieve Products
- Product Filtering
- Product Sorting
- Product Pagination
- Product Details
- Brands Management
- Types Management

---

# 🔎 Specification Pattern

Implemented Specification Pattern to build reusable and dynamic queries.

Supports:

- Filtering
- Sorting
- Pagination
- Including related entities
- Query composition

Example:

```
GET /api/products?brandId=2&typeId=3&pageIndex=1&pageSize=10&sort=name
```

---

# 📄 Pagination & Query Parameters

The API supports flexible data querying using query parameters.

Example:

```
GET /api/products?pageIndex=1&pageSize=20
```

Features:

- Page index
- Page size
- Sorting
- Filtering

This improves performance when dealing with large datasets.

---

# 🛒 Basket System

Implemented shopping basket functionality using Redis.

Features:

- Add items to basket
- Update basket
- Delete basket
- Retrieve customer basket

Redis is used for:

- Fast access
- Temporary storage
- Improved performance

Basket expiration is configured using Redis TTL.

---

# 💳 Stripe Payment Integration

Integrated Stripe Payment Gateway for secure online payments.

Payment Flow:

```
Customer
   |
Create Basket
   |
Create Payment Intent
   |
Receive Client Secret
   |
Complete Payment
   |
Stripe Webhook
   |
Update Order Payment Status
```

Implemented:

- Payment Intent Creation
- Payment Intent Update
- Stripe Configuration
- Secure API Key Management

---

# 🔔 Stripe Webhook

Implemented Stripe Webhook handling to receive payment events.

Webhook responsibilities:

- Validate Stripe events
- Receive payment confirmation
- Update order payment status
- Handle failed payments

Example events:

```
payment_intent.succeeded
payment_intent.payment_failed
```

---

# 📦 Order Management

Complete order workflow:

Features:

- Create Order
- Retrieve User Orders
- Retrieve Order Details
- Manage Delivery Methods
- Track Payment Status

Order includes:

- Customer information
- Ordered products
- Delivery details
- Payment information

---

# ⚡ Caching

Implemented caching to improve API performance.

Used:

- Redis Distributed Cache

Caching applied for frequently accessed data such as:

- Products
- Catalog information

Benefits:

- Reduced database calls
- Faster response time
- Better scalability

---

# 🌱 Database Seeding

Automatic database initialization with predefined data.

Seeded:

- Product Types
- Brands
- Products
- Delivery Methods
- Identity Roles

---

# ❌ Error Handling

Implemented centralized error handling.

Features:

- Global Exception Middleware
- Consistent API responses
- Custom error types

---

# ✅ Result Pattern

Implemented Result Pattern instead of throwing exceptions for expected failures.

Example:

Successful Response:

```json
{
  "isSuccess": true,
  "value": {}
}
```

Failure Response:

```json
{
  "isSuccess": false,
  "error": {
      "code": "Product.NotFound",
      "message": "Product was not found"
  }
}
```

Benefits:

- Cleaner business logic
- Better error management
- Consistent responses

---

# 🛠️ Technologies Used

## Backend

- ASP.NET Core Web API
- C#
- Entity Framework Core
- LINQ
- SQL Server

## Authentication

- ASP.NET Core Identity
- JWT

## Database

- SQL Server
- EF Core Code First
- Migrations

## Caching

- Redis
- StackExchange.Redis

## Payment

- Stripe API
- Stripe Webhooks

## Tools

- Visual Studio
- Swagger
- Git & GitHub

---

# ⚙️ Getting Started

## Prerequisites

- .NET 8 SDK
- SQL Server
- Redis
- Stripe Account

---


Swagger documentation:

```
https://localhost:xxxx/swagger
```

# 🧠 Software Engineering Concepts Applied

✔ Clean Architecture  
✔ Repository Pattern  
✔ Unit Of Work Pattern  
✔ Specification Pattern  
✔ Result Pattern  
✔ Dependency Injection  
✔ Middleware Pipeline  
✔ DTO Pattern  
✔ Separation of Concerns  
✔ External Service Integration  
✔ Secure Configuration Management  

---

# 📈 Future Improvements

- Docker Containerization
- Automated Testing (xUnit)
- Background Jobs
- Email Notifications
- Logging with Serilog
- CI/CD Pipeline
- Deployment to Cloud

---

# 👨‍💻 Author

**Ahmed Mohamed**

Backend Developer | ASP.NET Core
