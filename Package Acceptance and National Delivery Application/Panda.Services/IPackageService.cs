namespace Panda.Services
{
    using Panda.Models;
    using Panda.Services.Models.Package;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPackageService
    {
        Task<PackageDetailsServiceModel> DetailsByUser(User user, int id);

        Task<IEnumerable<PackageListingServiceModel>> PendingForUserAsync(User user);

        Task<IEnumerable<PackageListingServiceModel>> ShippedForUserAsync(User user);

        Task<IEnumerable<PackageListingServiceModel>> DeliveredForUserAsync(User user);
    }
}