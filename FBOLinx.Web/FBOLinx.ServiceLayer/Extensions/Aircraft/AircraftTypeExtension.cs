using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.ServiceLayer.Extensions.Aircraft
{
    public static class AircraftTypeExtension
    {
        public static bool IsActiveFuelRelease(this FuelReqDto? fo)
        {
            return (fo != null);
        }
        public static bool IsOutOfNetwork(this CustomerAircrafts? ca, FuelReqDto? fo)
        {
            return !IsActiveFuelRelease(fo) && !IsInNetwork(ca, fo);
        }
        public static bool IsFuelerLinxClient(this CustomerAircrafts? ca, FuelReqDto? fo)
        {
            return IsInNetwork(ca, fo) && isFuelerLinxCustomer(ca);
        }
        public static bool IsInNetwork(this CustomerAircrafts? ca, FuelReqDto? fo)
        {
            return ((ca?.Oid > 0) && (fo == null) && (isFuelerLinxCustomer(ca)));
        }
        public static bool isFuelerLinxCustomer(this CustomerAircrafts? ca)
        {
            return (ca?.Customer?.FuelerlinxId.GetValueOrDefault() > 0);
        }
    }
}
