USE [paragon_test]
GO

ALTER TABLE AirportWatchDistinctBoxes
ADD Latitude float not null
DEFAULT 0;

ALTER TABLE AirportWatchDistinctBoxes
ADD Longitude float not null
DEFAULT 0;
GO