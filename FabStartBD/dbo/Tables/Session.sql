CREATE TABLE [dbo].[Session] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (100) NOT NULL,
    [Description] VARCHAR (MAX) NOT NULL,
    [ProcessID]   INT           NOT NULL,
    [Order] INT NOT NULL DEFAULT 0, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Session_Program] FOREIGN KEY ([ProcessID]) REFERENCES [dbo].[Process] ([Id])
);

