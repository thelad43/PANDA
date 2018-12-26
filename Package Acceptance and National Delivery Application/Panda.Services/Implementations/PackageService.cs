namespace Panda.Services.Implementations
{
    using Models;
    using Data;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Panda.Models;
    using System;
    using System.Linq;
    using Panda.Common.Mapping;

    public class PackageService : IPackageService
    {
        private readonly PandaDbContext db;
        private readonly UserManager<User> userManager;

        public PackageService(PandaDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<PackageListingServiceModel>> PendingForUserAsync(string username)
        {
            var user = await this.userManager.FindByNameAsync(username);

            if (user == null)
            {
                throw new InvalidOperationException();
            }

            var packages = await this.db
                .Packages
                .Where(p => p.RecipientId == user.Id)
                .To<PackageListingServiceModel>()
                .ToListAsync();

            return packages;
        }
    }
}