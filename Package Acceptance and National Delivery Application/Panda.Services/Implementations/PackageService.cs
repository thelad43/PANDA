﻿namespace Panda.Services.Implementations
{
    using Data;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Panda.Common.Mapping;
    using Panda.Models;
    using Panda.Models.Enums;
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
    }
}