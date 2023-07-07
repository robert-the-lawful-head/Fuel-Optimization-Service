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
using FBOLinx.Core.Enums;

namespace FBOLinx.ServiceLayer.DTO
{
    public class OrderDetailsDto : FBOLinxBaseEntityModel<int>
    {
        public int FuelerLinxTransactionId { get; set; }
        public string ConfirmationEmail { get; set; }
        public string FuelVendor { get; set; }
    }
}
