using FBOLinx.Core.BaseModels.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.OrderNotes
{
    public sealed class OrderNoteByIdSpecification : Specification<Models.OrderNote>
    {
        public OrderNoteByIdSpecification(int oid) : base(x => x.Oid == oid)
        {
        }
    }
}
