namespace Panda.Services
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPackageService
    {
        Task<IEnumerable<PackageListingServiceModel>> PendingForUserAsync(string username);
    }
}