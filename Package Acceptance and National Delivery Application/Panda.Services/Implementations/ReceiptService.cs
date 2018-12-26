namespace Panda.Services.Implementations
{
    using Models;
    using Panda.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ReceiptService : IReceiptService
    {
        public Task<IEnumerable<ReceiptServiceModel>> ForUserAsync(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}