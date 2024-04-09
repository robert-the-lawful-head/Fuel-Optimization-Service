use [paragon_test]
GO
ALTER TABLE airportwatchdistinctboxes
ALTER COLUMN latitude float;

ALTER TABLE airportwatchdistinctboxes
ALTER COLUMN longitude float;

update airportwatchdistinctboxes
set AirportICAO=null,latitude=43.56702,longitude=-80.26423
where boxname='cykf_a01'

update airportwatchdistinctboxes
set AirportICAO=null,latitude=43.38238,longitude=-92.19945
where boxname='kcjj_a01'

update airportwatchdistinctboxes
set AirportICAO=null,latitude=39.6982,longitude=-104.93345
where boxname='kden_a01'

update airportwatchdistinctboxes
set AirportICAO=null,latitude=38.3198,longitude=-80.6087
where boxname='klwb_a01'

update airportwatchdistinctboxes
set AirportICAO=null,latitude=32.93811,longitude=-117.10014
where boxname='knkx_a01'

update airportwatchdistinctboxes
set AirportICAO=null,latitude=34.19,longitude=-117.05
where boxname='kont_a01'

update airportwatchdistinctboxes
set AirportICAO=null,latitude=45.65,longitude=-122.50
where boxname='kpdx_a01'

update airportwatchdistinctboxes
set AirportICAO=null,latitude=34.07160,longitude=-118.46374
where boxname='ksmo_a01'
GO
