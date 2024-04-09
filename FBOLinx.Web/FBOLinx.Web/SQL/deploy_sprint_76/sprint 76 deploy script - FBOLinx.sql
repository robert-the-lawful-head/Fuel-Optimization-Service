use [paragon_test]
GO
ALTER TABLE EmailContent
ADD GroupId INT;

ALTER TABLE EmailContent
ADD FromAddress VARCHAR(255);
GO
USE [fileStorage]
GO

ALTER TABLE [dbo].[FboLinxImageFileData]
ADD GroupId INT NULL;

CREATE NONCLUSTERED INDEX [INX_FboLinxImageFileData_GroupId] ON [dbo].[FboLinxImageFileData]
(
	GroupId ASC
)
INCLUDE([OID], FileData, ContentType, [FileName]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO