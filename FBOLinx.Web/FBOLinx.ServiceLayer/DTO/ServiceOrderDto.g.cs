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

namespace FBOLinx.ServiceLayer.DTO
{
    public class ServiceOrderDto
    {
        public int Oid { get; set; }
        public int Fboid { get; set; }
        public int GroupId { get; set; }
        public int CustomerId { get; set; }
        public DateTime ServiceDateTimeUtc { get; set; }
        [Required]
        [StringLength(50)]
        public string TailNumber { get; set; }
        public int? AssociatedFuelOrderId { get; set; }

        public int NumberOfCompletedItems
        {
            get
            {
                return (ServiceOrderItems?.Where(x => x.IsCompleted == true).Count()).GetValueOrDefault();
            }
        }

        #region Relationships
        
        public List<ServiceOrderItem> ServiceOrderItems { get; set; }
        #endregion
    }
}
