USE [Dega]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_Calc_Distance_Miles]    Script Date: 3/30/2020 6:44:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER function [dbo].[fn_Calc_Distance_Miles] (@Lat1 decimal(8,4), @Long1 decimal(8,4), @Lat2 decimal(8,4), @Long2 decimal(8,4))
returns decimal (8,4) as
begin
declare @d decimal(28,10)
-- Convert to radians
set @Lat1 = @Lat1 / 57.2958
set @Long1 = @Long1 / 57.2958
set @Lat2 = @Lat2 / 57.2958
set @Long2 = @Long2 / 57.2958
-- Calc distance
set @d = (Sin(@Lat1) * Sin(@Lat2)) + (Cos(@Lat1) * Cos(@Lat2) * Cos(@Long2 - @Long1))
-- Convert to miles
if @d <> 0
begin
set @d = 3958.75 * Atan(Sqrt(1 - power(@d, 2)) / @d);
end
return @d
end 