USE [paragon_test]
GO

ALTER TABLE AirportWatchDistinctBoxes
ADD Latitude nvarchar(50) NULL

ALTER TABLE AirportWatchDistinctBoxes
ADD Longitude nvarchar(50) NULL
GO