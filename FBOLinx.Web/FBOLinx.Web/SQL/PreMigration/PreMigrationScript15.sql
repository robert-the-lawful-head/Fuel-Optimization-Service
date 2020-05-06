ALTER TABLE Paragon_Test.dbo.[Group]
	ADD IsLegacyAccount bit NULL
GO

update g
set g.IsLegacyAccount = 1
from [Group] g
left join [User] u on u.GroupID = g.OID
where u.oid is null

GO


USE [sqlfl]
GO
/****** Object:  StoredProcedure [dbo].[up_InsertUpdate_FuelReq]    Script Date: 5/6/2020 3:49:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--region [dbo].[up_InsertUpdate_FuelReq]

------------------------------------------------------------------------------------------------------------------------
-- Author:   Mike
-- Procedure Name: [dbo].[up_InsertUpdate_FuelReq]
-- Date Generated: Thursday, December 08, 2011
--**********************************************
--                Updates
--  DATE	  By Who	Change Made
--**********************************************
--  05/17/13    MM      #47025167: Added an insert into FBOLinx if it's a newly created transaction
--  07/11/13    MM      #53086667: Inserted the company's default fees into the otherFees table if it's a new transaction
--  08/09/13    MM      #54915074: Corrected the FBOLinx transaction insertion to use the right FBO ID, based on the ICAO + Group ID
--  08/23/13    MM      #55731402: Removed single quotes from the FBO name check
--  09/24/14    MM      #79190062: Removed wholesalecost, added FuelMasterID
--	02/12/16	CL		Added DemoMode check for insert into FBOlinx
------------------------------------------------------------------------------------------------------------------------

ALTER PROCEDURE [dbo].[up_InsertUpdate_FuelReq]
	@OID int = null,
	@member_id int,
	@AirportID int,
	@FBO varchar(500),
	@IntoPlane varchar(500) = '',
	@FuelerID int,
	@UserAircraftID int,
	@reconciled bit,
	@discrepancy bit,
	@cancelled bit,
	@tail_number nvarchar(50),
	@eta datetime,
	@etd datetime,
	@ICAO nvarchar(4),
	@IATA nvarchar(15),
	@fuel_relationship_1 nvarchar(500),
	@fuel_est_weight int,
	@fuel_est_cost float,
	@fbo_notes varchar(max),
	@creation_date datetime = null,
	@time_standard char(1),
	@invoicedWholesale float,
	@ActualPPG float,
	@ActualVolume float,
	@basePPG float,
	@postedRetail float,
	@TripID int,
	@forcedReconcile bit,
	@archived bit,
	@CompanyID int,
	@WebDispatchID int,
	@Direct bit,
	@MemoFor int,
	@DiscCorrected bit,
	@QuotedTotal float = 0,
	@DiscrepancyDiff float = 0,
	@DiscrepancyDiffTotal float = 0,
	@AcuFBO varchar(100) = '',
	@retailTotal float = 0,
	@Fueler varchar(75) = '',
	@CustomNotes bit = 0,
	@PlattsPrice float = 0,
	@IntegrationTripID int = null,
	@FuelMasterID int = 0,
	@Source smallint = 0
	
AS

BEGIN
    SET NOCOUNT ON

    IF (ISNULL(@OID, 0) > 0) AND EXISTS(SELECT [OID] FROM [dbo].[FuelReq] WHERE [OID] = @OID)
    BEGIN
	    DECLARE @MemoID int = null
    	
	    SELECT @MemoID = OID
	    FROM FuelReq
	    WHERE MemoFor = @OID

	    UPDATE [dbo].[FuelReq] SET
		    [member_id] = @member_id,
		    [AirportID] = @AirportID,
		    [FBO] = @FBO,
		    [IntoPlane] = @IntoPlane,
		    [FuelerID] = @FuelerID,
		    [UserAircraftID] = @UserAircraftID,
		    [reconciled] = @reconciled,
		    [discrepancy] = @discrepancy,
		    [cancelled] = @cancelled,
		    [tail_number] = @tail_number,
		    [eta] = @eta,
		    [etd] = @etd,
		    [ICAO] = @ICAO,
		    [IATA] = @IATA,
		    [fuel_relationship_1] = @fuel_relationship_1,
		    [fuel_est_weight] = @fuel_est_weight,
		    [fuel_est_cost] = @fuel_est_cost,
		    [fbo_notes] = @fbo_notes,
		    [creation_date] = @creation_date,
		    [time_standard] = @time_standard,
		    [invoicedWholesale] = @invoicedWholesale,
		    [ActualPPG] = @ActualPPG,
		    [ActualVolume] = @ActualVolume,
		    [basePPG] = @basePPG,
		    [postedRetail] = @postedRetail,
		    [TripID] = @TripID,
		    [forcedReconcile] = @forcedReconcile,
		    [archived] = @archived,
		    [CompanyID] = @CompanyID,
		    [WebDispatchID] = @WebDispatchID,
		    [Direct] = @Direct,
		    [MemoFor] = @MemoFor,
		    [DiscCorrected] = @DiscCorrected,
		    [CustomNotes] = @CustomNotes,
		    [PlattsPrice] = @PlattsPrice,
		    [IntegrationTripID] = @IntegrationTripID,
		    FuelMasterID = @FuelMasterID,
			[Source] = @Source
	    WHERE
		    [OID] = @OID
    		
	    IF (ISNULL(@MemoID, 0) > 0)
		    BEGIN
			    EXEC up_Transactions_InsUpdCreditMemo @OID
		    END
    END
    ELSE
    BEGIN
	    INSERT INTO [dbo].[FuelReq] (
		    [member_id],
		    [AirportID],
		    [FBO],
		    [IntoPlane],
		    [FuelerID],
		    [UserAircraftID],
		    [reconciled],
		    [discrepancy],
		    [cancelled],
		    [tail_number],
		    [eta],
		    [etd],
		    [ICAO],
		    [IATA],
		    [fuel_relationship_1],
		    [fuel_est_weight],
		    [fuel_est_cost],
		    [fbo_notes],
		    [creation_date],
		    [time_standard],
		    [invoicedWholesale],
		    [ActualPPG],
		    [ActualVolume],
		    [basePPG],
		    [postedRetail],
		    [TripID],
		    [forcedReconcile],
		    [archived],
		    [CompanyID],
		    [WebDispatchID],
		    [Direct],
		    [MemoFor],
		    [DiscCorrected],
		    [CustomNotes],
		    [PlattsPrice],
		    IntegrationTripID,
		    FuelMasterID,
			[Source]
	    ) VALUES (
		    @member_id,
		    @AirportID,
		    @FBO,
		    @IntoPlane,
		    @FuelerID,
		    @UserAircraftID,
		    @reconciled,
		    @discrepancy,
		    @cancelled,
		    @tail_number,
		    @eta,
		    @etd,
		    @ICAO,
		    @IATA,
		    @fuel_relationship_1,
		    @fuel_est_weight,
		    @fuel_est_cost,
		    @fbo_notes,
		    @creation_date,
		    @time_standard,
		    @invoicedWholesale,
		    @ActualPPG,
		    @ActualVolume,
		    @basePPG,
		    @postedRetail,
		    @TripID,
		    @forcedReconcile,
		    @archived,
		    @CompanyID,
		    @WebDispatchID,
		    @Direct,
		    @MemoFor,
		    @DiscCorrected,
		    @CustomNotes,
		    @PlattsPrice,
		    @IntegrationTripID,
		    @FuelMasterID,
			@Source
	    )
    	
	    SET @OID = @@IDENTITY
    	
	    --Insert into FBOLinx if applicable
    	
	    declare @FBOLinxID int = 0
    	
	    select @FBOLinxID = FBOLinxID
	    from fuelerList FL
	    where FL.OID = @FuelerID
    	
	    IF (@FBOLinxID > 0 AND ISNULL(@Direct, 0) = 0)
			    BEGIN
				    DECLARE @FBOLinxFBOID int = 0
    				
				    SELECT @FBOLinxFBOID = F.OID
				    FROM FBOLinx_FBOs F
				    inner join FBOLinx_FBOAirports FA ON FA.FBOID = F.OID
					INNER JOIN FBOLinx_Group G on G.OID = F.GroupID
				    WHERE REPLACE(LTRIM(RTRIM(F.Fbo)), '''', '') = REPLACE(LTRIM(RTRIM(@FBO)), '''', '')
				        AND F.GroupID = @FBOLinxID
				        AND FA.ICAO = @ICAO
						AND isnull(g.IsLegacyAccount, 0) = 1
    				
				    IF (@FBOLinxFBOID > 0)
					    BEGIN
						    INSERT INTO FBOLinx_FuelReq (FBOID, CustomerAircraftID, ETA, ETD, ICAO, Notes, QuotedPPG, QuotedVolume, TimeStandard, CustomerID, DateCreated, [Source], SourceID)
						    SELECT @FBOLinxFBOID, ISNULL(CA.OID, 0), @eta, @etd, @ICAO, @fbo_notes, @fuel_est_cost, @fuel_est_weight, @time_standard, C.OID, GETDATE(), 'Fuelerlinx', @OID
						    FROM FBOLinx_Customers C 
						    INNER JOIN FBOLinx_CustomerInfoByGroup CG ON CG.CustomerID = C.OID
																				    AND C.FuelerlinxID = @CompanyID
						    INNER JOIN FBOLinx_FBOs F ON F.GroupID = CG.GroupID
                                                                    AND F.OID = @FBOLinxFBOID
						    INNER JOIN FBOLinx_CustomerAircrafts CA ON LTRIM(RTRIM(CA.TailNumber)) = LTRIM(RTRIM(@tail_number))
																			    AND C.OID = CA.CustomerID
																			    AND CA.GroupID = CG.GroupID
					    END				
			    END	
    END

    SELECT @OID AS OID
--endregion
END
GO