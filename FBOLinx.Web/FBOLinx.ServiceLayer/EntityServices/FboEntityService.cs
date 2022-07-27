using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IFboEntityService : IRepository<Fbos, FboLinxContext>
    {

    }

    public class FboEntityService : Repository<Fbos, FboLinxContext>, IFboEntityService
    {
        public FboEntityService(FboLinxContext context) : base(context)
        {
        }
    }
}
