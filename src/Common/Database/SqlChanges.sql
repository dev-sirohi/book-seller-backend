IF NOT EXISTS (SELECT 1 FROM sys.objects 
               WHERE object_id = OBJECT_ID(N'TUser') 
                 AND type = 'U')
BEGIN
    CREATE TABLE TUser
    (
        Id INT IDENTITY(1,1),
		UserId NVARCHAR(255) PRIMARY KEY,
        Name NVARCHAR(255) NOT NULL,
		Email NVARCHAR(255) NOT NULL UNIQUE,
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        IsEmailVerified BIT NOT NULL DEFAULT 0,
		IsActive BIT NOT NULL DEFAULT 0
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

-- Table Insertions

-- TUserAuthLinkType
IF NOT EXISTS (SELECT 1 FROM TUserAuthLinkType WHERE NAME = 'Password Reset')
BEGIN
    
    INSERT INTO TUserAuthLinkType(NAME) VALUES('Password Reset');
END

IF NOT EXISTS (SELECT 1 FROM TUserAuthLinkType WHERE NAME = 'Email Verification')
BEGIN
    INSERT INTO TUserAuthLinkType(NAME) VALUES('Email Verification');
END