using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.CustomerInfoByGroupNote
{
    public class CustomerInfoByGroupNoteByCustomerInfoByGroupIdSpecification : Specification<DB.Models.CustomerInfoByGroupNote>
    {
        public CustomerInfoByGroupNoteByCustomerInfoByGroupIdSpecification(int customerInfoByGroupId) : base(x => x.CustomerInfoByGroupId == customerInfoByGroupId)
        {
        }
    }
}
