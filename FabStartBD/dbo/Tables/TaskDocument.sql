CREATE TABLE [dbo].[TaskDocument]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [TaskID] INT NOT NULL, 
    [DocumentID] INT NOT NULL, 
    CONSTRAINT [FK_TaskDocument_Task] FOREIGN KEY (TaskID) REFERENCES [Task]([ID]),
    CONSTRAINT [FK_TaskDocument_Document] FOREIGN KEY (DocumentID) REFERENCES [Document]([ID])
)
