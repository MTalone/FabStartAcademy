CREATE TABLE [dbo].[Program] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [ProcessID]   INT           NULL,
    [Code]        VARCHAR (10)  NULL,
    [LogoID]      INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Group_Document] FOREIGN KEY ([LogoID]) REFERENCES [dbo].[Document] ([Id]),
    CONSTRAINT [FK_Group_Program] FOREIGN KEY ([ProcessID]) REFERENCES [dbo].[Process] ([Id])
);

