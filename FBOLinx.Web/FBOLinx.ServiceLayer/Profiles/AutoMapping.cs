using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FBOLinx.ServiceLayer.DTO;

namespace FBOLinx.ServiceLayer.Profiles
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            AllowNullCollections = true;

            CreateMap<CustomerDTO, FBOLinx.DB.Models.Customers>();
            CreateMap<FBOLinx.DB.Models.Customers, CustomerDTO>();

            CreateMap<GroupDTO, FBOLinx.DB.Models.Group>();
            CreateMap<FBOLinx.DB.Models.Group, GroupDTO>();

            CreateMap<CustomerInfoByGroupDTO, FBOLinx.DB.Models.CustomerInfoByGroup>();
            CreateMap<FBOLinx.DB.Models.CustomerInfoByGroup, CustomerInfoByGroupDTO>();

            CreateMap<CustomerAircraftDTO, FBOLinx.DB.Models.CustomerAircrafts>();
            CreateMap<FBOLinx.DB.Models.CustomerAircrafts, CustomerAircraftDTO>();
        }
    }
}
