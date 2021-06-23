﻿CREATE TABLE [dbo].[Process] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [LogoID]      INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Program_Document] FOREIGN KEY ([LogoID]) REFERENCES [dbo].[Document] ([Id])
);

