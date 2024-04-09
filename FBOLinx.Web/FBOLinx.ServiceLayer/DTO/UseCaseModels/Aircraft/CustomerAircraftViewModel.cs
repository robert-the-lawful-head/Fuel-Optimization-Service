﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft
{
    public class CustomerAircraftsViewModel
    {
        public int Oid { get; set; }
        public int? GroupId { get; set; }
        public int CustomerId { get; set; }
        public string Company { get; set; }
        public int AircraftId { get; set; }
        public string TailNumber { get; set; }
        public AircraftSizes? Size { get; set; }
        public string BasedPaglocation { get; set; }
        public string NetworkCode { get; set; }
        public int? AddedFrom { get; set; }
        public int? PricingTemplateId { get; set; }
        public string PricingTemplateName { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public double? FuelCapacityGal { get; set; }
        public bool IsFuelerlinxNetwork { get; set; }
        public bool IsCompanyPricing { get; set; }
        public string ICAOAircraftCode { get; set; }
        public string Phone { get; set; }
        public int CustomerInfoByGroupId { get; set; }
        public int? FuelerlinxCompanyId { get; set; }
        public int CustomerAircraftId { get; set; }

        public FboFavoriteAircraft? FavoriteAircraft { get; set; }
        public bool IsCustomerManagerAircraft { get; set; } = true;

        public string AircraftSizeDescription
        {
            get { return FBOLinx.Core.Utilities.Enums.EnumHelper.GetDescription(Size ?? AircraftSizes.NotSet); }
        }
        public List<CustomerAircraftNoteDto> Notes { get; set; }

        public void CastFromDTO(CustomerAircraftsDto customerAircraft)
        {
            Oid = customerAircraft.Oid;
            GroupId = customerAircraft.GroupId;
            CustomerId = customerAircraft.CustomerId;
            Company = customerAircraft.Customer?.Company;
            AircraftId = customerAircraft.AircraftId;
            TailNumber = customerAircraft.TailNumber;
            Size = customerAircraft.Size.HasValue && customerAircraft.Size != AircraftSizes.NotSet
                ? customerAircraft.Size
                : (AircraftSizes.NotSet);
            BasedPaglocation = customerAircraft.BasedPaglocation;
            NetworkCode = customerAircraft.NetworkCode;
            AddedFrom = customerAircraft.AddedFrom ?? 0;
            IsFuelerlinxNetwork = customerAircraft.Customer?.FuelerlinxId > 0;
            Phone = customerAircraft?.Customer?.CustomerInfoByGroup?.FirstOrDefault()?.MainPhone;
            CustomerInfoByGroupId = (customerAircraft?.Customer?.CustomerInfoByGroup?.FirstOrDefault()?.Oid).GetValueOrDefault();
            FuelerlinxCompanyId = (customerAircraft?.Customer?.FuelerlinxId);
            Notes = customerAircraft.Notes;
            FavoriteAircraft = customerAircraft.FavoriteAircraft;
            CustomerAircraftId = customerAircraft.Oid;
        }

        public static CustomerAircraftsViewModel Cast(CustomerAircraftsDto customerAircraft)
        {
            var result = new CustomerAircraftsViewModel();
            result.CastFromDTO(customerAircraft);
            return result;
        }
    }
}
