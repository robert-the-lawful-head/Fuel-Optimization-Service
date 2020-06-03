UPDATE ca
SET ca.AddedFrom = 1
FROM FUELERLINX_Companies co
INNER JOIN Customers c ON co.OID = c.FuelerlinxID
INNER JOIN FUELERLINX_custData cd ON co.OID = cd.CompanyId AND cd.PrimaryAccount = 1
INNER JOIN FUELERLINX_UserAircraft ua ON ua.CustId = cd.OID AND ua.CompanyId = co.OID
INNER JOIN FUELERLINX_acData2 ad ON ua.TailNumID = ad.OID
INNER JOIN CustomerAircrafts ca ON ca.TailNumber = ad.TailNo
WHERE co.Active = 1
     AND ISNULL(ca.AddedFrom, 0) <> 1
	 AND ca.GroupID <> 1;
