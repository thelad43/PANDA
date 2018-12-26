﻿namespace Panda.Services
{
    using Models;
    using Panda.Models;
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