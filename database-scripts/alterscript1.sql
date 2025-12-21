/* =========================================================
   CloudSpend Database Setup + Seed Script
   Safe to run multiple times
   ========================================================= */

------------------------------------------------------------
-- 1️⃣ Create database if it does not exist
------------------------------------------------------------
IF NOT EXISTS (
    SELECT 1 FROM sys.databases WHERE name = 'CloudSpendDb'
)
BEGIN
    CREATE DATABASE CloudSpendDb;
END
GO

------------------------------------------------------------
-- 2️⃣ Switch to database
------------------------------------------------------------
USE CloudSpendDb;
GO

------------------------------------------------------------
-- 3️⃣ Create Users table if it does not exist
------------------------------------------------------------
IF NOT EXISTS (
    SELECT 1
    FROM sys.tables
    WHERE name = 'Users'
)
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Email NVARCHAR(255) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(255) NOT NULL,
        CreatedBy NVARCHAR(255) NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

------------------------------------------------------------
-- 4️⃣ Insert default admin user (only if not exists)
------------------------------------------------------------
IF NOT EXISTS (
    SELECT 1 FROM Users WHERE Email = 'admin@cloudspend.com'
)
BEGIN
    INSERT INTO Users (Email, PasswordHash, CreatedBy)
    VALUES (
        'admin@cloudspend.com',
        -- password = Admin@123 (hashed)
        'AQAAAAEAACcQAAAAEMockHashedPasswordReplaceLater==',
        'system'
    );
END
GO