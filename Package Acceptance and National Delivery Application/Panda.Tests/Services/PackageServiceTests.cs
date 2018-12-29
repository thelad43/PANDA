namespace Panda.Tests.Services
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Panda.Models;
    using Panda.Models.Enums;
    using Panda.Services;
    using Panda.Services.Implementations;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class PackageServiceTests
    {
        private const int Weight = 15;
        private const string Description = "Descr";
        private const string ShippingAddress = "Some Address";

        public PackageServiceTests()
        {
            Tests.Initialize();
        }

        [Fact]
        public void CreateAsyncShouldThrowInvalidOperationExceptionIfUserIsNull()
        {
            var userManager = this.GetUserManagerMock().Object;

            var receiptService = new Mock<IReceiptService>().Object;

            var db = DbInfrastructure.GetDatabase();

            var packageService = new PackageService(db, receiptService, userManager);

            Func<Task> func = async () => await packageService
                .CreateAsync("Descr", 15, "Some Address", Guid.NewGuid().ToString());

            func.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async Task CreateAsyncShouldCreateAndSavePackageToDb()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            var user = new User
            {
                UserName = "Some User"
            };

            await db.AddAsync(user);
            await db.SaveChangesAsync();

            var model = await packageService.CreateAsync(Description, Weight, ShippingAddress, user.Id);

            var package = await db.Packages.FirstOrDefaultAsync();

            package
                .Should()
                .Match<Package>(p => p.Description == Description)
                .And
                .Match<Package>(p => p.RecipientId == user.Id)
                .And
                .Match<Package>(p => p.ShippingAddress == ShippingAddress)
                .And
                .Match<Package>(p => p.Status == Status.Pending)
                .And
                .Match<Package>(p => p.Weight == Weight);
        }

        [Fact]
        public void AcquireAsyncShouldThrowInvalidOperationExceptionIfPackageIsNull()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            Func<Task> func = async () => await packageService.AcquireAsync(5);

            func.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async Task AcquireAsyncShouldMarkGivenPackageAsAcquired()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            var package = new Package
            {
                Description = Description,
                ShippingAddress = ShippingAddress,
                Weight = Weight
            };

            await db.AddAsync(package);
            await db.SaveChangesAsync();

            var createdPackage = await db.Packages.FirstOrDefaultAsync();

            await packageService.AcquireAsync(createdPackage.Id);

            var actualPackage = await db.Packages.FirstOrDefaultAsync();

            actualPackage
                .Status.Should().Be(Status.Acquired);
        }

        [Fact]
        public async Task AllDeliveredAsyncShouldReturnOnlyDeliveredPackages()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            await AddPackagesToDb(db);

            var delivered = await packageService.AllDeliveredAsync();

            delivered
                .Should()
                .HaveCount(20);
        }

        [Fact]
        public async Task AllPendingAsyncShouldReturnOnlyPendingPackages()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            await AddPackagesToDb(db);

            var pending = await packageService.AllPendingAsync();

            pending
                .Should()
                .HaveCount(10);
        }

        [Fact]
        public async Task AllShippedAsyncShouldReturnOnlyShippedPackages()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            await AddPackagesToDb(db);

            var shipped = await packageService.AllShippedAsync();

            shipped
                .Should()
                .HaveCount(30);
        }

        private static async Task AddPackagesToDb(Data.PandaDbContext db)
        {
            for (var i = 0; i < 10; i++)
            {
                var package = new Package
                {
                    Description = $"Descr {i}",
                    Weight = i * 2.2,
                    ShippingAddress = $"Shipping {i + 100}",
                    RecipientId = Guid.NewGuid().ToString(),
                    Status = Status.Pending,
                };

                await db.AddAsync(package);
            }

            for (var i = 0; i < 20; i++)
            {
                var package = new Package
                {
                    Description = $"Descr {i}",
                    Weight = i * 2.3,
                    ShippingAddress = $"Shipping {i + 1000}",
                    RecipientId = Guid.NewGuid().ToString(),
                    Status = Status.Delivered,
                };

                await db.AddAsync(package);
            }

            for (var i = 0; i < 30; i++)
            {
                var package = new Package
                {
                    Description = $"Descr {i}",
                    Weight = i * 2.4,
                    ShippingAddress = $"Shipping {i + 10000}",
                    RecipientId = Guid.NewGuid().ToString(),
                    Status = Status.Shipped,
                };

                await db.AddAsync(package);
            }

            for (var i = 0; i < 40; i++)
            {
                var package = new Package
                {
                    Description = $"Descr {i}",
                    Weight = i * 2.5,
                    ShippingAddress = $"Shipping {i + 100000}",
                    RecipientId = Guid.NewGuid().ToString(),
                    Status = Status.Acquired,
                };

                await db.AddAsync(package);
            }

            await db.SaveChangesAsync();
        }

        private Mock<UserManager<User>> GetUserManagerMock()
            => new Mock<UserManager<User>>(
                   new Mock<IUserStore<User>>().Object,
                   new Mock<IOptions<IdentityOptions>>().Object,
                   new Mock<IPasswordHasher<User>>().Object,
                   new IUserValidator<User>[0],
                   new IPasswordValidator<User>[0],
                   new Mock<ILookupNormalizer>().Object,
                   new Mock<IdentityErrorDescriber>().Object,
                   new Mock<IServiceProvider>().Object,
                   new Mock<ILogger<UserManager<User>>>().Object);
    }
}