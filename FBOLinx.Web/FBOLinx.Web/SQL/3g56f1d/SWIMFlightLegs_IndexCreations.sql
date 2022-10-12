use [Dega]
GO

DROP INDEX [INX_SWIMFlightLegs_DepartureICAO] ON [dbo].[SWIMFlightLegs]
GO

/****** Object:  Index [INX_SWIMFlightLegData_MessageTimestamp]    Script Date: 10/12/2022 10:18:00 AM ******/
CREATE NONCLUSTERED INDEX [INX_SWIMFlightLegs_DepartureICAO] ON [dbo].[SWIMFlightLegs]
(
	DepartureICAO
)
INCLUDE([ETA],ATD) 
GO

DROP INDEX [INX_SWIMFlightLegs_ArrivalICAO] ON [dbo].[SWIMFlightLegs]
GO

/****** Object:  Index [INX_SWIMFlightLegData_MessageTimestamp]    Script Date: 10/12/2022 10:18:00 AM ******/
CREATE NONCLUSTERED INDEX [INX_SWIMFlightLegs_ArrivalICAO] ON [dbo].[SWIMFlightLegs]
(
	ArrivalICAO
)
INCLUDE([ETA],ATD) 
GO

DROP INDEX [INX_SWIMFlightLegs_AircraftIdentification_ATD] ON [dbo].[SWIMFlightLegs]
GO

CREATE NONCLUSTERED INDEX [INX_SWIMFlightLegs_AircraftIdentification_ATD] ON [dbo].[SWIMFlightLegs]
(
	[AircraftIdentification],
	ATD
)
INCLUDE([IsPlaceholder]) 
GO