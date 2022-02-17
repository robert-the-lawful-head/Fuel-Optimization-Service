using System;
using System.Collections.Generic;
using FBOLinx.Core.Enums;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class FbosDto
    {
        public int Oid { get; set; }
        public string Fbo { get; set; }
        public int? GroupId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public double? PostedRetail { get; set; }
        public double? TotalCost { get; set; }
        public bool? Active { get; set; }
        public DateTime? DateActivated { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public double? TotalCost100Ll { get; set; }
        public double? PostedRetail100Ll { get; set; }
        public bool? Suspended { get; set; }
        public bool? GroupMargin { get; set; }
        public bool? ApplyGroupMargin { get; set; }
        public short? GroupMarginSetting { get; set; }
        public bool? GroupMarginFuture { get; set; }
        public short? GroupMarginTemplate { get; set; }
        public double? GroupMarginMargin { get; set; }
        public MarginTypes? GroupMarginType { get; set; }
        public double? GroupMargin100Llmargin { get; set; }
        public short? GroupMargin100Lltype { get; set; }
        public string FuelDeskEmail { get; set; }
        public string Website { get; set; }
        public string MainPhone { get; set; }
        public string Extension { get; set; }
        public short? DefaultMarginTypeJetA { get; set; }
        public MarginTypes? DefaultMarginType100Ll { get; set; }
        public bool? SalesTax { get; set; }
        public bool? ApplySalesTax { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool? PriceUpdateReminderPrompt { get; set; }
        public bool? PriceUpdateNeverPrompt { get; set; }
        public string Currency { get; set; }
        public bool? InitialSetupPhase { get; set; }
        public bool DisableCost { get; set; }
        public int? AcukwikFBOHandlerId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string SenderAddress { get; set; }
        public string ReplyTo { get; set; }
        public AccountTypes AccountType { get; set; }
        public GroupDto Group { get; set; }
        public FboairportsDto fboAirport { get; set; }
        public ICollection<FuelReqDto> FuelReqs { get; set; }
        public ICollection<PricingTemplateDto> PricingTemplates { get; set; }
        public FbopreferencesDto Preferences { get; set; }
        public ICollection<FbocontactsDto> Contacts { get; set; }
        public ICollection<UserDto> Users { get; set; }
        public ICollection<FbopricesDto> Fboprices { get; set; }
    }
}