namespace Panda.Services
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPackageService
    {
        Task<IEnumerable<PackageListingServiceModel>> PendingForUserAsync(string username);

        Task<IEnumerable<PackageListingServiceModel>> ShippedForUserAsync(string username);

        Task<IEnumerable<PackageListingServiceModel>> DeliveredForUser(string username);
    }
}