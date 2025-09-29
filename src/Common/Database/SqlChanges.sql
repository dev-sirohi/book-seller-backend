IF NOT EXISTS (SELECT 1 FROM sys.objects 
               WHERE object_id = OBJECT_ID(N'TUser') 
                 AND type = 'U')
BEGIN
    CREATE TABLE TUser
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
		UserId NVARCHAR(255) UNIQUE,
        Name NVARCHAR(255) NOT NULL,
		Email NVARCHAR(255) NOT NULL UNIQUE,
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        IsEmailVerified BIT NOT NULL DEFAULT 0,
		IsActive BIT NOT NULL DEFAULT 0,
        IsWriter BIT NOT NULL DEFAULT 0
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects 
               WHERE object_id = OBJECT_ID(N'TUserAuth') 
                 AND type = 'U')
BEGIN
    CREATE TABLE TUserAuth
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
		UserId NVARCHAR(255) UNIQUE,
        Password NVARCHAR(255) NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        ModifiedAt DATETIME2
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects 
               WHERE object_id = OBJECT_ID(N'TUserAuthLink') 
                 AND type = 'U')
BEGIN
    CREATE TABLE TUserAuthLink
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
		UserId NVARCHAR(255) UNIQUE,
        Link NVARCHAR(4000) UNIQUE,
        LinkType INT,
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects 
               WHERE object_id = OBJECT_ID(N'TUserAuthLinkType') 
                 AND type = 'U')
BEGIN
    CREATE TABLE TUserAuthLinkType
    (
        Id INT IDENTITY(10,10) PRIMARY KEY,
        Name NVARCHAR(255) NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects 
               WHERE object_id = OBJECT_ID(N'TXAdmin') 
                 AND type = 'U')
BEGIN
    CREATE TABLE TXAdmin
    (
        UserId NVARCHAR(255) NOT NULL UNIQUE,
        StatusId INT NOT NULL DEFAULT 10
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects 
               WHERE object_id = OBJECT_ID(N'TXAdminStatus') 
                 AND type = 'U')
BEGIN
    CREATE TABLE TXAdminStatus
    (
        Id INT IDENTITY(10, 10) PRIMARY KEY,
        Name NVARCHAR(10) NOT NULL UNIQUE
    );
END
GO

-- Table Insertions For Testing

-- TUserAuthLinkType
IF NOT EXISTS (SELECT 1 FROM TUserAuthLinkType WHERE NAME = 'Password Reset')
BEGIN
    
    INSERT INTO TUserAuthLinkType(NAME) VALUES('Password Reset');
END
GO

IF NOT EXISTS (SELECT 1 FROM TUserAuthLinkType WHERE NAME = 'Email Verification')
BEGIN
    INSERT INTO TUserAuthLinkType(NAME) VALUES('Email Verification');
END
GO

--TUser

IF NOT EXISTS (SELECT 1 FROM TUser WHERE EMAIL = 'tester1@bsb.com')
BEGIN
    INSERT INTO TUser(UserId, Name, Email, IsEmailVerified, IsActive, IsWriter) VALUES('52112249-7859-4662-B46E-C1CACA1D85ED', 'Tester 1', 'tester1@bsb.com', 1, 1, 1);
END
GO

--TUserAuth

IF NOT EXISTS (SELECT 1 FROM TUserAuth WHERE UserId = (SELECT TOP 1 UserId FROM TUser))
BEGIN
    INSERT INTO TUserAuth(UserId, Password) VALUES((SELECT TOP 1 UserId FROM TUser), 'tester1@bsb');
END
GO

--TXAdminStatus

IF NOT EXISTS (SELECT 1 FROM TXAdminStatus WHERE NAME IN ('Read', 'Read/Write'))
BEGIN
    INSERT INTO TXAdminStatus(Name) VALUES('Read', 'Read/Write');
END
GO

--TXAdminStatus

IF NOT EXISTS (SELECT 1 FROM TXAdmin WHERE UserId = (SELECT TOP 1 UserId FROM TUser))
BEGIN
    INSERT INTO TXAdmin(UserId, StatusId) VALUES((SELECT TOP 1 UserId FROM TUser), 10);
END
GO