use paragon_test

update fp
set fp.enablesaf=0
from fbos f
inner join fbopreferences fp on f.oid=fp.fboid and f.groupid>1