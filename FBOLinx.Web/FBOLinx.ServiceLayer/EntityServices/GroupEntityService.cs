using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FBOLinx.DB.Context;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class GroupEntityService : FBOLinxBaseEntityService<DB.Models.Group, DTO.GroupDTO, int>, IEntityService<DB.Models.Group, DTO.GroupDTO, int>
    {
        public GroupEntityService(IMapper mapper, FboLinxContext context) : base(mapper, context)
        {
        }
    }
}
