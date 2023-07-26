using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore;
using FBOLinx.ServiceLayer.EntityServices;
using NUnit.Framework;

namespace FBOLinx.ServiceLayer.Test.Repositories
{
    public abstract class BaseRepositoryTests<TEntity, TRepository, TContext> : BaseTestFixture<TRepository>
        where TEntity : FBOLinxBaseEntityModel<int>
        where TRepository: IRepository<TEntity, TContext>
        where TContext : DbContext
    {
        protected abstract TEntity CreateTestEntity();

        [SetUp]
        public async Task SetupBeforeEachTest()
        {
            Arrange();
            await subject.BeginDbTransaction();
        }

        [TearDown]
        public async Task TeardownAfterEachTest()
        {
            await subject.RollbackDbTransaction();
        }

        [Test]
        public async Task AddAsync_ShouldAddEntityToDatabase()
        {
            var result = await subject.AddAsync(CreateTestEntity());

            Assert.Greater(result.Oid, 0);
        }

        [Test]
        public async Task AddRangeAsync_ShouldAddAllEntitiesToDatabase()
        {
            var result = await subject.AddRangeAsync(new List<TEntity>()
            {
                CreateTestEntity(),
                CreateTestEntity()
            });

            Assert.IsTrue(result.All(x => x.Oid > 0));
        }
        
        [Test]
        public async Task DeleteAsync_ShouldDeleteEntityInDatabase()
        {
            var entityToDelete = await subject.AddAsync(CreateTestEntity());
            await subject.DeleteAsync(entityToDelete);
            var result = await subject.FindAsync(entityToDelete.Oid);

            Assert.IsNull(result);
        }

        [Test]
        public async Task DeleteById_ShouldDeleteEntityInDatabase()
        {
            var entityToDelete = await subject.AddAsync(CreateTestEntity());
            await subject.DeleteAsync(entityToDelete.Oid);
            var result = await subject.FindAsync(entityToDelete.Oid);

            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateEntityInDatabase()
        {
            var testEntity = await subject.AddAsync(CreateTestEntity());
            var entityToUpdate = CreateTestEntity();
            entityToUpdate.Oid = testEntity.Oid;
            await subject.UpdateAsync(entityToUpdate);

            Assert.Greater(entityToUpdate.Oid, 0);
        }
    }
}
