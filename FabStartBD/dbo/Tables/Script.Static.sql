/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
USE [FabStartDB]
GO

IF(NOT EXISTS(Select * from Role))
BEGIN
	INSERT INTO [dbo].[Role]([Name],[IsAdmin])VALUES('Admin',1)
	INSERT INTO [dbo].[Role]([Name],[IsAdmin])VALUES('Mentor',0)
	INSERT INTO [dbo].[Role]([Name],[IsAdmin])VALUES('Member',0)
	INSERT INTO [dbo].[Role]([Name],[IsAdmin])VALUES('SuperAdmin',1)
END
GO



GO
IF(NOT EXISTS(Select * from [ActivityType]))
BEGIN
INSERT INTO [dbo].[ActivityType]([Type]) VALUES('Document')
INSERT INTO [dbo].[ActivityType]([Type]) VALUES('Submission')
INSERT INTO [dbo].[ActivityType]([Type]) VALUES('Message')
INSERT INTO [dbo].[ActivityType]([Type]) VALUES('Comment')
INSERT INTO [dbo].[ActivityType]([Type]) VALUES('Evaluation')
INSERT INTO [dbo].[ActivityType]([Type]) VALUES('Completed')
END
GO

USE [FabStartDB]
GO
IF(NOT EXISTS(Select * from TaskStatus))
BEGIN
INSERT INTO [dbo].[TaskStatus]([Label]) VALUES ('Status_Pending')
INSERT INTO [dbo].[TaskStatus]([Label]) VALUES ('Status_Started')
INSERT INTO [dbo].[TaskStatus]([Label]) VALUES ('Status_Submitted')
INSERT INTO [dbo].[TaskStatus]([Label]) VALUES ('Status_InReview')
INSERT INTO [dbo].[TaskStatus]([Label]) VALUES ('Status_Completed')
END
GO

