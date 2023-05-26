using FBOLinx.Service.Mapping.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.Extensions.Customer
{
    public static class CustomerTypeExtension
    {
        public static bool IsOutOfNetwork(this CustomersDto? c)
        {
            return !IsInNetwork(c);
        }
        public static bool IsFuelerLinxClient(this CustomersDto? c)
        {
            return IsInNetwork(c) && isFuelerLinxCustomer(c);
        }
        public static bool IsInNetwork(this CustomersDto? c)
        {
            return ((c?.Oid > 0));
        }
        public static bool isFuelerLinxCustomer(this CustomersDto? c)
        {
            return (c?.FuelerlinxId.GetValueOrDefault() > 0);
        }
    }
}
