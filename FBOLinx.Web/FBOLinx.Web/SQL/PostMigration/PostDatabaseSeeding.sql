update cg
set CustomerCompanyType = cct.OID
from CustomerInfoByGroup cg
inner join CustomerCompanyTypes cct on cct.Name = case when cg.CustomerType = 2 then 'Base Tenant'
                                                       when cg.CustomerType = 3 then 'Transient'
                                                       when cg.CustomerType = 4 then 'Contract Fuel Vendor'
                                                       else '' end
GO
--Ask chau about fuel vendors