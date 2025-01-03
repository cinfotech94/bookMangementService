-- Create multiple databases
CREATE DATABASE "BookManagementService4User";
\c BookManagementService4User;
CREATE TABLE IF NOT EXISTS Users (
    Id UUID PRIMARY KEY,
    Name VARCHAR(255),
    Username VARCHAR(255) UNIQUE,
    Email VARCHAR(255) UNIQUE,
    Role VARCHAR(255),
    PhoneNumber VARCHAR(15),
    Address VARCHAR(500),
    City VARCHAR(255),
    State VARCHAR(255),
    Country VARCHAR(255),
    Balance DOUBLE PRECISION DEFAULT 0,
    Password VARCHAR(255)
);
CREATE DATABASE "BookManagementService4Book";
\c BookManagementService4Book;
CREATE TABLE IF NOT EXISTS Books (
    Id UUID PRIMARY KEY,
    Title VARCHAR(255),
    ISBN VARCHAR(255),
    Author VARCHAR(255),
    PublicationYear VARCHAR(4),
    TimeAdded TIMESTAMP,
    Genre VARCHAR(255),
    Quantity INT DEFAULT 1,
    Price FLOAT,
    Pages INT,
    Description VARCHAR(1000),
    Category VARCHAR(255),
    NoClick INT DEFAULT 0,
    NoOfPurchase INT DEFAULT 0,
    NoOfCart INT DEFAULT 0
);
CREATE TABLE IF NOT EXISTS Carts (
    Username VARCHAR(255),
    BookId UUID,
    PRIMARY KEY (Username, BookId)
);
CREATE DATABASE "BookManagementService4Payment";
\c BookManagementService4Payment;
CREATE TABLE IF NOT EXISTS Purchases (
    Username VARCHAR(255),
    BookId UUID,
    PRIMARY KEY (Username, BookId)
);

