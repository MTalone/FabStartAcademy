﻿CREATE TABLE [dbo].[Document] (
    [Id]       INT             IDENTITY (1, 1) NOT NULL,
    [FileName] VARCHAR (250)   NOT NULL,
    [FileType] VARCHAR (100)   NOT NULL,
    [Content]  VARBINARY (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
