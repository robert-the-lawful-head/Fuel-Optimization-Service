using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.CustomerAircraftNote
{
    public class CustomerAircraftNoteByCustomerAircraftIdSpecification : Specification<Models.CustomerAircraftNote>
    {
        public CustomerAircraftNoteByCustomerAircraftIdSpecification(int customerAircraftId) : base(x => x.CustomerAircraftId == customerAircraftId)
        {
        }
    }
}
