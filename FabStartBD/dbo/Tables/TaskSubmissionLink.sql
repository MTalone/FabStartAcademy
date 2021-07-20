CREATE TABLE [dbo].[TaskSubmissionLink]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	[URL] varchar(250) NOT NULL,
	[TaskSubmissionID] INT NOT NULL,
	 CONSTRAINT [FK_TaskSubmissionLink_TaskSubmission] FOREIGN KEY ([TaskSubmissionID]) REFERENCES [TaskSubmission]([ID]),
)
