﻿namespace Panda.Services
{
    using Models.Receipt;
    using Panda.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IReceiptService
    {
        Task CreateAsync(decimal fee, int packageId, string recipientId);

        Task<ReceiptDetailsServiceModel> ByIdAsync(User user, int id);

        Task<IEnumerable<ReceiptServiceModel>> ForUserAsync(User user);
    }
}