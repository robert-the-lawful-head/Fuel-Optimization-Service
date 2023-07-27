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
            List<CustomerInfoByGroup> testEntities = null;

            List<CustomerInfoByGroup> result = await Act<List<CustomerInfoByGroup>>(async () =>
            {
                testEntities = await subject.AddRangeAsync(new List<CustomerInfoByGroup>()
                {
                    CreateTestEntity(),
                    CreateTestEntity()
                });

                return await subject.GetListBySpec(new CustomerInfoByGroupCustomerIdGroupIdSpecification(testEntities.First().CustomerId, testEntities.First().GroupId));
            });

            Assert.AreEqual(2, result.Count(x => x.Username == testEntities.First().Username));
        }
    }
}
