using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft;

namespace FBOLinx.ServiceLayer.Extensions.Aircraft
{
    public static class AircraftTypeExtension
    {
        public static bool IsActiveFuelRelease(this FuelReqDto? fo)
        {
            return (fo != null);
        }
        public static bool IsActiveFuelRelease(this FuelReqsGridViewModel? fo)
        {
            return fo != null;
        }
        public static bool IsOutOfNetwork(this CustomerAircrafts? ca)
        {
            return !IsInNetwork(ca);
        }
        public static bool IsOutOfNetwork(this CustomerAircraftsViewModel? ca)
        {
            return !IsInNetwork(ca);
        }
        public static bool IsFuelerLinxClient(this CustomerAircrafts? ca)
        {
            return IsInNetwork(ca) && isFuelerLinxCustomer(ca);
        }
        public static bool IsFuelerLinxClient(this CustomerAircraftsViewModel? ca)
        {
            return IsInNetwork(ca) && isFuelerLinxCustomer(ca);
        }
        public static bool IsInNetwork(this CustomerAircrafts? ca)
        {
            return ((ca?.Oid > 0));
        }
        public static bool IsInNetwork(this CustomerAircraftsViewModel? ca)
        {
            return ((ca?.Oid > 0));
        }
        public static bool isFuelerLinxCustomer(this CustomerAircrafts? ca)
        {
            return (ca?.Customer?.FuelerlinxId.GetValueOrDefault() > 0);
        }
        public static bool isFuelerLinxCustomer(this CustomerAircraftsViewModel? ca)
        {
            return (ca?.IsFuelerlinxNetwork).GetValueOrDefault();
        }
    }
}
