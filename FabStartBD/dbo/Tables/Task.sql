CREATE TABLE [dbo].[Task] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (100) NOT NULL,
    [Description]  NVARCHAR (400) NOT NULL,
    [SessionID]    INT           NOT NULL,
    [TaskType]     INT           DEFAULT ((0)) NULL,
    [Instructions] VARCHAR (MAX) NULL,
    [Order]        INT           DEFAULT ((0)) NOT NULL,
    [IsEvaluated]  BIT           DEFAULT ((0)) NOT NULL,
    [DocumentID]   INT           NULL,
    [AvailableOn]  DATE          NULL,
    [ToolID]       INT           NULL,
    [ProcessID] INT NOT NULL DEFAULT 1, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Task_Document] FOREIGN KEY ([DocumentID]) REFERENCES [dbo].[Document] ([Id]),
    CONSTRAINT [FK_Task_Session] FOREIGN KEY ([SessionID]) REFERENCES [dbo].[Session] ([Id]),
    CONSTRAINT [FK_Task_Process] FOREIGN KEY ([ProcessID]) REFERENCES [dbo].[Process] ([Id]),
    CONSTRAINT [FK_Task_Tool] FOREIGN KEY ([ToolID]) REFERENCES [dbo].[Tool] ([Id])
);

