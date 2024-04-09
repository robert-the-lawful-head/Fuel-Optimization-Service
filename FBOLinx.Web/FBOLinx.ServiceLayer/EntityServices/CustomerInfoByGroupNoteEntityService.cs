using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface ICustomerInfoByGroupNoteEntityService : IRepository<DB.Models.CustomerInfoByGroupNote,
        DB.Context.FboLinxContext>
    {
    }

    public class CustomerInfoByGroupNoteEntityService :
        Repository<DB.Models.CustomerInfoByGroupNote, DB.Context.FboLinxContext>,
        ICustomerInfoByGroupNoteEntityService
    {
        public CustomerInfoByGroupNoteEntityService(DB.Context.FboLinxContext context) : base(context)
        {
        }
    }
}
