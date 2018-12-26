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

            return receipt;
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