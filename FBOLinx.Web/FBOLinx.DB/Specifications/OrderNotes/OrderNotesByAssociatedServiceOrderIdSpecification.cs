using FBOLinx.Core.BaseModels.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.OrderNotes
{
    public sealed class OrderNotesByAssociatedServiceOrderIdSpecification : Specification<Models.OrderNote>
    {
        public OrderNotesByAssociatedServiceOrderIdSpecification(int associatedServiceOrderId) : base(x => x.AssociatedServiceOrderId == associatedServiceOrderId)
        {
        }
    }
}
