-- opret databasen

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