delete fr
FROM fuelreq fr
    WHERE isnull(sourceid,0)>0 and datecreated>='2020-12-01 13:03:50.887' and fr.OID NOT IN
    (
        SELECT MAX(fr.OID)
        FROM fuelreq fr
		where isnull(sourceid,0)>0
        GROUP BY SourceID
    )