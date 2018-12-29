namespace Panda.Tests.Services
{
    using FluentAssertions;
    using Panda.Models;
    using Panda.Services.Implementations;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class ReceiptServiceTests
    {
        public ReceiptServiceTests()
        {
            Tests.Initialize();
        }

        [Fact]
        public async Task CreateAsyncShouldCreateReceiptAndSaveItToDb()
        {
            var db = DbInfrastructure.GetDatabase();
            var receiptService = new ReceiptService(db);

            const decimal Fee = 5.23M;
            const string UserId = "UserId";

            await receiptService.CreateAsync(5.23M, 1, UserId);

            var receipt = await db.Receipts.FindAsync(1);

            receipt
                .Should()
                .NotBeNull();

            receipt
                .As<Receipt>()
                .RecipientId.Should().Be(UserId);

            receipt
                .As<Receipt>()
                .Fee.Should().Be(Fee);
        }

        [Fact]
        public void ByIdAsyncShouldThrowInvalidOperationExceptionIfUserIsNull()
        {
            var db = DbInfrastructure.GetDatabase();
            var receiptService = new ReceiptService(db);

            Func<Task> func = async () => await receiptService.ByIdAsync(null, 1);

            func.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async Task ByIdAsyncShouldThrowInvalidOperationExceptionIfReceiptIsNotFound()
        {
            var db = DbInfrastructure.GetDatabase();
            var receiptService = new ReceiptService(db);

            var user = new User
            {
                UserName = "User"
            };

            await db.AddAsync(user);
            await db.SaveChangesAsync();

            Func<Task> func = async () => await receiptService.ByIdAsync(user, 1);

            func.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void ForUserAsyncShouldThrowInvalidOperationExceptionIfUserIsNull()
        {
            var db = DbInfrastructure.GetDatabase();
            var receiptService = new ReceiptService(db);

            Func<Task> func = async () => await receiptService.ForUserAsync(null);

            func.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async Task ForUserAsyncShouldReturnAllReceiptsForGivenUser()
        {
            var db = DbInfrastructure.GetDatabase();
            var receiptService = new ReceiptService(db);

            const string FirstUsername = "UserFirstname";

            var firstUser = new User
            {
                UserName = FirstUsername
            };

            var secondUser = new User
            {
                UserName = "Second"
            };

            await db.AddRangeAsync(firstUser, secondUser);
            await db.SaveChangesAsync();

            for (var i = 0; i < 3; i++)
            {
                var receipt = new Receipt
                {
                    Fee = (i + 1) * 10,
                    PackageId = i + 1,
                    RecipientId = firstUser.Id
                };

                await db.AddAsync(receipt);
            }

            for (var i = 0; i < 10; i++)
            {
                var receipt = new Receipt
                {
                    Fee = (i + 1) * 10,
                    PackageId = i + 1,
                    RecipientId = secondUser.Id
                };

                await db.AddAsync(receipt);
            }

            await db.SaveChangesAsync();

            var receipts = await receiptService.ForUserAsync(firstUser);

            receipts.Should().HaveCount(3);

            receipts.Should()
                .Match(r => r.ElementAt(0).Fee == 10).And.Match(r => r.ElementAt(0).RecipientName == FirstUsername)
                .And
                .Match(r => r.ElementAt(1).Fee == 20).And.Match(r => r.ElementAt(1).RecipientName == FirstUsername)
                .And
                .Match(r => r.ElementAt(2).Fee == 30).And.Match(r => r.ElementAt(2).RecipientName == FirstUsername);
        }
    }
}