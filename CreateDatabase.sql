USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EventManagerDB')
BEGIN
    CREATE DATABASE EventManagerDB 
    ON (NAME = 'EventManagerDB', 
        FILENAME = 'D:\EventMaster-1\EventMenager\Database\EventManagerDB.mdf')
    LOG ON (NAME = 'EventManagerDB_Log', 
            FILENAME = 'D:\EventMaster-1\EventMenager\Database\EventManagerDB_Log.ldf');
END
GO

USE EventManagerDB;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Ime NVARCHAR(50) NOT NULL,
        Email NVARCHAR(100) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(255) NOT NULL,
        Role NVARCHAR(20) NOT NULL DEFAULT 'User',
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'EventTypes')
BEGIN
    CREATE TABLE EventTypes (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Naziv NVARCHAR(50) NOT NULL UNIQUE,
        Opis NVARCHAR(200)
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PerformerTypes')
BEGIN
    CREATE TABLE PerformerTypes (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Naziv NVARCHAR(50) NOT NULL UNIQUE,
        Opis NVARCHAR(200)
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Events')
BEGIN
    CREATE TABLE Events (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Naziv NVARCHAR(100) NOT NULL,
        Opis NVARCHAR(MAX),
        Datum DATETIME2 NOT NULL,
        Lokacija NVARCHAR(200) NOT NULL,
        EventTypeId INT NOT NULL,
        AdminId INT NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
        FOREIGN KEY (AdminId) REFERENCES Users(Id),
        FOREIGN KEY (EventTypeId) REFERENCES EventTypes(Id)
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Performers')
BEGIN
    CREATE TABLE Performers (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Ime NVARCHAR(100) NOT NULL,
        Opis NVARCHAR(MAX),
        PerformerTypeId INT NOT NULL,
        FOREIGN KEY (PerformerTypeId) REFERENCES PerformerTypes(Id)
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'EventPerformers')
BEGIN
    CREATE TABLE EventPerformers (
        EventId INT NOT NULL,
        PerformerId INT NOT NULL,
        PRIMARY KEY (EventId, PerformerId),
        FOREIGN KEY (EventId) REFERENCES Events(Id) ON DELETE CASCADE,
        FOREIGN KEY (PerformerId) REFERENCES Performers(Id) ON DELETE CASCADE
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Subscriptions')
BEGIN
    CREATE TABLE Subscriptions (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        EventId INT NOT NULL,
        SubscribedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
        FOREIGN KEY (EventId) REFERENCES Events(Id) ON DELETE CASCADE,
        UNIQUE(UserId, EventId)
    );
END
GO

-- Insert initial data
IF NOT EXISTS (SELECT * FROM Users WHERE Email = 'admin@test.com')
BEGIN
    INSERT INTO Users (Ime, Email, PasswordHash, Role) 
    VALUES ('Admin', 'admin@test.com', 'admin123', 'Admin');
END
GO

IF NOT EXISTS (SELECT * FROM Users WHERE Email = 'user@test.com')
BEGIN
    INSERT INTO Users (Ime, Email, PasswordHash, Role) 
    VALUES ('User', 'user@test.com', 'user123', 'User');
END
GO

IF NOT EXISTS (SELECT * FROM EventTypes WHERE Naziv = 'Koncert')
BEGIN
    INSERT INTO EventTypes (Naziv, Opis) VALUES ('Koncert', 'Muzički koncert');
END
GO

IF NOT EXISTS (SELECT * FROM EventTypes WHERE Naziv = 'Festival')
BEGIN
    INSERT INTO EventTypes (Naziv, Opis) VALUES ('Festival', 'Muzički festival');
END
GO

IF NOT EXISTS (SELECT * FROM EventTypes WHERE Naziv = 'Kazalište')
BEGIN
    INSERT INTO EventTypes (Naziv, Opis) VALUES ('Kazalište', 'Kazališna predstava');
END
GO

IF NOT EXISTS (SELECT * FROM PerformerTypes WHERE Naziv = 'Band')
BEGIN
    INSERT INTO PerformerTypes (Naziv, Opis) VALUES ('Band', 'Muzički bend');
END
GO

IF NOT EXISTS (SELECT * FROM PerformerTypes WHERE Naziv = 'Solo')
BEGIN
    INSERT INTO PerformerTypes (Naziv, Opis) VALUES ('Solo', 'Solo izvođač');
END
GO

IF NOT EXISTS (SELECT * FROM PerformerTypes WHERE Naziv = 'Orkestar')
BEGIN
    INSERT INTO PerformerTypes (Naziv, Opis) VALUES ('Orkestar', 'Orkestar');
END
GO

IF NOT EXISTS (SELECT * FROM Performers WHERE Ime = 'Rock Band')
BEGIN
    INSERT INTO Performers (Ime, Opis, PerformerTypeId) VALUES ('Rock Band', 'Najbolji rock bend', 1);
END
GO

IF NOT EXISTS (SELECT * FROM Performers WHERE Ime = 'Jazz Trio')
BEGIN
    INSERT INTO Performers (Ime, Opis, PerformerTypeId) VALUES ('Jazz Trio', 'Profesionalni jazz sastav', 1);
END
GO

IF NOT EXISTS (SELECT * FROM Performers WHERE Ime = 'Pop Star')
BEGIN
    INSERT INTO Performers (Ime, Opis, PerformerTypeId) VALUES ('Pop Star', 'Poznati pop izvođač', 2);
END
GO

IF NOT EXISTS (SELECT * FROM Events WHERE Naziv = 'Rock Koncert')
BEGIN
    INSERT INTO Events (Naziv, Opis, Datum, Lokacija, EventTypeId, AdminId) 
    VALUES ('Rock Koncert', 'Najbolji rock bendovi u gradu', DATEADD(day, 30, GETDATE()), 'Gradski stadion', 1, 1);
END
GO

IF NOT EXISTS (SELECT * FROM Events WHERE Naziv = 'Jazz Festival')
BEGIN
    INSERT INTO Events (Naziv, Opis, Datum, Lokacija, EventTypeId, AdminId) 
    VALUES ('Jazz Festival', 'Jazz glazba pod vedrim nebom', DATEADD(day, 45, GETDATE()), 'Gradski park', 2, 1);
END
GO

IF NOT EXISTS (SELECT * FROM EventPerformers WHERE EventId = 1 AND PerformerId = 1)
BEGIN
    INSERT INTO EventPerformers (EventId, PerformerId) VALUES (1, 1);
END
GO

IF NOT EXISTS (SELECT * FROM EventPerformers WHERE EventId = 2 AND PerformerId = 2)
BEGIN
    INSERT INTO EventPerformers (EventId, PerformerId) VALUES (2, 2);
END
GO 