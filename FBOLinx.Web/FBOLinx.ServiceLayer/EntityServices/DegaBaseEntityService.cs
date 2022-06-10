using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class DegaBaseEntityService<T, TDTO, TIDType>: BaseEntityService<T, TDTO, TIDType> where T : FBOLinxBaseEntityModel<TIDType> where TDTO : IEntityModelDTO<T, TIDType>
    {
        public DegaBaseEntityService(DegaContext context): base(context)
        {
            _Context = context;
        }
    }
}
