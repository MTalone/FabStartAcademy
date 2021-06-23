CREATE TABLE [dbo].[Tool] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (100) NOT NULL,
    [Url]  VARCHAR (250) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

