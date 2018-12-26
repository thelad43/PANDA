namespace Panda.Web.Models.Package
{
    using Panda.Services.Models.Package;
    using System.Collections.Generic;

    public class PackageListingViewModel
    {
        public bool IsAuthenticated { get; set; }

        public IEnumerable<PackageListingServiceModel> Pending { get; set; }

        public IEnumerable<PackageListingServiceModel> Shipped { get; set; }

        public IEnumerable<PackageListingServiceModel> Delivered { get; set; }
    }
}