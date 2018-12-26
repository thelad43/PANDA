namespace Panda.Services
{
    using Panda.Models;
    using Panda.Services.Models.Receipt;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IReceiptService
    {
        Task<ReceiptDetailsServiceModel> ById(User user, int id);

        Task<IEnumerable<ReceiptServiceModel>> ForUserAsync(User user);
    }
}