use paragon_test

ALTER TABLE fbopreferences
ADD EnableJetA bit;

ALTER TABLE fbopreferences
ADD EnableSaf bit;


update fp
set fp.enablejeta=1, fp.enablesaf=1
from fbos f
inner join fbopreferences fp on f.oid=fp.fboid and f.groupid>1