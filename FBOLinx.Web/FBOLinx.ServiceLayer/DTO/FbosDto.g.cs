using System;
using System.Collections.Generic;
using Azure;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.Requests;
using Fuelerlinx.SDK;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class FbosDto
    {
        public int Oid { get; set; }
        public string Fbo { get; set; }
        public int GroupId { get; set; }
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
        public string AntennaName { get; set; }
        public AccountTypes AccountType { get; set; }
        public GroupDTO Group { get; set; }
        public FboairportsDto FboAirport { get; set; }
        public ICollection<FuelReqDto> FuelReqs { get; set; }
        public ICollection<PricingTemplateDto> PricingTemplates { get; set; }
        public FboPreferencesDTO Preferences { get; set; }
        public ICollection<FbocontactsDto> Contacts { get; set; }
        public ICollection<UserDTO> Users { get; set; }
        //public ICollection<FbopricesDto> Fboprices { get; set; }

        public void Cast(Fbos item)
        {
            Oid = item.Oid;
            Fbo = item.Fbo;
            GroupId = item.GroupId;
            Username = item.Username;
            Password = item.Password;
            PostedRetail = item.PostedRetail;
            TotalCost = item.TotalCost;
            Active = item.Active;
            DateActivated = item.DateActivated;
            Address = item.Address;
            City = item.City;
            State = item.State;
            Country = item.Country;
            TotalCost100Ll = item.TotalCost100Ll;
            PostedRetail100Ll = item.PostedRetail100Ll;
            Suspended = item.Suspended;
            GroupMargin = item.GroupMargin;
            ApplyGroupMargin = item.ApplyGroupMargin;
            GroupMarginSetting = item.GroupMarginSetting;
            GroupMarginFuture = item.GroupMarginFuture;
            GroupMarginTemplate = item.GroupMarginTemplate;
            GroupMarginMargin = item.GroupMarginMargin;
            GroupMarginType = item.GroupMarginType;
            GroupMargin100Llmargin = item.GroupMargin100Llmargin;
            GroupMargin100Lltype = item.GroupMargin100Lltype;
            FuelDeskEmail = item.FuelDeskEmail;
            Website = item.Website;
            MainPhone = item.MainPhone;
            Extension = item.Extension;
            DefaultMarginTypeJetA = item.DefaultMarginTypeJetA;
            DefaultMarginType100Ll = item.DefaultMarginType100Ll;
            SalesTax = item.SalesTax;
            ApplySalesTax = item.ApplySalesTax;
            LastLogin = item.LastLogin;
            PriceUpdateReminderPrompt = item.PriceUpdateReminderPrompt;
            PriceUpdateNeverPrompt = item.PriceUpdateNeverPrompt;
            Currency = item.Currency;
            InitialSetupPhase = item.InitialSetupPhase;
            DisableCost = item.DisableCost;
            AcukwikFBOHandlerId = item.AcukwikFBOHandlerId;
            ExpirationDate = item.ExpirationDate;
            SenderAddress = item.SenderAddress;
            ReplyTo = item.ReplyTo;
            AccountType = item.AccountType;
        }
    }
}