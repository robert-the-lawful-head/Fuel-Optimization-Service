/****** Object:  Table [dbo].[AuditsLogs]    Script Date: 1/11/2021 3:21:16 ã ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditsLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Type] [nvarchar](max) NULL,
	[TableName] [nvarchar](max) NULL,
	[DateTime] [datetime] NULL,
	[OldValues] [nvarchar](max) NULL,
	[NewValues] [nvarchar](max) NULL,
	[AffectedColumns] [nvarchar](max) NULL,
	[PrimaryKey] [nvarchar](max) NULL,
 CONSTRAINT [PK_AuditsLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
