CREATE TABLE [dbo].[Member]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [Email] VARCHAR(50) NULL, 
    [IsUser] BIT NULL,
	[PartnerID] INT NOT NULL,
	[FirstName] NVARCHAR(450) NULL,
	[LastName] NVARCHAR(450) NULL,
	[UserId] NVARCHAR(450) NULL,
	[CreatedByID] NVARCHAR(450)  NULL,
	CONSTRAINT [FK_Member_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
	CONSTRAINT [FK_Member_Partner] FOREIGN KEY ([PartnerID]) REFERENCES [dbo].[Partner] ([Id]),
	CONSTRAINT [FK_Member_CreatedBy] FOREIGN KEY ([CreatedByID]) REFERENCES [dbo].[AspNetUsers] ([Id]),
)
