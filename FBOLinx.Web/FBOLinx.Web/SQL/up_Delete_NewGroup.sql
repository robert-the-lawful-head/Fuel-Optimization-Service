USE [paragon_test]
GO
/****** Object:  StoredProcedure [dbo].[up_Delete_NewGroup]    Script Date: 7/13/2022 1:43:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[up_Delete_NewGroup]
@GroupID int
as

BEGIN

IF (@GroupID = 1)
	BEGIN

		DELETE FROM FBOs WHERE GroupID = @GroupID

		DELETE FROM [Group] WHERE OID = @GroupID

		DELETE FROM CustomerInfoByGroup WHERE GroupID = @GroupID

		DELETE FROM customerAircrafts WHERE GroupID = @GroupID

		DELETE FROM AdminEmails WHERE GroupID = @GroupID

		--DELETE FROM sqlfl.dbo.fuelerList WHERE FBOLinxID = @GroupID

		--DELETE FROM sqlfl_Test.dbo.fuelerList WHERE FBOLinxID = @GroupID

	END
END
