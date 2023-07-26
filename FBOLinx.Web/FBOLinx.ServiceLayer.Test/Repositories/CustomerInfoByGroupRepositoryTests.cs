using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.TableStorage.Entities;
using IdentityModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;

namespace FBOLinx.ServiceLayer.Test.Repositories
{
    public class CustomerInfoByGroupRepositoryTests: BaseRepositoryTests<CustomerInfoByGroup, ICustomerInfoByGroupEntityService, FboLinxContext>
    {
        protected override CustomerInfoByGroup CreateTestEntity()
        {
            return TestDataHelper.CreateCustomerInfoByGroup();
        }

        [Test]
        public async Task GetListBySpec_ShouldReturnSpecifiedEntities()
        {
            var testEntities = await subject.AddRangeAsync(new List<CustomerInfoByGroup>()
            {
                CreateTestEntity(),
                CreateTestEntity()
            });
            var result = await subject.GetListBySpec(new CustomerInfoByGroupCustomerIdGroupIdSpecification(testEntities.First().CustomerId, testEntities.First().GroupId));

            Assert.AreEqual(2, result.Count(x => x.Username == testEntities.First().Username));
        }
    }
}
