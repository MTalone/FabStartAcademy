CREATE TABLE [dbo].[LearningUnit] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [Title]     NVARCHAR (100) NOT NULL,
    [Active]    BIT NOT NULL,
    [Teaser]    NVARCHAR (225),
    [Content]   NVARCHAR (4000),
    [CategoryID] INT NULL, 
    [LogoID] INT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_LearningUnit_LearningCategory] FOREIGN KEY ([CategoryID]) REFERENCES [LearningCategory]([ID]), 
    CONSTRAINT [FK_LearningUnit_Document] FOREIGN KEY ([LogoID]) REFERENCES [Document]([Id]) 
    );
