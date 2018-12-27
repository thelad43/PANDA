namespace Panda.Services.Implementations
{
    using Common.Mapping;
    using Data;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Panda.Models;
    using Panda.Models.Enums;
    using Services.Models.Admin;
    using Services.Models.Package;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PackageService : IPackageService
    {
        private readonly PandaDbContext db;
        private readonly UserManager<User> userManager;

        public PackageService(PandaDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<AdminPackageServiceModel>> AllDeliveredAsync()
         => await this.db
             .Packages
             .Where(p => p.Status == Status.Delivered)
             .To<AdminPackageServiceModel>()
             .ToListAsync();

        public async Task<IEnumerable<AdminPackageServiceModel>> AllPendingAsync()
          => await this.db
              .Packages
              .Where(p => p.Status == Status.Pending)
              .To<AdminPackageServiceModel>()
              .ToListAsync();

        public async Task<IEnumerable<AdminPackageServiceModel>> AllShippedAsync()
            => await this.db
                .Packages
                .Where(p => p.Status == Status.Shipped)
                .To<AdminPackageServiceModel>()
                .ToListAsync();

        public async Task<PackageDetailsServiceModel> DeliverAsync(int id)
        {
            var package = await this.db
                .Packages
                .FirstOrDefaultAsync(p => p.Id == id);

            if (package == null)
            {
                throw new InvalidOperationException();
            }

            package.Status = Status.Delivered;

            await this.db.SaveChangesAsync();

            var model = await this.db.
                Packages
                .Where(p => p.Id == id)
                .To<PackageDetailsServiceModel>()
                .FirstOrDefaultAsync();

            return model;
        }

        public async Task<IEnumerable<PackageListingServiceModel>> DeliveredForUserAsync(User user)
        {
            if (user == null)
            {
                throw new InvalidOperationException();
            }

            var packages = await this.db
                .Packages
                .Where(p => p.RecipientId == user.Id && p.Status == Status.Delivered)
                .To<PackageListingServiceModel>()
                .ToListAsync();

            return packages;
        }

        public async Task<PackageDetailsServiceModel> DetailsAsync(int id)
        {
            var package = await this.db
              .Packages
              .Where(p => p.Id == id)
              .To<PackageDetailsServiceModel>()
              .FirstOrDefaultAsync();

            if (package == null)
            {
                throw new InvalidOperationException();
            }

            return package;
        }

        public async Task<PackageDetailsServiceModel> DetailsByUserAsync(User user, int id)
        {
            var package = await this.db
                .Packages
                .Where(p => p.Id == id && p.RecipientId == user.Id)
                .To<PackageDetailsServiceModel>()
                .FirstOrDefaultAsync();

            if (package == null)
            {
                throw new InvalidOperationException();
            }

            return package;
        }

        public async Task<IEnumerable<PackageListingServiceModel>> PendingForUserAsync(User user)
        {
            if (user == null)
            {
                throw new InvalidOperationException();
            }

            var packages = await this.db
                .Packages
                .Where(p => p.RecipientId == user.Id && p.Status == Status.Pending)
                .To<PackageListingServiceModel>()
                .ToListAsync();

            return packages;
        }

        public async Task<PackageDetailsServiceModel> ShipAsync(int id)
        {
            var package = await this.db
                .Packages
                .FirstOrDefaultAsync(p => p.Id == id);

            if (package == null)
            {
                throw new InvalidOperationException();
            }

            package.Status = Status.Shipped;

            await this.db.SaveChangesAsync();

            var model = await this.db.
                Packages
                .Where(p => p.Id == id)
                .To<PackageDetailsServiceModel>()
                .FirstOrDefaultAsync();

            return model;
        }

        public async Task<IEnumerable<PackageListingServiceModel>> ShippedForUserAsync(User user)
        {
            if (user == null)
            {
                throw new InvalidOperationException();
            }

            var packages = await this.db
                .Packages
                .Where(p => p.RecipientId == user.Id && p.Status == Status.Shipped)
                .To<PackageListingServiceModel>()
                .ToListAsync();

            return packages;
        }
    }
}