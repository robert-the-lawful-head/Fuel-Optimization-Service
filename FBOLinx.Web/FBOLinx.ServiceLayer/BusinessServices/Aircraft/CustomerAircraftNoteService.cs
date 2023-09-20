using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.Aircraft
{
    public interface ICustomerAircraftNoteService : IBaseDTOService<CustomerAircraftNoteDto, DB.Models.CustomerAircraftNote>
    {
    }

    public class CustomerAircraftNoteService : BaseDTOService<CustomerAircraftNoteDto, DB.Models.CustomerAircraftNote, FboLinxContext>, ICustomerAircraftNoteService
    {
        public CustomerAircraftNoteService(ICustomerAircraftNoteEntityService customerAircraftNoteEntityService) : base(customerAircraftNoteEntityService)
        {
        }
    }
}
