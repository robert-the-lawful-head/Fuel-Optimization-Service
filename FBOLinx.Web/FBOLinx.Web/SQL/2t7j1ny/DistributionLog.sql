USE [paragon_Test]
GO

delete dl
from distributionlog dl
left join distributionqueue dq on dq.distributionlogid=dl.oid
where dq.distributionlogid is null
GO