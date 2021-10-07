USE [fileStorage]
GO

/****** Object:  Table [dbo].[FuelerLinxImageFileData]    Script Date: 3/16/2021 9:08:48 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FboLinxImageFileData](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[FileData] [varbinary](max) NULL,
	[FileName] [varchar](200) NULL,
	[ContentType] [varchar](100) NULL,
	[FboId] int NULL,
 CONSTRAINT [PK_FboLinxImageFileData] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE NONCLUSTERED INDEX [INX_FboLinxImageFileData_FboId] ON [dbo].[FboLinxImageFileData]
(
	FboId ASC
)
INCLUDE([OID], FileData, ContentType, [FileName]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
use [paragon_test]
GO
ALTER TABLE AirportWatchHistoricalData
ADD AirportICAO varchar(255) null
GO
CREATE NONCLUSTERED INDEX [INX_AirportWatchHistoricalData_ICAO_PositionDate]
ON [dbo].[AirportWatchHistoricalData] ([AirportICAO],[AircraftPositionDateTimeUtc])
INCLUDE ([AircraftHexCode],[AtcFlightNumber],[AircraftTypeCode],[AircraftStatus])
GO
TRUNCATE TABLE AirportWatchHistoricalData;
TRUNCATE TABLE AirportWatchChangeTracker;
TRUNCATE TABLE AirportWatchAircraftTailNumber;
TRUNCATE TABLE AirportWatchLiveData;
GO
/****** Object:  StoredProcedure [dbo].[up_Update_FuelerlinxCompany]    Script Date: 3/10/2021 2:58:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mike Mieglitz
-- Create date: 02/27/2013
-- Description:	Update a company tied to Fuelerlinx based on their account status
--*********************************************
--              Change History
--*********************************************
--  09/24/2013  CL  Added in queries to add CustomerInfoByGroup records if company was deleted while they were deactivated from FuelerLinx; add in current aircrafts
--  09/26/2013  CL	Changed query to use FuelerLinxID instead of FBOlinxID when updating Customers table so that it'd grab records for Paragon/Generic FBOlinx, in case there were duplicates 
--  10/09/2013  CL	Changed Distribute flag from 0 to 1 when adding new FuelerLinx customers
--  10/29/2013  CL	Added in an update for Company for CustomerInfoByGroup
--  11/06/2013	CL	Added COALESCE(@Company, CG.Company) when updating Company name to prevent blanks for inserting/updating
--  11/18/2013  CL	Set Distribute = 1 when updating current FuelerLinx customers
--	01/16/2014	CL	Added in AddedFrom column for CustomerAircrafts query
--	08/01/2017	CL	Changed SP to use local variables instead
--	11/17/2017	CL	Added and @Local_FuelerlinxCompanyID <> 0
-- =============================================
ALTER PROCEDURE [dbo].[up_Update_FuelerlinxCompany]
	@FuelerlinxCompanyID int,
	@FBOLinxCompanyID int = null,
	@Company varchar(50) = null
AS
BEGIN
	declare @Local_FuelerlinxCompanyID int
	declare @Local_FBOLinxCompanyID int
	declare @Local_Company varchar(50)

	SET @Local_FuelerlinxCompanyID = @FuelerlinxCompanyID
	SET @Local_FBOLinxCompanyID = @FBOLinxCompanyID
	SET @Local_Company = @Company

	if (@Local_FBOLinxCompanyID is null)
		SELECT @Local_FBOLinxCompanyID = OID FROM Customers WHERE FuelerlinxID = @Local_FuelerlinxCompanyID OR FuelerLinxID = -@Local_FuelerlinxCompanyID
	
	IF (ISNULL(@Local_FBOLinxCompanyID, 0) > 0 and @Local_FuelerlinxCompanyID <> 0)
		BEGIN
			IF (EXISTS(SELECT OID FROM FUELERLINX_custData WHERE active = 1 and companyID = @Local_FuelerlinxCompanyID))
				BEGIN
					IF (@Local_FBOLinxCompanyID > 0)
						UPDATE Customers
						SET FuelerlinxID = @Local_FuelerlinxCompanyID
						WHERE FuelerLinxID = -@Local_FuelerlinxCompanyID
						
						UPDATE CG
						SET Distribute = 1, CustomerType = 1, Company = COALESCE(@Local_Company, CG.Company)
						FROM CustomerInfoByGroup CG
						INNER JOIN Customers C 
						ON C.FuelerlinxID = @Local_FuelerlinxCompanyID
						and C.OID = CG.CustomerID
						
						INSERT INTO CustomerInfoByGroup (GroupID, CustomerID, Company, Active, Distribute, Network, ShowJetA, Show100LL, Suspended, CustomerType, CertificateType)
						SELECT G.OID, @Local_FBOLinxCompanyID, @Local_Company, 1, 1, 0, 1, 0, 0, 1, CustCertificateType
						FROM [Group] G
						LEFT JOIN 
							(select CIBG.*, C.CertificateType as CustCertificateType from CustomerInfoByGroup CIBG
							 inner join Customers C on C.OID = CIBG.CustomerID and C.FuelerlinxID = @Local_FuelerlinxCompanyID) CC ON G.OID = CC.GroupID
						WHERE CC.CustomerID IS NULL
						
						INSERT INTO CustomerAircrafts (TailNumber, CustomerID, AircraftID, Size, GroupID, AddedFrom)
						SELECT AC.tailNo, C.OID, AC.AircraftID, AC.Size, CG.GroupID, 1
						FROM Customers C
						INNER JOIN CustomerInfoByGroup CG ON CG.CustomerID = C.OID
						INNER JOIN FUELERLINX_UserAircraft UA ON UA.CompanyID = @Local_FuelerlinxCompanyID
						INNER JOIN FUELERLINX_acData2 AC ON AC.OID = UA.TailNumID
						LEFT JOIN CustomerAircrafts CA ON CA.TailNumber = AC.tailNo
																		AND C.OID = CA.CustomerID
																		AND CA.GroupID = CG.GroupID
						WHERE C.FuelerlinxID = @Local_FuelerlinxCompanyID
							AND CA.OID IS NULL
						GROUP BY AC.tailNo, C.OID, AC.AircraftID, AC.Size, CG.GroupID
				END
			ELSE
				BEGIN
					UPDATE CG
					SET Distribute = 1, CustomerType = 3
					FROM CustomerInfoByGroup CG
					INNER JOIN Customers C 
					ON C.FuelerlinxID = @Local_FuelerlinxCompanyID
					and C.OID = CG.CustomerID
				
					UPDATE Customers
					SET FuelerlinxID = -@Local_FuelerlinxCompanyID,
						Distribute = 1
					WHERE FuelerlinxID = @Local_FuelerlinxCompanyID
				END
		END
END
GO
