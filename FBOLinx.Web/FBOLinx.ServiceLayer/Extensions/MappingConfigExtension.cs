using FBOLinx.DB.Models;
using FBOLinx.DB.Models.Dega;
using FBOLinx.DB.Models.ServicesAndFees;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees;
using FBOLinx.ServiceLayer.DTO.ServicesAndFees;
using Fuelerlinx.SDK;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace FBOLinx.ServiceLayer.Extensions
{
    public static class MappingConfig
    {
        public static void RegisterMappingConfiguration(this IServiceCollection services)
        {
            TypeAdapterConfig<TransactionDTO, FuelReqDto>
                .NewConfig()
                .Map(dest => dest.Oid, src => 0)
                .Map(dest => dest.ActualPpg, src => 0)
                .Map(dest => dest.ActualVolume, src => src.InvoicedVolume.Amount)
                .Map(dest => dest.Archived, src => src.Archived)
                .Map(dest => dest.Cancelled, src => false)
                .Map(dest => dest.DateCreated, src => src.CreationDate)
                .Map(dest => dest.DispatchNotes, src => "")
                .Map(dest => dest.Eta, src => src.ArrivalDateTime)

                .Map(dest => dest.Etd, src => src.DepartureDateTime)
                .Map(dest => dest.Icao, src => src.Icao)
                .Map(dest => dest.Notes, src => "")
                .Map(dest => dest.QuotedPpg, src => 0)
                .Map(dest => dest.QuotedVolume, src => src.DispatchedVolume.Amount)
                .Map(dest => dest.Source, src => src.FuelVendor)
                .Map(dest => dest.SourceId, src => src.Id)
                .Map(dest => dest.TimeStandard, src => src.TimeStandard.ToString() == "0" ? "Z" : "L")
                .Map(dest => dest.Email, src => "")
                .Map(dest => dest.PhoneNumber, src => "")
                .Map(dest => dest.CustomerAircraft, src => new CustomerAircraftsDto() { TailNumber = src.TailNumber });

            TypeAdapterConfig<TransactionDTO, FuelReq>
                .NewConfig()
                .Map(dest => dest.Oid, src => 0)
                .Map(dest => dest.ActualPpg, src => 0)
                .Map(dest => dest.ActualVolume, src => src.InvoicedVolume.Amount)
                .Map(dest => dest.Archived, src => src.Archived)
                .Map(dest => dest.Cancelled, src => false)
                .Map(dest => dest.DateCreated, src => src.CreationDate)
                .Map(dest => dest.DispatchNotes, src => "")
                .Map(dest => dest.Eta, src => src.ArrivalDateTime)

                .Map(dest => dest.Etd, src => src.DepartureDateTime)
                .Map(dest => dest.Icao, src => src.Icao)
                .Map(dest => dest.Notes, src => "")
                .Map(dest => dest.QuotedPpg, src => 0)
                .Map(dest => dest.QuotedVolume, src => src.DispatchedVolume.Amount)
                .Map(dest => dest.Source, src => src.FuelVendor)
                .Map(dest => dest.SourceId, src => src.Id)
                .Map(dest => dest.TimeStandard, src => src.TimeStandard.ToString() == "0" ? "Z" : "L")
                .Map(dest => dest.Email, src => "")
                .Map(dest => dest.PhoneNumber, src => "")
                .Map(dest => dest.CustomerAircraft, src => new CustomerAircrafts() { TailNumber = src.TailNumber });

                TypeAdapterConfig<AcukwikServicesOffered, ServicesAndFeesDto>
                .NewConfig()
                .Map(dest => dest.IsActive, src => true)
                .Map(dest => dest.Oid, src => src.HandlerId.ToString()+src.ServiceOfferedId.ToString());

            TypeAdapterConfig<AcukwikServicesOffered, ServiceTypeResponse>
            .NewConfig()
            .Map(dest => dest.IsCustom, src => false);
                //.Map(dest => dest.ServiceType, src => src.Adapt<FboCustomServiceType>());
    
                TypeAdapterConfig<AcukwikServicesOffered, FboCustomServiceType>
                .NewConfig()
                .Map(dest => dest.Oid, src => 0)
                .Map(dest => dest.Name, src => src.Service);

                TypeAdapterConfig<AcukwikServicesOffered, ServicesAndFeesResponse>
                .NewConfig()
                .Map(dest => dest.IsCustom, src => false);

                TypeAdapterConfig<FboCustomServicesAndFees, ServiceTypeResponse>
                .NewConfig()
                .Map(dest => dest.IsCustom, src => true);

                TypeAdapterConfig<FboCustomServicesAndFees, FboCustomServiceType>
                .NewConfig()
                .Map(dest => dest.Oid, src => src.ServiceTypeId)
                .Map(dest => dest.Name, src => src.ServiceType);

                 TypeAdapterConfig<FboCustomServicesAndFees, ServicesAndFeesResponse>
                .NewConfig()
                .Map(dest => dest.IsCustom, src => true);
        }
    }
}
