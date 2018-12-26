namespace Panda.Web.Models.Package
{
    using Services.Models;
    using System.Collections.Generic;

    public class PackageListingViewModel
    {
        public bool IsAuthenticated { get; set; }

        public IEnumerable<PackageListingServiceModel> Pending { get; set; }

        public IEnumerable<PackageListingServiceModel> Shipped { get; set; }

        public IEnumerable<PackageListingServiceModel> Delivered { get; set; }
    }
}