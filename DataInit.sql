-- --------------------------------------------------
-- CREATE TABLE
-- --------------------------------------------------
-- sql server create role table
CREATE TABLE [dbo].[Role]
(
    [roleId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [roleName] [nvarchar](50) NOT NULL,
)
-- sql server create account table
CREATE TABLE [dbo].[Account]
(
    [accountId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [userName] [nvarchar](50) NOT NULL,
    [hashedPassword] [nvarchar](255) NOT NULL,
    [dob] [datetime] DEFAULT (getdate()),
    [email] [nvarchar](255) NOT NULL,
    [isDeleted] [bit] NOT NULL DEFAULT 0,
    [deletedReason] [nvarchar](255) DEFAULT NULL,
)
-- sql server create account role table
CREATE TABLE [dbo].[AccountRole]
(
    [arId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [accountId] [int] NOT NULL,
    [roleId] [int] NOT NULL,
    FOREIGN KEY ([accountId]) REFERENCES [dbo].[Account] ([accountId]),
    FOREIGN KEY ([roleId]) REFERENCES [dbo].[Role] ([roleId])
)
-- sql server create song table
CREATE TABLE [dbo].[Song]
(
    [songId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [songName] [nvarchar](255) NOT NULL,
    [author] [nvarchar](255) NOT NULL,
    [bpm] [int] NOT NULL DEFAULT 100,
    [notes] [nvarchar](1000) NOT NULL,
    [releaseDate] [datetime] NOT NULL DEFAULT (getdate()),
    [isPublic] [bit] NOT NULL DEFAULT 0,
    [isDeleted] [bit] NOT NULL DEFAULT 0,
    [deletedReason] [nvarchar](255) DEFAULT NULL,
)
-- sql server create tag table
CREATE TABLE [dbo].[Tag]
(
    [tagId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [tagName] [nvarchar](50) NOT NULL,
)
-- sql server create song tag table
CREATE TABLE [dbo].[SongTag]
(
    [stId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [songId] [int] NOT NULL,
    [tagId] [int] NOT NULL,
    FOREIGN KEY ([songId]) REFERENCES [dbo].[Song] ([songId]),
    FOREIGN KEY ([tagId]) REFERENCES [dbo].[Tag] ([tagId])
)
-- sql server create account song table
CREATE TABLE [dbo].[AccountSong]
(
    [asId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [accountId] [int] NOT NULL,
    [songId] [int] NOT NULL,
    [bestScore] [int] NOT NULL DEFAULT 0,
    FOREIGN KEY ([accountId]) REFERENCES [dbo].[Account] ([accountId]),
    FOREIGN KEY ([songId]) REFERENCES [dbo].[Song] ([songId])
)
-- sql server create song report table
CREATE TABLE [dbo].[SongReport]
(
    [reportId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [songId] [int] NOT NULL,
    [accountId] [int] NOT NULL,
    [reportTitle] [nvarchar](255) NOT NULL,
    [reportReason] [nvarchar](255) NOT NULL,
    [reportDate] [datetime] NOT NULL DEFAULT (getdate()),
    [reportStatus] [bit] DEFAULT NULL,
    FOREIGN KEY ([songId]) REFERENCES [dbo].[Song] ([songId]),
    FOREIGN KEY ([accountId]) REFERENCES [dbo].[Account] ([accountId])
)
-- --------------------------------------------------
-- DROP TABLE
-- --------------------------------------------------
-- sql server drop role table
DROP TABLE [dbo].[Role]
-- sql server drop account table
DROP TABLE [dbo].[Account]
-- sql server drop account role table
DROP TABLE [dbo].[AccountRole]
-- sql server drop song table
DROP TABLE [dbo].[Song]
-- sql server drop tag table
DROP TABLE [dbo].[Tag]
-- sql server drop song tag table
DROP TABLE [dbo].[SongTag]
-- sql server drop account song table
DROP TABLE [dbo].[AccountSong]
-- sql server drop song report table
DROP TABLE [dbo].[SongReport]

-- --------------------------------------------------
-- INSERT TABLE
-- --------------------------------------------------
-- sql server insert role table
INSERT INTO [dbo].[Role]
    ([roleName])
VALUES
    ('Admin'),
    ('User')
-- sql server insert account table
INSERT INTO [dbo].[Account]
    ([userName],[hashedPassword],[email])
VALUES
    ('johndoe', 'admin', 'johnd@mail.com'),
    ('janedoe', 'user', 'janed@mail.com')
-- sql server insert account role table
INSERT INTO [dbo].[AccountRole]
    ([accountId],[roleId])
VALUES
    (1, 1),
    (2, 2)
-- sql server insert song table
INSERT INTO [dbo].[Song]
    ([songName],[author],[bpm],[notes],[releaseDate],[isPublic],[isDeleted],[deletedReason])
VALUES
    ('song1', 'author1', 100, 'notes1', '2020-01-01 00:00:00', 0, 0, NULL),
    ('song2', 'author2', 200, 'notes2', '2020-01-02 00:00:00', 1, 0, NULL),
    ('song3', 'author3', 300, 'notes3', '2020-01-03 00:00:00', 0, 1, 'reason3'),
    ('song4', 'author4', 400, 'notes4', '2020-01-04 00:00:00', 1, 1, 'reason4'),
    ('song5', 'author5', 500, 'notes5', '2020-01-05 00:00:00', 0, 0, NULL),
    ('song6', 'author6', 600, 'notes6', '2020-01-06 00:00:00', 1, 0, NULL),
    ('song7', 'author7', 700, 'notes7', '2020-01-07 00:00:00', 0, 1, 'reason7'),
    ('song8', 'author8', 800, 'notes8', '2020-01-08 00:00:00', 1, 1, 'reason8'),
    ('song9', 'author9', 900, 'notes9', '2020-01-09 00:00:00', 0, 0, NULL)
-- sql server insert tag table
INSERT INTO [dbo].[Tag]
    ([tagName])
VALUES
    ('Tag1'),
    ('Tag2'),
    ('Tag3'),
    ('Tag4'),
    ('Tag5'),
    ('Tag6'),
    ('Tag7'),
    ('Tag8'),
    ('Tag9')
-- sql server insert song tag table
INSERT INTO [dbo].[SongTag]
    ([songId],[tagId])
VALUES
    (1, 1),
    (1, 2),
    (2, 3),
    (2, 4),
    (4, 5),
    (5, 6),
    (7, 7),
    (9, 8),
    (9, 9)
-- sql server insert account song table
INSERT INTO [dbo].[AccountSong]
    ([accountId],[songId],[bestScore])
VALUES
    (1, 1, 100),
    (1, 2, 200),
    (1, 3, 300),
    (1, 4, 400),
    (1, 5, 500),
    (1, 6, 600),
    (1, 7, 700),
    (1, 8, 800),
    (1, 9, 900),
    (2, 1, 100),
    (2, 2, 200),
    (2, 3, 300),
    (2, 4, 400),
    (2, 5, 500),
    (2, 6, 600),
    (2, 7, 700),
    (2, 8, 800),
    (2, 9, 900)
-- sql server insert song report table
INSERT INTO [dbo].[SongReport]
    ([songId],[accountId],[reportTitle],[reportReason],[reportDate])
    VALUES
    (1, 2, 'report2', 'report2', '2020-01-02 00:00:00'),
    (2, 1, 'report17', 'report17', '2020-01-08 00:00:00')