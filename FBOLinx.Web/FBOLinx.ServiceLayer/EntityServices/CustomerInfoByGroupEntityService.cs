using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FBOLinx.DB.Context;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class CustomerInfoByGroupEntityService : FBOLinxBaseEntityService<DB.Models.CustomerInfoByGroup, DTO.CustomerInfoByGroupDTO, int>, IEntityService<DB.Models.CustomerInfoByGroup, DTO.CustomerInfoByGroupDTO, int>
    {
        public CustomerInfoByGroupEntityService(IMapper mapper, FboLinxContext context) : base(mapper, context)
        {
        }
    }
}
