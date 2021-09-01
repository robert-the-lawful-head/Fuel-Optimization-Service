use [paragon_test]
GO
CREATE TABLE [dbo].[CustomerTag](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[GroupID] [int] NOT NULL,
	[CustomerID] [int] NULL,
	[Name] [varchar](100) NOT NULL
 CONSTRAINT [PK_CustomerTag] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO [dbo].[CustomerTag]
           ([GroupID],[CustomerID],[Name])
     VALUES
           (0,0,'Contract Fuel Vendor'),
		   (0,0,'Base'),
		   (0,0,'CAA'),
		   (0,0,'FuelerLinx'),
		   (0,0,'Transient')
GO


INSERT INTO [dbo].[CustomerTag]
           ([GroupID]
           ,[CustomerID]
		    ,[Name])
     SELECT [b].[GroupID]
      ,[b].[CustomerID]
	  ,'FuelerLinx'
  FROM [dbo].[CustomerInfoByGroup] as [b]
  INNER JOIN [dbo].[Customers] as [c] 
  ON [c].[OID] = [b].[CustomerID]
  where [c].[FuelerlinxID] > 0
GO
