CREATE TABLE [dbo].[TeamMember]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	[TeamID] INT NOT NULL, 
    [MemberID] INT NOT NULL,
	[RoleID] INT NOT NULL,
    [IsConfirmed] BIT NOT NULL,

)
