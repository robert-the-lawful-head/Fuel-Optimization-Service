GO
use [paragon_test]
update fp
set EffectiveTo = dateadd(dd, 1, fp.EffectiveTo)
from fbos f
inner join fboairports fa on fa.fboid = f.oid
inner join fboprices fp on fp.fboid = f.oid
inner join [group] g on g.oid = f.groupid
where isnull(g.IsFBONetwork, 0) = 0
and fp.EffectiveTo is not null
and fp.EffectiveTo > '1/1/2020'
and fp.Effectiveto < '9999-12-31'
GO