using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Service.Mapping.Dto;
using Mapster;
using FBOLinx.ServiceLayer.BusinessServices.Airport;

namespace FBOLinx.ServiceLayer.DTO
{
    public class ServiceOrderDto
    {
        private DateTime? _ArrivalDateTimeLocal;
        private DateTime? _DepartureDateTimeLocal;
        public int Oid { get; set; }
        public int Fboid { get; set; }
        public int GroupId { get; set; }
        public int CustomerInfoByGroupId { get; set; }
        public DateTime ArrivalDateTimeUtc { get; set; }
        public DateTime? DepartureDateTimeUtc { get; set; }
        public int CustomerAircraftId { get; set; }
        public int? AssociatedFuelOrderId { get; set; }
        public int? FuelerLinxTransactionId { get; set; }

        public int NumberOfCompletedItems
        {
            get
            {
                return (ServiceOrderItems?.Where(x => x.IsCompleted == true).Count()).GetValueOrDefault();
            }
        }

        public bool IsCompleted
        {
            get
            {
                return (ServiceOrderItems != null && ServiceOrderItems.Count > 0 && (ServiceOrderItems?.Where(x => x.IsCompleted == false).Count()).GetValueOrDefault() == 0);
            }
        }
        
        public DateTime? ArrivalDateTimeLocal => _ArrivalDateTimeLocal;
        public DateTime? DepartureDateTimeLocal => _DepartureDateTimeLocal;

        #region Relationships

        [AdaptIgnore(MemberSide.Source)]
        public List<ServiceOrderItemDto> ServiceOrderItems { get; set; }
        [AdaptIgnore(MemberSide.Source)]
        public CustomerInfoByGroupDto CustomerInfoByGroup { get; set; }
        [AdaptIgnore(MemberSide.Source)]
        public CustomerAircraftsDto CustomerAircraft { get; set; }

        #endregion

        public async Task PopulateLocalTimes(IAirportTimeService airportTimeService)
        {
            _ArrivalDateTimeLocal = await airportTimeService.GetAirportLocalDateTime(Fboid, ArrivalDateTimeUtc);
            if (DepartureDateTimeUtc.HasValue)
                _DepartureDateTimeLocal = await airportTimeService.GetAirportLocalDateTime(Fboid, DepartureDateTimeUtc);
        }
    }
}
