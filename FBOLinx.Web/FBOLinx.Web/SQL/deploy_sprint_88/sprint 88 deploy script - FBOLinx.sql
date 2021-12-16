use paragon_test
GO
/****** Object:  Table [dbo].[AircraftHexTailMapping]    Script Date: 12/13/2021 11:33:12 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AircraftHexTailMapping](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[AircraftHexCode] [varchar](25) NOT NULL,
	[TailNumber] [varchar](25) NULL,
 CONSTRAINT [PK_AircraftHexTailMapping] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FAAAircraftHexTailMapping]    Script Date: 12/13/2021 11:36:18 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FAAAircraftHexTailMapping](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[AircraftHexCode] [varchar](25) NOT NULL,
	[TailNumber] [varchar](25) NULL,
 CONSTRAINT [PK_FAAAircraftHexTailMapping] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
insert into AircraftHexTailMapping (AircraftHexCode, TailNumber)
select f.AircraftHexCode, f.TailNumber
from FAAAircraftHexTailMapping f
left join AircraftHexTailMapping a on a.AircraftHexCode = f.AircraftHexCode
where a.oid is null
and isnull(f.TailNumber, '') <> ''
and isnull(f.AircraftHexCode, '') <> ''
group by f.AircraftHexCode, f.TailNumber

update a
set a.TailNumber = a.TailNumber
from AircraftHexTailMapping a
inner join FAAAircraftHexTailMapping f on f.AircraftHexCode = a.aircraftHexCode
where isnull(f.TailNumber, '') <> ''
and isnull(f.AircraftHexCode, '') <> ''
GO