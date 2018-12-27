namespace Panda.Services.Implementations
{
    using Microsoft.EntityFrameworkCore;
    using Panda.Common.Mapping;
    using Panda.Data;
    using Panda.Models;
    using Panda.Services.Models.Receipt;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReceiptService : IReceiptService
    {
        private readonly PandaDbContext db;

        public ReceiptService(PandaDbContext db)
        {
            this.db = db;
        }

        public async Task<ReceiptDetailsServiceModel> ById(User user, int id)
        {
            if (user == null)
            {
                throw new InvalidOperationException();
            }

            var receipt = await this.db
                .Receipts
                .Where(r => r.RecipientId == user.Id && r.Id == id)
                .To<ReceiptDetailsServiceModel>()
                .FirstOrDefaultAsync();

            if (receipt == null)
            {
                throw new InvalidOperationException();
            }

            return receipt;
        }

        public async Task CreateAsync(decimal fee, int packageId, string recipientId)
        {
            var receipt = new Receipt
            {
                Fee = fee,
                IssuedOn = DateTime.UtcNow,
                PackageId = packageId,
                RecipientId = recipientId
            };

            await this.db.AddAsync(receipt);
            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ReceiptServiceModel>> ForUserAsync(User user)
        {
            if (user == null)
            {
                throw new InvalidOperationException();
            }

            var receipts = await this.db
                .Receipts
                .Where(r => r.RecipientId == user.Id)
                .To<ReceiptServiceModel>()
                .ToListAsync();

            return receipts;
        }
    }
}