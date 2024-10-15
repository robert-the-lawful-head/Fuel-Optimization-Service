using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.CustomerInfoByGroup
{
    public sealed class CustomerInfoByGroupByGroupIdSpecification : Specification<Models.CustomerInfoByGroup>
    {
        public CustomerInfoByGroupByGroupIdSpecification(int groupId) : base(x => x.GroupId == groupId)
        {
            AddInclude(x => x.Customer);
            AddInclude(x => x.Notes);
        }
        public CustomerInfoByGroupByGroupIdSpecification(int groupId, int customerInfoByGroupId = 0) : base(x => x.GroupId == groupId && x.Oid == customerInfoByGroupId && (!x.Customer.Suspended.HasValue || x.Customer.Suspended == false))
        {
            AddInclude(x => x.Customer);
        }
    }
}
