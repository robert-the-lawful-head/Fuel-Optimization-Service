using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class CustomerEntityService : FBOLinxBaseEntityService<DB.Models.Customers, DTO.CustomerDTO, int>, IEntityService<DB.Models.Customers, DTO.CustomerDTO, int>
    {
        public CustomerEntityService(IMapper mapper, FboLinxContext context) : base(mapper, context)
        {
        }
    }
}
