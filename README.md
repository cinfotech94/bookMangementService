<!DOCTYPE html>
<html>
<head>
    <title>Project Overview</title>
</head>
<body>
    <h1>Overview</h1>
    <p>
        This project consists of two microservices: <strong>Book Management Service</strong> and <strong>User Management Service</strong>, designed to work together to manage books and users effectively. Both services are set up using <strong>Docker Compose</strong> to ensure all necessary dependencies are easily configured.
    </p>

    <h2>Services</h2>
    <ul>
        <li>
            <strong>Book Management Service:</strong>
            <ul>
                <li>Handles all book-related operations, including buying and management.</li>
            </ul>
        </li>
        <li>
            <strong>User Management Service:</strong>
            <ul>
                <li>Manages user-related operations, such as user profiles, cart management, and authentication.</li>
                <li>Facilitates communication between services using <strong>RabbitMQ</strong> for asynchronous messaging and <strong>gRPC</strong> for immediate response requests.</li>
            </ul>
        </li>
    </ul>

    <h2>Features</h2>
    <h3>Dependencies</h3>
    <ul>
        <li><strong>PostgreSQL:</strong> Main database for structured data.</li>
        <li><strong>Elasticsearch:</strong> Search functionality.</li>
        <li><strong>RabbitMQ:</strong> Asynchronous messaging.</li>
        <li><strong>MongoDB:</strong> Logging and auditing.</li>
        <li><strong>Serilog:</strong> File-based logging and integration with Elasticsearch.</li>
    </ul>

    <h3>Security</h3>
    <p>Middleware handles input validation and security headers, including:</p>
    <ul>
        <li>Content Security Policy</li>
        <li>Correlation Middleware</li>
        <li>Input Validation</li>
        <li>JSON Validation</li>
        <li>Strict Transport Security</li>
        <li>Referrer Policy</li>
        <li>X-Content-Type Options</li>
        <li>X-Frame-Options</li>
    </ul>
    <p>Passwords are securely encrypted using a dedicated encryption service. IP Service captures request IPs during logging and auditing.</p>

    <h3>Architecture</h3>
    <p>Layered architecture with strict adherence to <strong>SOLID principles</strong> and <strong>OOP practices</strong>.</p>
    <p>Design patterns include:</p>
    <ul>
        <li>DTO</li>
        <li>Repository Pattern</li>
        <li>Service Pattern</li>
    </ul>

    <h3>Error Handling</h3>
    <p>Middleware such as <strong>Exception Middleware</strong> handles errors gracefully. Background services retry failed tasks.</p>

    <h3>Documentation</h3>
    <p>Two Swagger pages:</p>
    <ul>
        <li>Main service.</li>
        <li>Login service, with Bearer token integration for secure access.</li>
    </ul>

    <h3>Auditing</h3>
    <p>MongoDB stores all audit logs, including inbound request logs.</p>

    <h2>Development Highlights</h2>
    <h3>Validation</h3>
    <p><strong>FluentValidation</strong> ensures all inputs are validated.</p>

    <h3>Task Management</h3>
    <p>Background services manage tasks that fail, with retry mechanisms in place.</p>
</body>
</html>
