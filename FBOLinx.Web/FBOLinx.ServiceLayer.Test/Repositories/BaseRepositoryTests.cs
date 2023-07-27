using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore;
using FBOLinx.ServiceLayer.EntityServices;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore.Storage;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace FBOLinx.ServiceLayer.Test.Repositories
{
    public abstract class BaseRepositoryTests<TEntity, TRepository, TContext> : BaseTestFixture<TRepository>
        where TEntity : FBOLinxBaseEntityModel<int>
        where TRepository: IRepository<TEntity, TContext>
        where TContext : DbContext
    {
        protected IExecutionStrategy executionStrategy;
        
        [SetUp]
        public void SetupBeforeEachTest()
        {
            Arrange();
            executionStrategy = subject.CreateExecutionStrategy();
        }

        [TearDown]
        public void TeardownAfterEachTest()
        {
        }
        
        [Test]
        public async Task AddAsync_ShouldAddEntityToDatabase()
        {
            TEntity result = await Act<TEntity>(async () =>
            {
                return await subject.AddAsync(CreateTestEntity());
            });
            
            Assert.Greater(result.Oid, 0);
        }

        [Test]
        public async Task AddRangeAsync_ShouldAddAllEntitiesToDatabase()
        {
            List<TEntity> result = await Act<List<TEntity>>(async () =>
            {
                return await subject.AddRangeAsync(new List<TEntity>()
                {
                    CreateTestEntity(),
                    CreateTestEntity()
                });
            });

            Assert.IsTrue(result.All(x => x.Oid > 0));
        }
        
        [Test]
        public async Task DeleteAsync_ShouldDeleteEntityInDatabase()
        {
            TEntity result = await Act<TEntity>(async () =>
            {
                var entityToDelete = await subject.AddAsync(CreateTestEntity());
                await subject.DeleteAsync(entityToDelete);
                return await subject.FindAsync(entityToDelete.Oid);
            });

            Assert.IsNull(result);
        }

        [Test]
        public async Task DeleteById_ShouldDeleteEntityInDatabase()
        {
            TEntity result = await Act<TEntity>(async () =>
            {
                var entityToDelete = await subject.AddAsync(CreateTestEntity());
                await subject.DeleteAsync(entityToDelete.Oid);
                return await subject.FindAsync(entityToDelete.Oid);
            });

            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateEntityInDatabase()
        {
            TEntity result = await Act<TEntity>(async () =>
            {
                var testEntity = await subject.AddAsync(CreateTestEntity());
                var entityToUpdate = CreateTestEntity();
                entityToUpdate.Oid = testEntity.Oid;
                await subject.UpdateAsync(entityToUpdate);

                return entityToUpdate;
            });

            Assert.Greater(result.Oid, 0);
        }

        protected async Task<TResult> Act<TResult>(Func<Task<TResult>> func)
        {
            TResult result = default;
            await executionStrategy.Execute(async () =>
            {
                await subject.BeginDbTransaction();
                result = await func();
                await subject.RollbackDbTransaction();
            });

            return result;
        }

        protected abstract TEntity CreateTestEntity();
    }
}
