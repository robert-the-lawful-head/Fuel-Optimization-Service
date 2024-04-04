using FBOLinx.Core.BaseModels.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.OrderNotes
{
    public sealed class OrderNotesByAssociatedFuelerlinxTransactionIdSpecification : Specification<Models.OrderNote>
    {
        public OrderNotesByAssociatedFuelerlinxTransactionIdSpecification(int associatedFuelerlinxTransactionId) : base(x => x.AssociatedFuelerLinxTransactionId == associatedFuelerlinxTransactionId)
        {
        }
    }
}
