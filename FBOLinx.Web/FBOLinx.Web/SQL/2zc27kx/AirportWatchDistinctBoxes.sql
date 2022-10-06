USE [paragon_test]
GO

ALTER TABLE AirportWatchDistinctBoxes
ADD Latitude nvarchar(50) NULL

ALTER TABLE AirportWatchDistinctBoxes
ADD Longitude nvarchar(50) NULL
GO

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'CRKF'
WHERE BoxName = 'cykf_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KDAL'
WHERE BoxName = 'kdal_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KDEN'
WHERE BoxName = 'kden_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = NULL
WHERE BoxName = 'kden_a02'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KFRG'
WHERE BoxName = 'kfrg_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KLWB'
WHERE BoxName = 'klwb_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KNKX'
WHERE BoxName = 'knkx_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KOAK'
WHERE BoxName = 'koak_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KOPF'
WHERE BoxName = 'kopf_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KPDX'
WHERE BoxName = 'kpae_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KPWK'
WHERE BoxName = 'kpwk_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KRNO'
WHERE BoxName = 'krno_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = NULL
WHERE BoxName = 'kryy_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KSBA'
WHERE BoxName = 'ksba_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = NULL
WHERE BoxName = 'ksmo_a03'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = NULL
WHERE BoxName = 'ksna_a02'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KTOA'
WHERE BoxName = 'ktoa_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = NULL
WHERE BoxName = 'mbpv_a01'