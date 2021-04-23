CREATE TABLE [dbo].[Member]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [Email] VARCHAR(50) NULL, 
    [IsUser] BIT NULL,
	--UserID
)
