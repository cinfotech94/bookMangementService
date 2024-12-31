# Project Overview

## Overview

This project consists of two microservices: **Book Management Service** and **User Management Service**, designed to work together to manage books and users effectively. Both services are set up using **Docker Compose** to ensure all necessary dependencies are easily configured.

## Services

- **Book Management Service:**
  - Handles all book-related operations, including buying and management.

- **User Management Service:**
  - Manages user-related operations, such as user profiles, cart management, and authentication.
  - Facilitates communication between services using **RabbitMQ** for asynchronous messaging and **gRPC** for immediate response requests.

## Features

### Dependencies

- **PostgreSQL:** Main database for structured data.
- **Elasticsearch:** Search functionality.
- **RabbitMQ:** Asynchronous messaging.
- **MongoDB:** Logging and auditing.
- **Serilog:** File-based logging and integration with Elasticsearch.

### Security

Middleware handles input validation and security headers, including:

- Content Security Policy
- Correlation Middleware
- Input Validation
- JSON Validation
- Strict Transport Security
- Referrer Policy
- X-Content-Type Options
- X-Frame-Options

Passwords are securely encrypted using a dedicated encryption service. IP Service captures request IPs during logging and auditing.

### Architecture

Layered architecture with strict adherence to **SOLID principles** and **OOP practices**.

Design patterns include:

- DTO
- Repository Pattern
- Service Pattern

### Error Handling

Middleware such as **Exception Middleware** handles errors gracefully. Background services retry failed tasks.

### Documentation

Two Swagger pages:

- Main service.
- Login service, with Bearer token integration for secure access.

### Auditing

MongoDB stores all audit logs, including inbound request logs.

## Development Highlights

### Validation

**FluentValidation** ensures all inputs are validated.

### Task Management

Background services manage tasks that fail, with retry mechanisms in place.
