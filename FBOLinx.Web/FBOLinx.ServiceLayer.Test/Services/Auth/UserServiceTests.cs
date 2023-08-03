using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.ServiceOrder;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Auth;
using FBOLinx.ServiceLayer.BusinessServices.User;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.Web.Auth;
using FBOLinx.Web.Services;
using Fuelerlinx.SDK;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;

namespace FBOLinx.ServiceLayer.Test.Services.Auth
{
    public class UserServiceTests : BaseTestFixture<Web.Services.IUserService>
    {
        [Test]
        public async Task GetUserByCredentials_ExistingUserCredentialsProvided_UserReturned()
        {
            // Arrange
            User testUser = MockGetUser();
            Mock<BusinessServices.User.IUserService> userServiceMock = null;
            Arrange(services =>
            {
                userServiceMock = MockUserService(services);
            });

            // Act
            UserDTO existingUser = await subject.GetUserByCredentials(testUser.Username, testUser.Password);

            //Assert
            Assert.AreEqual(testUser.Oid, existingUser.Oid);
        }

        [Test]
        public async Task GetUserByCredentials_ExistingUserCredentialsProvidedAndAuthenticationRequested_UserWithAuthTokenReturned()
        {
            User testUser = MockGetUser();
            Mock<BusinessServices.User.IUserService> userServiceMock = null;
            Arrange(services =>
            {
                userServiceMock = MockUserService(services);
            });

            UserDTO existingUser = await subject.GetUserByCredentials(testUser.Username, testUser.Password, true);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(testUser.Oid, existingUser.Oid);
                Assert.IsNotNull(existingUser.Token);
                Assert.AreEqual(1, existingUser.LoginCount);
            });
        }

        [Test]
        public async Task GetUserByCredentials_ExistingUsernameButIncorrectPasswordProvided_NullReturned()
        {
            User testUser = MockGetUser();
            Mock<BusinessServices.User.IUserService> userServiceMock = null;
            Arrange(services =>
            {
                userServiceMock = MockUserService(services);
            });

            UserDTO result = await subject.GetUserByCredentials(testUser.Username, "incorrect");

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserByCredentials_IncorrectUsernameAndPasswordProvided_NullReturned()
        {
            User testUser = MockGetUser();
            Mock<BusinessServices.User.IUserService> userServiceMock = null;
            Arrange(services =>
            {
                userServiceMock = MockUserService(services);
            });

            UserDTO result = await subject.GetUserByCredentials("incorrect", "incorrect");

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserByCredentials_FboNetworkUsernameAndPasswordProvided_NullReturned()
        {
            User testUser = MockGetUser(true);
            Mock<BusinessServices.User.IUserService> userServiceMock = null;
            Arrange(services =>
            {
                userServiceMock = MockUserService(services);
            });

            UserDTO result = await subject.GetUserByCredentials(testUser.Username, testUser.Password);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserByCredentials_InactiveUserCredentialsProvided_NullReturned()
        {
            User testUser = MockGetUser(active: false);
            Mock<BusinessServices.User.IUserService> userServiceMock = null;
            Arrange(services =>
            {
                userServiceMock = MockUserService(services);
            });

            var result = await subject.GetUserByCredentials(testUser.Username, testUser.Password);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserByCredentials_OldLoginCredsProvidedAndCorrespondingFboRecordExists_FboLoginCreated()
        {
            User testUser = TestDataHelper.CreateTestUser();
            int fboRecordId = 1;
            int contactId = 1;
            Mock<FboLinxContext> dbContextMock = null;
            Mock<DbSet<User>> userDbSetMock = null;
            Mock<BusinessServices.User.IUserService> userServiceMock = null;
            Arrange(services =>
            {
                dbContextMock = new Mock<FboLinxContext>();
                userDbSetMock = MockUserDbSet(dbContextMock, new List<User>());
                MockFbosDbSet(dbContextMock, new List<Fbos>()
                {
                    new Fbos() { Oid = fboRecordId, Username = testUser.Username, Password = testUser.Password, GroupId = testUser.GroupId.Value }
                });
                MockGroupDbSet(dbContextMock, new List<Group>()
                {
                    new Group() { Oid = testUser.GroupId.Value, Isfbonetwork = false }
                });
                MockFboContactDbSet(dbContextMock, new List<Fbocontacts>()
                {
                    new Fbocontacts() { Fboid = fboRecordId, ContactId = contactId }
                });
                MockContactsDbSet(dbContextMock, new List<Contacts>()
                {
                    new Contacts() { Oid = contactId }
                });
                services.AddSingleton(dbContextMock.Object);

                userServiceMock = MockUserService(services);

                MockEncryptionService(services, testUser.Username, testUser.Password);
            });

            UserDTO createdUser = await subject.GetUserByCredentials(testUser.Username, testUser.Password);

            Assert.Multiple(() =>
            {
                userServiceMock.Verify(x => x.AddAsync(createdUser), Times.Once);
                Assert.AreEqual(testUser.Username, createdUser.Username);
            });
        }

        [Test]
        public async Task GetUserByCredentials_OldLoginCredsProvidedAndCorrespondingGroupRecordExists_GroupLoginCreated()
        {
            User testUser = TestDataHelper.CreateTestUser();
            int fboRecordId = 1;
            int contactId = 1;
            Mock<FboLinxContext> dbContextMock = null;
            Mock<DbSet<User>> userDbSetMock = null;
            Mock<BusinessServices.User.IUserService> userServiceMock = null;
            Arrange(services =>
            {
                dbContextMock = new Mock<FboLinxContext>();
                userDbSetMock = MockUserDbSet(dbContextMock, new List<User>());
                MockFbosDbSet(dbContextMock, new List<Fbos>());
                MockGroupDbSet(dbContextMock, new List<Group>()
                {
                    new Group() { Oid = testUser.GroupId.Value, Isfbonetwork = false, Username = testUser.Username, Password = testUser.Password }
                });
                MockFboContactDbSet(dbContextMock, new List<Fbocontacts>()
                {
                    new Fbocontacts() { Fboid = fboRecordId, ContactId = contactId }
                });
                MockContactsDbSet(dbContextMock, new List<Contacts>()
                {
                    new Contacts() { Oid = contactId }
                });
                services.AddSingleton(dbContextMock.Object);

                userServiceMock = MockUserService(services);

                MockEncryptionService(services, testUser.Username, testUser.Password);
            });

            UserDTO createdUser = await subject.GetUserByCredentials(testUser.Username, testUser.Password);

            Assert.Multiple(() =>
            {
                userServiceMock.Verify(x => x.AddAsync(createdUser), Times.Once);
                Assert.AreEqual(testUser.Username, createdUser.Username);
            });
        }

        [Test]
        public async Task CreateFBOLoginIfNeeded_UserExistsForFbo_ExistingUserReturned()
        {
            User testUser = TestDataHelper.CreateTestUser();
            testUser.FboId = 154;
            testUser.Role = FBOLinx.Core.Enums.UserRoles.Primary;
            Arrange(services =>
            {
                var dbContextMock = new Mock<FboLinxContext>();
                MockUserDbSet(dbContextMock, new List<User>() { testUser });
                services.AddSingleton(dbContextMock.Object);
                MockUserService(services);
            });

            UserDTO user = await subject.CreateFBOLoginIfNeeded(new Fbos() { Oid = testUser.FboId });

            Assert.AreEqual(testUser.Oid, user.Oid);
        }

        [Test]
        public async Task CreateFBOLoginIfNeeded_UserDoesNotExistForFbo_NewUserCreated()
        {
            User testUser = TestDataHelper.CreateTestUser();
            testUser.FboId = 1;
            testUser.Role = FBOLinx.Core.Enums.UserRoles.Primary;
            int fboRecordId = 1;
            int contactId = 1;
            Mock<FboLinxContext> dbContextMock = null;
            Mock<DbSet<User>> userDbSetMock = null;
            Mock<BusinessServices.User.IUserService> userServiceMock = null;
            Arrange(services =>
            {
                dbContextMock = new Mock<FboLinxContext>();
                userDbSetMock = MockUserDbSet(dbContextMock, new List<User>() { testUser });
                MockFboContactDbSet(dbContextMock, new List<Fbocontacts>()
                {
                    new Fbocontacts() { Fboid = fboRecordId, ContactId = contactId }
                });
                MockContactsDbSet(dbContextMock, new List<Contacts>()
                {
                    new Contacts() { Oid = contactId }
                });

                services.AddSingleton(dbContextMock.Object);

                userServiceMock = MockUserService(services);
            });

            UserDTO createdUser = await subject.CreateFBOLoginIfNeeded(new Fbos() { Oid = 2, Username = testUser.Username, Password = testUser.Password });

            Assert.Multiple(() =>
            {
                userServiceMock.Verify(x => x.AddAsync(createdUser), Times.Once);
                Assert.AreEqual(testUser.Username, createdUser.Username);
            });
        }

        [Test]
        public async Task CreateFBOLoginIfNeeded_UserDoesNotExistForFboAndMissedCredentials_NullReturned()
        {
            User testUser = TestDataHelper.CreateTestUser();
            testUser.FboId = 1;
            testUser.Role = FBOLinx.Core.Enums.UserRoles.Primary;
            Mock<BusinessServices.User.IUserService> userServiceMock = null;
            Arrange(services =>
            {
                var dbContextMock = new Mock<FboLinxContext>();
                MockUserDbSet(dbContextMock, new List<User>() { testUser });
                services.AddSingleton(dbContextMock.Object);

                userServiceMock = MockUserService(services);
            });

            var result = await subject.CreateFBOLoginIfNeeded(new Fbos() { Oid = 2 });

            Assert.IsNull(result);
        }

        [Test]
        public async Task CreateGroupLoginIfNeeded_UserExistsForGroup_ExistingUserReturned()
        {
            User testUser = TestDataHelper.CreateTestUser();
            testUser.GroupId = 1;
            testUser.Role = FBOLinx.Core.Enums.UserRoles.Conductor;
            Mock<BusinessServices.User.IUserService> userServiceMock = null;
            Arrange(services =>
            {
                var dbContextMock = new Mock<FboLinxContext>();
                MockUserDbSet(dbContextMock, new List<User>() { testUser });
                services.AddSingleton(dbContextMock.Object);

                userServiceMock = MockUserService(services);
            });

            UserDTO user = await subject.CreateGroupLoginIfNeeded(new Group() { Oid = testUser.GroupId.Value });

            Assert.AreEqual(testUser.Oid, user.Oid);
        }

        [Test]
        public async Task CreateGroupLoginIfNeeded_UserDoesNotExistForGroup_NewUserCreated()
        {
            User testUser = TestDataHelper.CreateTestUser();
            testUser.GroupId = 1;
            testUser.Role = FBOLinx.Core.Enums.UserRoles.Conductor;
            int fboRecordId = 1;
            int contactId = 1;
            Mock<FboLinxContext> dbContextMock = null;
            Mock<BusinessServices.User.IUserService> userServiceMock = null;
            Arrange(services =>
            {
                dbContextMock = new Mock<FboLinxContext>();
                services.AddSingleton(dbContextMock.Object);

                userServiceMock = MockUserService(services);

                MockEncryptionService(services, testUser.Username, testUser.Password);
            });

            UserDTO createdUser = await subject.CreateGroupLoginIfNeeded(
                new Group() { Oid = 2, Username = testUser.Username, Password = testUser.Password });

            Assert.Multiple(() =>
            {
                userServiceMock.Verify(x => x.AddAsync(createdUser), Times.Once);
                Assert.AreEqual(testUser.Username, createdUser.Username);
            });
        }

        [Test]
        public async Task CreateGroupLoginIfNeeded_UserDoesNotExistForGroupAndMissedCredentials_NullReturned()
        {
            User testUser = TestDataHelper.CreateTestUser();
            testUser.GroupId = 1;
            testUser.Role = FBOLinx.Core.Enums.UserRoles.Conductor;
            Mock<BusinessServices.User.IUserService> userServiceMock = null;
            Arrange(services =>
            {
                var dbContextMock = new Mock<FboLinxContext>();
                MockUserDbSet(dbContextMock, new List<User>() { testUser });
                services.AddSingleton(dbContextMock.Object);

                userServiceMock = MockUserService(services);
            });

            var result = await subject.CreateGroupLoginIfNeeded(new Group() { Oid = 2 });

            Assert.IsNull(result);
        }

        [Test]
        public void GetClaimedUserId_NameIdentifierExistsInHttpContext_ClaimedUserIdReturned()
        {
            User testUser = TestDataHelper.CreateTestUser();
            IHttpContextAccessor httpContextAccessor = MockHttpContextAccessor(ClaimTypes.NameIdentifier, testUser.Oid.ToString());
            
            int result = JwtManager.GetClaimedUserId(httpContextAccessor);

            Assert.AreEqual(testUser.Oid, result);
        }

        [Test]
        public void GetClaimedUserId_NameIdentifierDoesNotExistInHttpContext_ZeroReturned()
        {
            IHttpContextAccessor httpContextAccessor = MockHttpContextAccessor(string.Empty, string.Empty);

            int result = JwtManager.GetClaimedUserId(httpContextAccessor);

            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetClaimedFboId_SidExistsInHttpContext_ClaimedFboIdReturned()
        {
            User testUser = TestDataHelper.CreateTestUser();
            IHttpContextAccessor httpContextAccessor = MockHttpContextAccessor(ClaimTypes.Sid, testUser.FboId.ToString());

            int result = JwtManager.GetClaimedFboId(httpContextAccessor);

            Assert.AreEqual(testUser.FboId, result);
        }

        [Test]
        public void GetClaimedFboId_SidDoesNotExistInHttpContext_ZeroReturned()
        {
            IHttpContextAccessor httpContextAccessor = MockHttpContextAccessor(string.Empty, string.Empty);

            int result = JwtManager.GetClaimedFboId(httpContextAccessor);

            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetClaimedGroupId_GroupSidExistsInHttpContext_ClaimedGroupIdReturned()
        {
            User testUser = TestDataHelper.CreateTestUser();
            IHttpContextAccessor httpContextAccessor = MockHttpContextAccessor(ClaimTypes.GroupSid, testUser.GroupId.ToString());

            int result = JwtManager.GetClaimedGroupId(httpContextAccessor);

            Assert.AreEqual(testUser.GroupId, result);
        }

        [Test]
        public void GetClaimedGroupIdGroupSidDoesNotExistInHttpContext_ZeroReturned()
        {
            IHttpContextAccessor httpContextAccessor = MockHttpContextAccessor(string.Empty, string.Empty);

            int result = JwtManager.GetClaimedGroupId(httpContextAccessor);

            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetClaimedRole_RoleExistsInHttpContext_ClaimedRoleReturned()
        {
            User testUser = TestDataHelper.CreateTestUser();
            IHttpContextAccessor httpContextAccessor = MockHttpContextAccessor(ClaimTypes.Role, ((short)testUser.Role).ToString());

            var result = JwtManager.GetClaimedRole(httpContextAccessor);

            Assert.AreEqual(testUser.Role, result);
        }

        [Test]
        public void GetClaimedRole_RoleDoesNotExistInHttpContext_NotSetReturned()
        {
            IHttpContextAccessor httpContextAccessor = MockHttpContextAccessor(string.Empty, string.Empty);

            var result = JwtManager.GetClaimedRole(httpContextAccessor);

            Assert.AreEqual(FBOLinx.Core.Enums.UserRoles.NotSet, result);
        }

        private IHttpContextAccessor MockHttpContextAccessor(string claimType, string claimValue)
        {
            var httpContextMock = new Mock<IHttpContextAccessor>();
            httpContextMock.Setup(x => x.HttpContext.User.FindFirst(It.Is<string>(y => y == claimType)))
                .Returns(new Claim(claimType, claimValue));

            return httpContextMock.Object;
        }

        private User MockGetUser(bool isFboNetwork = false, bool active = true)
        {
            User testUser = TestDataHelper.CreateTestUser();
            testUser.Active = active;
            Arrange(services =>
            {
                var dbContextMock = new Mock<FboLinxContext>();
                MockUserDbSet(dbContextMock, new List<User>() { testUser });
                MockFbosDbSet(dbContextMock, new List<Fbos>());
                MockGroupDbSet(dbContextMock, new List<Group>() { new Group() { Oid = testUser.GroupId.Value, Isfbonetwork = isFboNetwork } });
                services.AddSingleton(dbContextMock.Object);
               

                MockEncryptionService(services, testUser.Username, testUser.Password);
            });

            return testUser;
        }

        private void MockEncryptionService(ServiceCollection services, string userName, string password)
        {
            var encryptionServiceMock = new Mock<IEncryptionService>();
            encryptionServiceMock
                .Setup(x => x.VerifyHashedPassword(
                    It.Is<string>(x => x == userName),
                    It.Is<string>(x => x == password)))
                .Returns(true);
            services.AddSingleton(encryptionServiceMock.Object);
        }

        private Mock<FBOLinx.ServiceLayer.BusinessServices.User.IUserService> MockUserService(ServiceCollection services)
        {
            var userServiceMock = new Mock<FBOLinx.ServiceLayer.BusinessServices.User.IUserService>();
            userServiceMock.Setup(x => x.GetSingleBySpec(new PrimaryUserByFboIdSpecification(It.IsAny<int>())));
            services.AddSingleton(userServiceMock.Object);

            return userServiceMock;
        }

        private Mock<DbSet<User>> MockUserDbSet(Mock<FboLinxContext> dbContextMock, IList<User> users)
        {
            Mock<DbSet<User>> userDbSet = users.AsQueryable().BuildMockDbSet();
            dbContextMock.Setup(x => x.User)
                .Returns(userDbSet.Object);

            return userDbSet;
        }

        private void MockFbosDbSet(Mock<FboLinxContext> dbContextMock, IList<Fbos> fbos)
        {
            var fbosDbSet = fbos.AsQueryable().BuildMockDbSet();
            dbContextMock.Setup(x => x.Fbos)
                .Returns(fbosDbSet.Object);
        }

        private void MockGroupDbSet(Mock<FboLinxContext> dbContextMock, IList<Group> groups)
        {
            var groupDbSet = groups.AsQueryable().BuildMockDbSet();
            dbContextMock.Setup(x => x.Group)
                .Returns(groupDbSet.Object);
        }

        private void MockFboContactDbSet(Mock<FboLinxContext> dbContextMock, IList<Fbocontacts> fbocontacts)
        {
            var fbocontactsDbSet = fbocontacts.AsQueryable().BuildMockDbSet();
            dbContextMock.Setup(x => x.Fbocontacts)
                .Returns(fbocontactsDbSet.Object);
        }

        private void MockContactsDbSet(Mock<FboLinxContext> dbContextMock, IList<Contacts> contacts)
        {
            var contactsDbSet = contacts.AsQueryable().BuildMockDbSet();
            dbContextMock.Setup(x => x.Contacts)
                .Returns(contactsDbSet.Object);
        }
    }
}