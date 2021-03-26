CREATE TABLE [dbo].[LearningCategory] (
    [Id]       INT IDENTITY (1, 1) NOT NULL,
    [CategoryID]     INT NOT NULL,
    [UnitID]     INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [LearningUnitToCategoryConnection] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[LearningCategory] ([Id])
);