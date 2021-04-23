CREATE TABLE [dbo].[Team]
(
	[ID] INT NOT NULL IDENTITY PRIMARY KEY, 
    [Name] VARCHAR(250) NOT NULL, 
    [Description] VARCHAR(1000) NULL, 
    [LogoID] INT NULL,
    [ProgramID] INT NOT NULL, 
    
    [Code] VARCHAR(10) NULL, 
    CONSTRAINT [FK_Team_Document] FOREIGN KEY ([LogoID]) REFERENCES [dbo].[Document] ([Id]),
    CONSTRAINT [FK_Team_Group] FOREIGN KEY ([ProgramID]) REFERENCES [dbo].[Program] ([Id]),
)
