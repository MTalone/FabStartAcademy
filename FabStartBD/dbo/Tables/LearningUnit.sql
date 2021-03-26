CREATE TABLE [dbo].[LearningUnit] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [Title]     VARCHAR (100) NOT NULL,
    [Active]    BIT NOT NULL,
    [Teaser]    VARCHAR (225),
    [Content]   VARCHAR (4000),
    [CategoryID] INT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_LearningUnit_LearningCategory] FOREIGN KEY ([CategoryID]) REFERENCES [LearningCategory]([ID])
    );
