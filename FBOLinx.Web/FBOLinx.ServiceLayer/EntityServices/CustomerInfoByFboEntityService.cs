using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface ICustomerInfoByFboEntityService : IRepository<DB.Models.CustomerInfoByFbo,
        DB.Context.FboLinxContext>
    {
    }

    public class CustomerInfoByFboEntityService :
        Repository<DB.Models.CustomerInfoByFbo, DB.Context.FboLinxContext>,
        ICustomerInfoByFboEntityService
    {
        public CustomerInfoByFboEntityService(DB.Context.FboLinxContext context) : base(context)
        {
        }
    }
}
