CREATE TABLE [dbo].[TaskSubmissionComment]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [TaskSubmissionID] INT NOT NULL, 
    [Comment] NVARCHAR(500) NOT NULL, 
    [MemberID] INT NOT NULL, 
    [CreatedOn] Datetime,
    CONSTRAINT [FK_TaskSubmissionComment_Member] FOREIGN KEY ([MemberID]) REFERENCES [Member]([ID]),
    CONSTRAINT [FK_TaskSubmissionComment_TaskSubmission] FOREIGN KEY ([TaskSubmissionID]) REFERENCES [TaskSubmission]([ID]),
)
