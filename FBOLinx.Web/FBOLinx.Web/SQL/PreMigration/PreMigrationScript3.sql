ALTER TABLE FBOs
	ADD AcukwikFBOHandlerID int
GO

update f
set f.AcukwikFBOHandlerID = af.Handler_ID
from FBOs f
inner join fboairports fa on fa.fboid = f.oid
inner join dega.dbo.AcukwikAirports aa on aa.icao = fa.icao
inner join dega.dbo.AcukwikFBOHandlerDetail af on af.Airport_ID = aa.AirporT_ID and dega.dbo.fn_FBOs_Equivalent(f.Fbo, af.HandlerLongName) = 1
GO

update f
set f.AcukwikFBOHandlerID = af.AcukwikFBOHandlerID
from FBOs f
inner join fboairports fa on fa.fboid = f.oid
inner join sqlfl.dbo.AcukwikFBOHandlerLookup af on af.FBOHandlerName = f.Fbo and fa.ICAO = af.ICAO
GO


/****** Object:  Table [dbo].[IntegrationPartners]    Script Date: 12/13/2019 8:38:57 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IntegrationPartners](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[PartnerName] [varchar](255) NOT NULL,
	[PartnerType] [smallint] NOT NULL,
	[Affiliation] [smallint] NOT NULL,
	[APIKey] [uniqueidentifier] NOT NULL,
	[CreationDate] [datetime2](7) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[PartnerId] [uniqueidentifier] NULL,
	[TrustLevel] [smallint] NULL,
 CONSTRAINT [PK_IntegrationPartners] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


insert into IntegrationPartners (PartnerName, PartnerType, Affiliation, APIKey, CreationDate, PartnerId, TrustLevel)
values ('FuelerLinx Internal', 7, 0, 'C82FFE95-848B-46AB-A8D6-5116BFA2AB07', GETDATE(), 'EEE1E4EF-DAD7-488F-BBAC-2E6530B9DDB1', 3)
GO
