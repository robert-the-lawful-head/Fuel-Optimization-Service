using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.DB.Models;
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
            user.Role = User.UserRoles.Member;

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
            distributePricingRequest.PricingTemplate.MarginType = PricingTemplate.MarginTypes.CostPlus;
            distributePricingRequest.Customer = CreateCustomerInfoByGroup();
            distributePricingRequest.Customer.CustomerId = 1;

            return distributePricingRequest;
        }

        public static DistributePricingRequest CreateDistributePricingRequest()
        {
            DistributePricingRequest distributePricingRequest = new DistributePricingRequest();
            distributePricingRequest.GroupId = 1;
            distributePricingRequest.FboId = 1;
            distributePricingRequest.PricingTemplate = new PricingTemplate();
            distributePricingRequest.PricingTemplate.EmailContentId = 1;
            distributePricingRequest.PricingTemplate.Oid = 1;
            distributePricingRequest.PricingTemplate.Notes = string.Empty;
            distributePricingRequest.PricingTemplate.MarginType = PricingTemplate.MarginTypes.CostPlus;
            
            return distributePricingRequest;
        }

        public static CustomerInfoByGroup CreateCustomerInfoByGroup()
        {
            CustomerInfoByGroup customerInfoByGroup = new CustomerInfoByGroup();
            customerInfoByGroup.Oid = 1;

            return customerInfoByGroup;
        }
    }
}
    