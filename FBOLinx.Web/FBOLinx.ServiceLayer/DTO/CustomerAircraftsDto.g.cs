using System.Collections.Generic;
using FBOLinx.Core.Enums;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class CustomerAircraftsDto
    {
        public int GroupId { get; set; }
        public int CustomerId { get; set; }
        public int AircraftId { get; set; }
        public string TailNumber { get; set; }
        public AircraftSizes? Size { get; set; }
        public string BasedPaglocation { get; set; }
        public string NetworkCode { get; set; }
        public int? AddedFrom { get; set; }
        public string Make => Aircraft?.Make;
        public string Model => Aircraft?.Model;
        public ICollection<FuelReqDto> FuelReqs { get; set; }
        public AircraftPricesDto AircraftPrices { get; set; }
        public AirCraftsDto Aircraft { get; set; }
        public CustomersDto Customer { get; set; }
        public List<CustomerAircraftNoteDto> Notes { get; set; }
        public int Oid { get; set; }
    }
}