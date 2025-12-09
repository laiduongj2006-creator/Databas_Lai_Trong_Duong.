-- Tạo cơ sở dữ liệu
CREATE DATABASE StoreManagement3;
GO

-- Sử dụng cơ sở dữ liệu vừa tạo
USE StoreManagement3;
GO

-------------------------------------------------------
-- 1. Employee
-------------------------------------------------------
-- Tạo bảng Employee
CREATE TABLE Employee (
    EmployeeID INT IDENTITY(1,1) PRIMARY KEY,         
    EmployeeName VARCHAR(100) NOT NULL,               
    Position VARCHAR(100) NOT NULL,                   
    AuthorityLevel INT NOT NULL,                      
    Username VARCHAR(50) NOT NULL UNIQUE,             
    PasswordHash VARCHAR(50) NOT NULL,  
    IsActive BIT NOT NULL DEFAULT 1
);
-- Customer
CREATE TABLE Customer (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    CustomerName VARCHAR(100) NOT NULL,
    PhoneNumber VARCHAR(20) UNIQUE NOT NULL,
    Email VARCHAR(100),
    Address VARCHAR(200)
);
-- Supplier
CREATE TABLE Supplier (
    SupplierID INT PRIMARY KEY IDENTITY(1,1),
    SupplierName VARCHAR(100) NOT NULL,
    ContactNumber VARCHAR(20),
    Email VARCHAR(100),
    Address VARCHAR(200)
);
-- Category
CREATE TABLE Category (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName VARCHAR(100) NOT NULL,
    Description VARCHAR(200)
);
-- PaymentMethod
CREATE TABLE PaymentMethod (
    PaymentMethodID INT PRIMARY KEY IDENTITY(1,1),
    MethodName VARCHAR(100) NOT NULL,
    Description VARCHAR(200)
);
-- Product Table 
CREATE TABLE Product (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName VARCHAR(100) NOT NULL,
    CategoryID INT NOT NULL,
    SupplierID INT NOT NULL,
    CostPrice DECIMAL(10,2) NOT NULL,
    SellingPrice DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID),
    FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID)
);
select *from product
--Order
CREATE TABLE [Order] (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    OrderDate DATE NOT NULL,
    CustomerID INT NOT NULL,
    EmployeeID INT NOT NULL,
    PaymentMethodID INT NOT NULL,
    TotalAmount DECIMAL(12,2),
    TotalProfit DECIMAL(12,2),
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID),
    FOREIGN KEY (PaymentMethodID) REFERENCES PaymentMethod(PaymentMethodID)
);
-- OrderDetail Table (Tạo mối quan hệ Nhiều-Nhiều giữa Order và Product)
CREATE TABLE OrderDetail (
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
    LineTotal DECIMAL(12,2),
    LineProfit DECIMAL(12,2),
    PRIMARY KEY (OrderID, ProductID),
    FOREIGN KEY (OrderID) REFERENCES [Order](OrderID),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);
-- Employee Data
INSERT INTO Employee (EmployeeName, Position, AuthorityLevel, Username, PasswordHash)
VALUES
('Nguyen Van A', 'Admin', 1, 'adminA', '12345'),
('Tran Thi B', 'Salesperson', 2, 'salesB', 'abc123'),
('Le Van C', 'Salesperson', 2, 'salesC', 'passC'),
('Pham Thi D', 'Warehouse', 3, 'whD', 'd123'),
('Do Van E', 'Warehouse', 3, 'whE', 'e456'),
('Hoang Thi F', 'Salesperson', 2, 'salesF', 'f789');
GO
-- Customer Data
INSERT INTO Customer (CustomerName, PhoneNumber, Email, Address)
VALUES
('Nguyen Quang Minh', '0905123456', 'minh@gmail.com', 'Hanoi'),
('Tran Thi Hoa', '0905345678', 'hoa@gmail.com', 'Danang'),
('Le Van Hung', '0905567890', 'hung@gmail.com', 'HCM'),
('Pham Thi Lan', '0905789012', 'lan@gmail.com', 'Can Tho'),
('Do Thanh Nam', '0905901234', 'nam@gmail.com', 'Hue'),
('Hoang Thi Thuy', '0906123456', 'thuy@gmail.com', 'Nha Trang');
GO
-- Supplier Data
INSERT INTO Supplier (SupplierName, ContactNumber, Email, Address)
VALUES
('ABC Electronics', '0281234567', 'contact@abc.com', 'HCM'),
('TechWorld', '0249876543', 'info@techworld.com', 'Hanoi'),
('GreenFood', '0236123123', 'sales@greenfood.com', 'Danang'),
('FashionLine', '0256789123', 'hello@fashionline.com', 'HCM'),
('SmartShop', '0212345678', 'support@smartshop.com', 'Hue'),
('VietHome', '0223456789', 'viet@home.com', 'Nha Trang');
GO
-- Category Data
INSERT INTO Category (CategoryName, Description)
VALUES
('Electronics', 'Electronic devices and gadgets'),
('Clothing', 'Men and Women fashion items'),
('Food', 'Groceries and packaged food'),
('Furniture', 'Home and office furniture'),
('Accessories', 'Phone and fashion accessories'),
('Stationery', 'Office and school supplies');
GO
-- PaymentMethod Data
INSERT INTO PaymentMethod (MethodName, Description)
VALUES
('Cash', 'Payment by cash'),
('Credit Card', 'Payment via credit card'),
('Bank Transfer', 'Payment through bank account'),
('E-Wallet', 'Payment using Momo, ZaloPay, etc.'),
('Voucher', 'Payment by discount voucher'),
('PayPal', 'Payment via PayPal account');
GO
-- Product Data
INSERT INTO Product (ProductName, CategoryID, SupplierID, CostPrice, SellingPrice)
VALUES 

('iPhone 15', 1, 1, 20000, 25000),
('T-Shirt Nike', 2, 4, 150, 300),
('Rice 10kg', 3, 3, 80, 120),
('Wooden Desk', 4, 6, 500, 800),
('Phone Case', 5, 5, 50, 100),
('Notebook', 6, 2, 20, 40);
GO
-- Order Data 
INSERT INTO [Order] (OrderDate, CustomerID, EmployeeID, PaymentMethodID, TotalAmount, TotalProfit)
VALUES 
('2025-10-01', 1, 2, 1, 25120, 5120),
('2025-10-02', 2, 3, 2, 885, 205),
('2025-10-03', 3, 1, 3, 920, 220),
('2025-10-04', 4, 2, 4, 160, 60),
('2025-10-05', 5, 6, 1, 40000, 8000),
('2025-10-06', 6, 5, 5, 350, 90);
GO
-- OrderDetail Data
INSERT INTO OrderDetail (OrderID, ProductID, Quantity, UnitPrice, LineTotal, LineProfit)
VALUES
(1, 1, 1, 25000, 25000, 5000),
(1, 3, 1, 120, 120, 40),
(2, 2, 2, 300, 600, 100),
(3, 4, 1, 800, 800, 300),
(4, 5, 2, 100, 200, 60),
(5, 1, 1, 25000, 25000, 5000),
(5, 6, 10, 40, 400, 100),
(6, 3, 3, 120, 360, 90);
GO

SELECT * FROM Employee;
SELECT * FROM Customer;
SELECT * FROM Supplier;
SELECT * FROM Category;
SELECT * FROM PaymentMethod;
SELECT * FROM Product;
SELECT * FROM [Order];
SELECT * FROM OrderDetail;




