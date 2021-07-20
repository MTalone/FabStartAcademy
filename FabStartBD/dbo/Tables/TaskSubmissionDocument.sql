CREATE TABLE [dbo].[TaskSubmissionDocument]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	[TaskSubmissionID] INT NOT NULL,
	[DocumentID] INT NOT NULL,
	[TeamID] INT NOT NULL,
	[ProcessID] INT NOT NULL,
	CONSTRAINT [FK_TaskSubmissionDocument_TaskSubmission] FOREIGN KEY ([TaskSubmissionID]) REFERENCES [TaskSubmission]([ID]),
	CONSTRAINT [FK_TaskSubmissionDocument_Document] FOREIGN KEY ([DocumentID]) REFERENCES [Document]([ID]),
	CONSTRAINT [FK_TaskSubmissionDocument_Process] FOREIGN KEY ([ProcessID]) REFERENCES [Process]([ID]),
	CONSTRAINT [FK_TaskSubmissionDocument_Team] FOREIGN KEY ([TeamID]) REFERENCES [Team]([ID]),
)
