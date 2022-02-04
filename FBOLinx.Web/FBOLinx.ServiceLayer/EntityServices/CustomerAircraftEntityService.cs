using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FBOLinx.DB.Context;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class CustomerAircraftEntityService : FBOLinxBaseEntityService<DB.Models.CustomerAircrafts, DTO.CustomerAircraftDTO, int>, IEntityService<DB.Models.CustomerAircrafts, DTO.CustomerAircraftDTO, int>
    {
        public CustomerAircraftEntityService(IMapper mapper, FboLinxContext context) : base(mapper, context)
        {
        }

        //public async Task<List<string>> GetMissingCustomerAircraftsFromTailList(int customerId, List<int> groupIds, List<string> tailNumbers)
        //{
        //    _Context.CustomerAircrafts.Where(x =>
        //        x.GroupId.HasValue && groupIds.Contains(x.GroupId.Value) && !string.IsNullOrEmpty(x.TailNumber) &&
        //        tailNumbers.Contains(x.TailNumber));

        //    //var  (from c in _Context.Customers
        //    //    join cg in _Context.CustomerInfoByGroup on c.Oid equals cg.CustomerId
        //    //    join ca in _Context.CustomerAircrafts on c.Oid equals ca.CustomerId
        //    //    into leftJoinCustomerAircrafts from ca in leftJoinCustomerAircrafts.DefaultIfEmpty()
        //    //    where c.Oid == customerId);
        //}
    }
}
