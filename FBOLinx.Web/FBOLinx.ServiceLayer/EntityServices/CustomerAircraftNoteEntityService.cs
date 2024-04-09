using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface ICustomerAircraftNoteEntityService : IRepository<DB.Models.CustomerAircraftNote,
            DB.Context.FboLinxContext>
    {
    }
    public class CustomerAircraftNoteEntityService : Repository<DB.Models.CustomerAircraftNote, DB.Context.FboLinxContext>,
        ICustomerAircraftNoteEntityService
    {
        public CustomerAircraftNoteEntityService(DB.Context.FboLinxContext context) : base(context)
        {
        }
    }
}
