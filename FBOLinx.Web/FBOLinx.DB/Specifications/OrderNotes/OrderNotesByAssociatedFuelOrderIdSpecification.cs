using FBOLinx.Core.BaseModels.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.OrderNotes
{
    public sealed class OrderNotesByAssociatedFuelOrderIdSpecification : Specification<Models.OrderNote>
    {
        public OrderNotesByAssociatedFuelOrderIdSpecification(int associatedFuelOrderId) : base(x => x.AssociatedFuelOrderId == associatedFuelOrderId)
        {
        }
    }
}
