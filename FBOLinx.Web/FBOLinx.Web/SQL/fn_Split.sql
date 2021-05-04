/****** Object:  UserDefinedFunction [dbo].[fn_Split]    Script Date: 4/15/2021 3:55:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fn_Split]
(
@List varchar(max),
@SplitOn varchar(5)
) 
RETURNS @RtnValue table 
(
ID int identity(1,1),
Value varchar(500)
) 
AS 
/******************************************************************************
** Name: fn_Split
** Desc: Returns table from comma-separated list
**
** Auth: Mike Mieglitz
** Date: 06/4/08
*******************************************************************************
** Change History
*******************************************************************************
** Date: Author: Description:
** -------- -------- ------------------------------------------- 
**
*******************************************************************************/
BEGIN

	WHILE (CHARINDEX(@SplitOn,@List)>0)
	BEGIN 
		INSERT INTO @RtnValue (value)
		SELECT Value = LTrim(RTRIM(SUBSTRING(@List,1,CHARINDEX(@SplitOn, @List) - 1))) 
		SET @List = SUBSTRING(@List,CHARINDEX(@SplitOn, @List) + LEN(@SplitOn), LEN(@List))
	END 
	INSERT INTO @RtnValue (Value)
	SELECT Value = LTRIM(RTRIM(@List))

	RETURN

END
GO


