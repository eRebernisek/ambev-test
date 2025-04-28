-- Create Users table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [Users] (
    [Id] uniqueidentifier DEFAULT NEWID() PRIMARY KEY,
    [Username] nvarchar(50) NOT NULL,
    [Password] nvarchar(100) NOT NULL,
    [Phone] nvarchar(20) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [Status] nvarchar(20) NOT NULL,
    [Role] nvarchar(20) NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL DEFAULT GETDATE(),
    [UpdatedAt] datetimeoffset NULL
)
END

-- Create Branches table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Branches]') AND type in (N'U'))
BEGIN
CREATE TABLE [Branches] (
    [Id] uniqueidentifier DEFAULT NEWID() PRIMARY KEY,
    [Name] nvarchar(100) NOT NULL,
    [Location] nvarchar(100) NOT NULL
)
END

-- Create Customers table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customers]') AND type in (N'U'))
BEGIN
CREATE TABLE [Customers] (
    [Id] uniqueidentifier DEFAULT NEWID() PRIMARY KEY,
    [Name] nvarchar(100) NOT NULL
)
END

-- Create Items table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Items]') AND type in (N'U'))
BEGIN
CREATE TABLE [Items] (
    [Id] uniqueidentifier DEFAULT NEWID() PRIMARY KEY,
    [Name] nvarchar(100) NOT NULL,
    [Price] decimal(10,2) NOT NULL,
    [QuantityInStock] int NOT NULL
)
END

-- Create Sales table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sales]') AND type in (N'U'))
BEGIN
CREATE TABLE [Sales] (
    [Id] uniqueidentifier DEFAULT NEWID() PRIMARY KEY,
    [CustomerId] uniqueidentifier NOT NULL,
    [BranchId] uniqueidentifier NOT NULL,
    [TotalPrice] decimal(10,2) NOT NULL,
    [TotalDiscount] decimal(10,2) NOT NULL,
    [SaleDate] datetimeoffset NOT NULL,
    [IsCancelled] bit NOT NULL DEFAULT 0,
    [CreatedAt] datetimeoffset NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [FK_Sales_Branches] FOREIGN KEY ([BranchId]) REFERENCES [Branches]([Id]),
    CONSTRAINT [FK_Sales_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [Customers]([Id])
)
END

-- Create SaleItems table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaleItems]') AND type in (N'U'))
BEGIN
CREATE TABLE [SaleItems] (
    [Id] uniqueidentifier DEFAULT NEWID() PRIMARY KEY,
    [SaleId] uniqueidentifier NOT NULL,
    [ItemId] uniqueidentifier NOT NULL,
    [Quantity] int NOT NULL,
    [UnitPrice] decimal(10,2) NOT NULL,
    [TotalPrice] decimal(10,2) NOT NULL,
    [Discount] decimal(10,2) NOT NULL,
    CONSTRAINT [FK_SaleItems_Sales] FOREIGN KEY ([SaleId]) REFERENCES [Sales]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_SaleItems_Items] FOREIGN KEY ([ItemId]) REFERENCES [Items]([Id])
)
END

-- Insert initial admin user if not exists
IF NOT EXISTS (SELECT * FROM [Users] WHERE [Username] = 'admin')
BEGIN
    INSERT INTO [Users] ([Username], [Password], [Phone], [Email], [Status], [Role], [CreatedAt], [UpdatedAt])
    VALUES (
        'admin',
        '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewLxNyYHm4L.Cavu', -- BCrypt hashed password for 'Admin123'
        '1234567890',
        'admin@email.com',
        'Active',
        'Admin',
        GETDATE(),
        NULL
    )
END

-- Create indexes for better performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Sales_CustomerId' AND object_id = OBJECT_ID('Sales'))
    CREATE INDEX [IX_Sales_CustomerId] ON [Sales]([CustomerId])

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Sales_BranchId' AND object_id = OBJECT_ID('Sales'))
    CREATE INDEX [IX_Sales_BranchId] ON [Sales]([BranchId])

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SaleItems_SaleId' AND object_id = OBJECT_ID('SaleItems'))
    CREATE INDEX [IX_SaleItems_SaleId] ON [SaleItems]([SaleId])

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SaleItems_ItemId' AND object_id = OBJECT_ID('SaleItems'))
    CREATE INDEX [IX_SaleItems_ItemId] ON [SaleItems]([ItemId]) 