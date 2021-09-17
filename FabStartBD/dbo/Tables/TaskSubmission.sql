CREATE TABLE [dbo].[TaskSubmission]
(
	[ID] INT NOT NULL IDENTITY PRIMARY KEY, 
    [TaskID] INT NOT NULL, 
    [TeamID] INT NOT NULL, 
    [SessionID] INT NOT NULL,
    [ProcessID] INT NOT NULL, 
    [MemberID] INT NOT NULL,  
    [Rating] INT NOT NULL DEFAULT 0, 
    [IsSubmitted] BIT NOT NULL DEFAULT 0, 
    [Text] varchar(2000) NULL,
    TaskStatusID int NOT NULL,
    CONSTRAINT [FK_TaskSubmission_Task] FOREIGN KEY ([TaskID]) REFERENCES [Task]([ID]),
    CONSTRAINT [FK_TaskSubmission_Team] FOREIGN KEY ([TeamID]) REFERENCES [Team]([ID]),
    CONSTRAINT [FK_TaskSubmission_Process] FOREIGN KEY ([ProcessID]) REFERENCES [Process]([ID]),
    CONSTRAINT [FK_TaskSubmission_Session] FOREIGN KEY ([SessionID]) REFERENCES [Session]([ID]),
    CONSTRAINT [FK_TaskSubmission_Member] FOREIGN KEY ([MemberID]) REFERENCES Member([ID]),
    CONSTRAINT [FK_TaskSubmission_TaskStatus] FOREIGN KEY ([TaskStatusID]) REFERENCES TaskStatus([ID]),
)
