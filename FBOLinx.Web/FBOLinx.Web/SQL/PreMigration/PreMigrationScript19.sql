CREATE TABLE [dbo].[RefreshTokens] (
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[AccessTokenID] [int] NOT NULL,
	[Token] [varchar](255) NOT NULL,
	[Expired] [datetime2](7) NULL,
	[UserID] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	CONSTRAINT PK_RefreshTokens PRIMARY KEY (OID)
)
GO

CREATE TABLE [dbo].[AccessTokens](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[AccessToken] [nvarchar](255) NOT NULL,
	[Expired] [datetime] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	CONSTRAINT PK_AccessTokens PRIMARY KEY (OID)
)
GO
