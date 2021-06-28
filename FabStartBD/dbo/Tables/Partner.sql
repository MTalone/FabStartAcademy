﻿CREATE TABLE [dbo].[Partner]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	[Name] NVARCHAR (100) NOT NULL,
	[IsSuspended] BIT NOT NULL,
	[IsDeleted] BIT NOT NULL, 
    [Code] VARCHAR(10) NOT NULL,
	[IsMain] BIT NOT NULL
)
