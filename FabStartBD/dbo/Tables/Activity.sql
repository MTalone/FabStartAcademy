CREATE TABLE [dbo].[Activity]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [TaskSubmissionID] INT NULL, 
    [ActivityTypeID] INT NOT NULL,
    [TeamID] INT NOT NULL,
    [MemberID] INT NOT NULL, 
     [CreatedOn] DATETIME NOT NULL, 
    [IsMentor] BIT NOT NULL DEFAULT 0, 
    [IsRead] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_Activity_Team] FOREIGN KEY ([TeamID]) REFERENCES [Team]([ID]),
    CONSTRAINT [FK_Activity_Member] FOREIGN KEY ([MemberID]) REFERENCES [Member]([ID]),
    CONSTRAINT [FK_Activity_ActivityType] FOREIGN KEY ([ActivityTypeID]) REFERENCES [ActivityType]([ID]),
    CONSTRAINT [FK_Activity_TaskSubmission] FOREIGN KEY ([TaskSubmissionID]) REFERENCES [TaskSubmission]([ID]),
)
