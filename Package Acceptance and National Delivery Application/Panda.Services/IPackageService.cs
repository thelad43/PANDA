namespace Panda.Services
{
    using Models.Admin;
    using Models.Package;
    using Panda.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPackageService
    {
        Task<AdminPackageServiceModel> CreateAsync(
            string description,
            double weigth,
            string shippingAddress,
            string recipientId);

        Task AcquireAsync(int id);

        Task<IEnumerable<AdminPackageServiceModel>> AllDeliveredAsync();

        Task<IEnumerable<AdminPackageServiceModel>> AllPendingAsync();

        Task<IEnumerable<AdminPackageServiceModel>> AllShippedAsync();

        Task<PackageDetailsServiceModel> DeliverAsync(int id);

        Task<IEnumerable<PackageListingServiceModel>> DeliveredForUserAsync(User user);

        Task<PackageDetailsServiceModel> DetailsAsync(int id);

        Task<PackageDetailsServiceModel> DetailsByUserAsync(User user, int id);

        Task<IEnumerable<PackageListingServiceModel>> PendingForUserAsync(User user);

        Task<PackageDetailsServiceModel> ShipAsync(int id);

        Task<IEnumerable<PackageListingServiceModel>> ShippedForUserAsync(User user);
    }
}