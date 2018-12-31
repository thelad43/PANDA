namespace Panda.Tests.Services
{
    using Data;
    using FluentAssertions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Models;
    using Moq;
    using Panda.Models.Enums;
    using Panda.Services;
    using Panda.Services.Implementations;
    using Panda.Services.Models.Package;
    using System;
    using System.Linq;
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
                .Status
                .Should()
                .Be(Status.Acquired);
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

        [Fact]
        public void DeliverAsyncShouldThrowInvalidOperationExceptionIfPackageIsNull()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            Func<Task> func = async () => await packageService.DeliverAsync(5);

            func.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async Task DeliverAsyncShouldMarkGivenPackageAsDelivered()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            await AddPackagesToDb(db);

            var packages = await db
                .Packages
                .Where(p => p.Status == Status.Pending)
                .OrderBy(p => Guid.NewGuid())
                .ToListAsync();

            var expectedPackage = packages.First();

            await packageService.DeliverAsync(expectedPackage.Id);

            var package = await db.Packages.FindAsync(expectedPackage.Id);

            package
                .Status
                .Should()
                .Be(Status.Delivered);
        }

        [Fact]
        public void DeliveredForUserAsyncShouldThrowInvalidOperationExceptionIfUserIsNull()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            Func<Task> func = async () => await packageService.DeliveredForUserAsync(null);

            func.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async Task DeliveredForUserAsyncShouldReturnAllDeliveredPackagesForGivenUser()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            var firstUser = new User
            {
                UserName = "Pesho"
            };

            var secondUser = new User
            {
                UserName = "Gosho"
            };

            await db.AddRangeAsync(firstUser, secondUser);
            await db.SaveChangesAsync();

            const int PackagesCount = 50;

            for (var i = 0; i < PackagesCount; i++)
            {
                var package = new Package
                {
                    RecipientId = firstUser.Id,
                    Status = Status.Delivered
                };

                await db.AddAsync(package);
            }

            for (var i = 0; i < 10; i++)
            {
                var package = new Package
                {
                    RecipientId = secondUser.Id
                };

                await db.AddAsync(package);
            }

            await db.SaveChangesAsync();

            var actualDeliveredPackages = await packageService.DeliveredForUserAsync(firstUser);

            actualDeliveredPackages
                .Should()
                .HaveCount(PackagesCount);
        }

        [Fact]
        public void DetailsAsyncShouldThrowInvalidOperationExceptionIfPackageIsNotFound()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            Func<Task> func = async () => await packageService.DetailsAsync(15);

            func.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async Task DetailsAsyncShouldReturnCorrectPackage()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            const int ActualWeight = 30;
            const string ActualDescription = "Third";
            const string ActualShippingAddress = "Adr3";

            var firstPackage = new Package { Id = 1, Description = "First", Weight = 15, ShippingAddress = "Adr1" };
            var secondPackage = new Package { Id = 2, Description = "Second", Weight = 20, ShippingAddress = "Adr2" };
            var thirdPackage = new Package { Id = 3, Description = ActualDescription, Weight = ActualWeight, ShippingAddress = ActualShippingAddress };

            await db.AddRangeAsync(firstPackage, secondPackage, thirdPackage);
            await db.SaveChangesAsync();

            var package = await packageService.DetailsAsync(3);

            package
                .Should()
                .Match<PackageDetailsServiceModel>(p => p.Description == ActualDescription)
                .And
                .Match<PackageDetailsServiceModel>(p => p.Weight == ActualWeight)
                .And
                .Match<PackageDetailsServiceModel>(p => p.ShippingAddress == ActualShippingAddress);
        }

        [Fact]
        public void DetailsByUserAsyncShouldThrowInvalidOperationExceptionIfUserIsNull()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            Func<Task> func = async () => await packageService.DetailsByUserAsync(null, 10);

            func.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async Task DetailsByUserAsyncShouldThrowInvalidOperationExceptionIfPackageIsNotFound()
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
                UserName = "Gosho Ivanov"
            };

            await db.AddAsync(user);
            await db.SaveChangesAsync();

            Func<Task> func = async () => await packageService.DetailsByUserAsync(user, 5);

            func.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async Task DetailsByUserAsyncShouldThrowInvalidOperationExceptionIfGivenUserIsNotOwnership()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            var firstUser = new User
            {
                UserName = "Ivan"
            };

            var secondUser = new User
            {
                UserName = "Drago"
            };

            await db.AddRangeAsync(firstUser, secondUser);
            await db.SaveChangesAsync();

            for (var i = 0; i < 50; i++)
            {
                var package = new Package
                {
                    RecipientId = firstUser.Id,
                    Description = $"Descr {i}",
                    Status = Status.Shipped
                };

                await db.AddAsync(package);
            }

            for (var i = 0; i < 10; i++)
            {
                var package = new Package
                {
                    RecipientId = secondUser.Id
                };

                await db.AddAsync(package);
            }

            Func<Task> func = async () => await packageService.DetailsByUserAsync(secondUser, 3);

            func.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async Task DetailsByUserAsyncShouldReturnCorrectPackage()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            var firstUser = new User
            {
                UserName = "Ivan"
            };

            var secondUser = new User
            {
                UserName = "Drago"
            };

            await db.AddRangeAsync(firstUser, secondUser);
            await db.SaveChangesAsync();

            var packageId = 0;

            for (var i = 0; i < 50; i++)
            {
                var package = new Package
                {
                    RecipientId = firstUser.Id,
                    Description = $"Descr {i}",
                    Status = Status.Shipped
                };

                await db.AddAsync(package);
                await db.SaveChangesAsync();

                if (i == 2)
                {
                    packageId = package.Id;
                }
            }

            for (var i = 0; i < 10; i++)
            {
                var package = new Package
                {
                    RecipientId = secondUser.Id
                };

                await db.AddAsync(package);
            }

            await db.SaveChangesAsync();

            var actualPackage = await packageService.DetailsByUserAsync(firstUser, packageId);

            actualPackage
                .Should()
                .Match<PackageDetailsServiceModel>(p => p.Id == packageId)
                .And
                .Match<PackageDetailsServiceModel>(p => p.Description == "Descr 2")
                .And
                .Match<PackageDetailsServiceModel>(p => p.Status == Status.Shipped)
                .And
                .Match<PackageDetailsServiceModel>(p => p.RecipientName == "Ivan");
        }

        [Fact]
        public void PendingForUserAsyncShouldThrowInvalidOperationExceptionIfUserIsNull()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            Func<Task> func = async () => await packageService.PendingForUserAsync(null);

            func.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async Task PendingForUserAsyncShouldReturnAllPendingPackagesForGivenUser()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            var firstUser = new User
            {
                UserName = "Stamat"
            };

            var secondUser = new User
            {
                UserName = "Ted"
            };

            await db.AddRangeAsync(firstUser, secondUser);
            await db.SaveChangesAsync();

            for (var i = 0; i < 50; i++)
            {
                var package = new Package
                {
                    RecipientId = firstUser.Id,
                    Status = Status.Delivered
                };

                await db.AddAsync(package);
            }

            const int PackagesCount = 25;

            for (var i = 0; i < PackagesCount; i++)
            {
                var package = new Package
                {
                    RecipientId = secondUser.Id,
                    Status = Status.Pending
                };

                await db.AddAsync(package);
            }

            await db.SaveChangesAsync();

            var actualPendingPackages = await packageService.PendingForUserAsync(secondUser);

            actualPendingPackages
                .Should()
                .HaveCount(PackagesCount);
        }

        [Fact]
        public void ShipAsyncShouldThrowInvalidOperationExceptionIfPackageIsNotFound()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            Func<Task> func = async () => await packageService.ShipAsync(55);

            func.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async Task ShipAsyncShouldMarkGivenPackageAsShipped()
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
                Weight = Weight,
                Status = Status.Pending
            };

            await db.AddAsync(package);
            await db.SaveChangesAsync();

            var createdPackage = await db.Packages.FirstOrDefaultAsync();

            await packageService.ShipAsync(createdPackage.Id);

            var actualPackage = await db.Packages.FirstOrDefaultAsync();

            actualPackage
                .Status
                .Should()
                .Be(Status.Shipped);
        }

        [Fact]
        public void ShippedForUserAsyncShouldThrowInvalidOperationExceptionIfUserIsNull()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            Func<Task> func = async () => await packageService.ShippedForUserAsync(null);

            func.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async Task ShippedForUserAsyncShouldReturnAllShippedPackagesForGivenUser()
        {
            var db = DbInfrastructure.GetDatabase();

            var mockUserManager = this.GetUserManagerMock();

            mockUserManager
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns((string id) => db.Users.SingleOrDefaultAsync(u => u.Id == id));

            var receiptService = new Mock<IReceiptService>().Object;

            var packageService = new PackageService(db, receiptService, mockUserManager.Object);

            var firstUser = new User
            {
                UserName = "Georgi"
            };

            var secondUser = new User
            {
                UserName = "Ivana"
            };

            await db.AddRangeAsync(firstUser, secondUser);
            await db.SaveChangesAsync();

            for (var i = 0; i < 100; i++)
            {
                var package = new Package
                {
                    RecipientId = firstUser.Id,
                    Status = Status.Delivered
                };

                await db.AddAsync(package);
            }

            const int PackagesCount = 25;

            var ids = new int[PackagesCount];

            for (var i = 0; i < PackagesCount; i++)
            {
                var package = new Package
                {
                    RecipientId = secondUser.Id,
                    Status = Status.Pending
                };

                await db.AddAsync(package);
                await db.SaveChangesAsync();

                ids[i] = package.Id;
            }

            for (var i = 0; i < PackagesCount; i++)
            {
                await packageService.ShipAsync(ids[i]);
            }

            var actualShippedPackages = await packageService.ShippedForUserAsync(secondUser);

            actualShippedPackages
                .Should()
                .HaveCount(PackagesCount);
        }

        private static async Task AddPackagesToDb(PandaDbContext db)
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