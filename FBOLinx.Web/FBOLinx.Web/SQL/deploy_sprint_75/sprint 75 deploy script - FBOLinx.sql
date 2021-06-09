use [paragon_test]
GO
update ec
set name=pt.name
from pricingtemplate pt
inner join emailcontent ec on ec.oid=pt.emailcontentid and isnull(ec.name,'')=''
GO
