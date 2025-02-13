-- Create the database

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'sharedExperiences')
BEGIN
    CREATE DATABASE sharedExperiences;
    PRINT 'Database sharedExperiences created.';
END
ELSE
BEGIN
    PRINT 'Database sharedExperiences already exists.';
END;
GO

-- Switch to the sharedExperiences database
USE sharedExperiences;

-- ******Check if tables already exist and create them if they don't****** --

-- Providers table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Providers')
BEGIN
    CREATE TABLE Providers (
        ProviderId INT PRIMARY KEY IDENTITY(1,1),
        BusinessAddress NVARCHAR(255) NOT NULL,
        PhoneNumber NVARCHAR(20) NOT NULL,
        CVR NVARCHAR(20) NOT NULL
    );
    PRINT 'Table Providers created.';
END
ELSE
BEGIN
    PRINT 'Table Providers already exists.';
END;

-- Guests table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Guests')
BEGIN
    CREATE TABLE Guests (
        GuestId INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        PhoneNumber NVARCHAR(20) NOT NULL,
        Age INT NOT NULL,
        PersonalId NVARCHAR(20) NOT NULL
    );
    PRINT 'Table Guests created.';
END
ELSE
BEGIN
    PRINT 'Table Guests already exists.';
END;

-- Experiences table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Experiences')
BEGIN
    CREATE TABLE Experiences (
        ExperienceId INT PRIMARY KEY IDENTITY(1,1),
        ProviderId INT,
        Name NVARCHAR(255) NOT NULL,
        Description NVARCHAR(MAX),
        Price DECIMAL(10,2) NOT NULL,
        FOREIGN KEY (ProviderId) REFERENCES Providers(ProviderId)
    );
    PRINT 'Table Experiences created.';
END
ELSE
BEGIN
    PRINT 'Table Experiences already exists.';
END;

-- SharedExperiences table 
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SharedExperiences')
BEGIN
    CREATE TABLE SharedExperiences (
        SharedExperienceId INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(255) NOT NULL,
        Date DATE NOT NULL
    );
    PRINT 'Table SharedExperiences created.';
END
ELSE
BEGIN
    PRINT 'Table SharedExperiences already exists.';
END;

-- SharedExperienceExperiences junction table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SharedExperienceExperiences')
BEGIN
    CREATE TABLE SharedExperienceExperiences (
        SharedExperienceId INT,
        ExperienceId INT,
        PRIMARY KEY (SharedExperienceId, ExperienceId),
        FOREIGN KEY (SharedExperienceId) REFERENCES SharedExperiences(SharedExperienceId),
        FOREIGN KEY (ExperienceId) REFERENCES Experiences(ExperienceId)
    );
    PRINT 'Table SharedExperienceExperiences created.';
END
ELSE
BEGIN
    PRINT 'Table SharedExperienceExperiences already exists.';
END;

-- SharedExperienceGuests junction table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SharedExperienceGuests')
BEGIN
    CREATE TABLE SharedExperienceGuests (
        SharedExperienceId INT,
        GuestId INT,
        PRIMARY KEY (SharedExperienceId, GuestId),
        FOREIGN KEY (SharedExperienceId) REFERENCES SharedExperiences(SharedExperienceId),
        FOREIGN KEY (GuestId) REFERENCES Guests(GuestId)
    );
    PRINT 'Table SharedExperienceGuests created.';
END
ELSE
BEGIN
    PRINT 'Table SharedExperienceGuests already exists.';
END;

-- ExperienceRegistrations table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ExperienceRegistrations')
BEGIN
    CREATE TABLE ExperienceRegistrations (
        ExperienceId INT,
        GuestId INT,
        SharedExperienceId INT,
        PRIMARY KEY (ExperienceId, GuestId, SharedExperienceId),
        FOREIGN KEY (ExperienceId) REFERENCES Experiences(ExperienceId),
        FOREIGN KEY (GuestId) REFERENCES Guests(GuestId),
        FOREIGN KEY (SharedExperienceId) REFERENCES SharedExperiences(SharedExperienceId)
    );
    PRINT 'Table ExperienceRegistrations created.';
END
ELSE
BEGIN
    PRINT 'Table ExperienceRegistrations already exists.';
END;

-- VolumeDiscounts table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'VolumeDiscounts')
BEGIN
    CREATE TABLE VolumeDiscounts (
        ProviderId INT,
        MinGroupSize INT NOT NULL,
        DiscountPercentage DECIMAL(5,2) NOT NULL,
        PRIMARY KEY (ProviderId, MinGroupSize),
        FOREIGN KEY (ProviderId) REFERENCES Providers(ProviderId)
    );
    PRINT 'Table VolumeDiscounts created.';
END
ELSE
BEGIN
    PRINT 'Table VolumeDiscounts already exists.';
END;