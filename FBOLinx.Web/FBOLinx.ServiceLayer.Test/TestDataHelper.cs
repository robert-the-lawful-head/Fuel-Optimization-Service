using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.Web.Models.Requests;

namespace FBOLinx.ServiceLayer.Test
{
    public static class TestDataHelper
    {
        public static User CreateTestUser()
        {
            User user = new User();
            user.Oid = 1;
            user.Username = "test";
            user.Password = "test";
            user.GroupId = 1;
            user.FboId = 1;
            user.Active = true;
            user.Role = UserRoles.Member;

            return user;
        }

        public static DistributePricingRequest CreateDistributePreviewPricingRequest()
        {
            DistributePricingRequest distributePricingRequest = new DistributePricingRequest();
            distributePricingRequest.GroupId = 1;
            distributePricingRequest.FboId = 1;
            distributePricingRequest.PricingTemplate = new PricingTemplate();
            distributePricingRequest.PricingTemplate.EmailContentId = 1;
            distributePricingRequest.PricingTemplate.Notes = string.Empty;
            distributePricingRequest.PricingTemplate.MarginType = MarginTypes.CostPlus;
            distributePricingRequest.Customer = new CustomerInfoByGroupDto()
            {
                Oid = 1,
                CustomerId = 1
            };
            distributePricingRequest.Customer.CustomerId = 1;

            return distributePricingRequest;
        }

        public static DistributePricingRequest CreateDistributePricingRequest()
        {
            DistributePricingRequest distributePricingRequest = new DistributePricingRequest();
            distributePricingRequest = CreateDistributePreviewPricingRequest();
            distributePricingRequest.PricingTemplate.Oid = 1;

            return distributePricingRequest;
        }

        public static CustomerInfoByGroup CreateCustomerInfoByGroup()
        {
            var customerInfoByGroup = new CustomerInfoByGroup();
            customerInfoByGroup.CustomerId = int.MaxValue;
            customerInfoByGroup.GroupId = int.MaxValue;
            customerInfoByGroup.Username = "TestUser";

            return customerInfoByGroup;
        }
    }
}
    