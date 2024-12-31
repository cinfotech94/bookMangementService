CREATE TABLE books (
    id UUID PRIMARY KEY,
    title TEXT NOT NULL,
    ISBN TEXT NOT NULL,
    author TEXT NOT NULL,
    publicationYear TEXT NOT NULL,
    timeAdded TIMESTAMP WITH TIME ZONE NOT NULL,
    genre TEXT NOT NULL,
    quantity INTEGER NOT NULL,
    price DOUBLE PRECISION NOT NULL,
    pages INTEGER NOT NULL,
    description TEXT NOT NULL,
    category TEXT NOT NULL
);
CREATE TABLE users (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    role VARCHAR(50) NOT NULL,
    phoneNumber VARCHAR(20) NOT NULL,
    address TEXT NOT NULL,
    city VARCHAR(100) NOT NULL,
    state VARCHAR(100) NOT NULL,
    country VARCHAR(100) NOT NULL,
    balance DOUBLE PRECISION DEFAULT 0,
    password VARCHAR(255) NOT NULL
);
CREATE TABLE carts (
    username VARCHAR(255) NOT NULL,
    bookId VARCHAR(255) NOT NULL,
    PRIMARY KEY (username, bookId)
);
