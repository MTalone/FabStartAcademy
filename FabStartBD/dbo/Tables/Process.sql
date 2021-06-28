CREATE TABLE [dbo].[Process] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [LogoID]      INT           NULL,
    [PartnerID]   INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Process_Document] FOREIGN KEY ([LogoID]) REFERENCES [dbo].[Document] ([Id]),
    CONSTRAINT [FK_Process_Partner] FOREIGN KEY ([PartnerID]) REFERENCES [dbo].[Partner] ([Id])
);

