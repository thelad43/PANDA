namespace Panda.Services
{
    using Models;
    using Panda.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IReceiptService
    {
        Task<IEnumerable<ReceiptServiceModel>> ForUserAsync(User user);
    }
}