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

namespace FBOLinx.ServiceLayer.DTO
{
    public class ServiceOrderDto
    {
        public int Oid { get; set; }
        public int Fboid { get; set; }
        public int GroupId { get; set; }
        public int CustomerInfoByGroupId { get; set; }
        public DateTime ServiceDateTimeUtc { get; set; }
        public DateTime ServiceDateTimeLocal { get; set; }
        public int CustomerAircraftId { get; set; }
        public int? AssociatedFuelOrderId { get; set; }

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
                return (ServiceOrderItems.Count > 0 && (ServiceOrderItems?.Where(x => x.IsCompleted == false).Count()).GetValueOrDefault() == 0);
            }
        }

        #region Relationships

        [AdaptIgnore(MemberSide.Source)]
        public List<ServiceOrderItem> ServiceOrderItems { get; set; }
        [AdaptIgnore(MemberSide.Source)]
        public CustomerInfoByGroupDto CustomerInfoByGroup { get; set; }
        [AdaptIgnore(MemberSide.Source)]
        public CustomerAircraftsDto CustomerAircraft { get; set; }

        #endregion
    }
}
