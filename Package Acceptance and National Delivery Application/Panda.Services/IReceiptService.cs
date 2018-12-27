namespace Panda.Services
{
    using Models.Receipt;
    using Panda.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IReceiptService
    {
        Task<ReceiptDetailsServiceModel> ById(User user, int id);

        Task<IEnumerable<ReceiptServiceModel>> ForUserAsync(User user);
    }
}