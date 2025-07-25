﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.Group
{
    public class IntegrationPartnersSpecification : Specification<Models.IntegrationPartners>
    {
        public IntegrationPartnersSpecification(int oid) : base(x => x.Oid == oid)
        {
        }
    }
}
