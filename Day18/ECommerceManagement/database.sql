
CREATE DATABASE ECommerceDB;
GO

USE ECommerceDB;
GO

CREATE TABLE Categories
(
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(300),
    IsActive BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE Products
(
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(150) NOT NULL,
    CategoryId INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL CHECK (Price >= 0),
    Stock INT NOT NULL CHECK (Stock >= 0),
    IsActive BIT NOT NULL DEFAULT 1,

    CONSTRAINT FK_Products_Categories
        FOREIGN KEY (CategoryId)
        REFERENCES Categories(CategoryId)
);
GO

CREATE TABLE Customers
(
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(15) NOT NULL,
    Address NVARCHAR(300),
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    IsActive BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE Orders
(
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    OrderDate DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    TotalAmount DECIMAL(12,2) NOT NULL DEFAULT 0,
    OrderStatus NVARCHAR(20) NOT NULL DEFAULT N'Pending',

    CONSTRAINT FK_Orders_Customers
        FOREIGN KEY (CustomerId)
        REFERENCES Customers(CustomerId),

    CONSTRAINT CK_Orders_Status
        CHECK (OrderStatus IN
        (N'Pending', N'Confirmed', N'Shipped',
         N'Delivered', N'Cancelled'))
);
GO

CREATE TABLE OrderItems
(
    OrderItemId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    UnitPrice DECIMAL(10,2) NOT NULL CHECK (UnitPrice >= 0),

    CONSTRAINT FK_OrderItems_Orders
        FOREIGN KEY (OrderId)
        REFERENCES Orders(OrderId),

    CONSTRAINT FK_OrderItems_Products
        FOREIGN KEY (ProductId)
        REFERENCES Products(ProductId),

    CONSTRAINT UQ_OrderItems_Order_Product
        UNIQUE (OrderId, ProductId)
);
GO

CREATE TABLE AuditLogs
(
    AuditId BIGINT IDENTITY(1,1) PRIMARY KEY,
    EntityName NVARCHAR(50) NOT NULL,
    EntityId INT NOT NULL,
    Action NVARCHAR(30) NOT NULL,
    Details NVARCHAR(500),
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);
GO

PRINT 'ECommerceDB created successfully.';
GO
