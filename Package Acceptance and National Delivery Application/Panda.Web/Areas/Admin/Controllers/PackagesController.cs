namespace Panda.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Panda.Services;
    using Panda.Web.Infrastructure.Extensions;
    using System.Threading.Tasks;

    public class PackagesController : BaseAdminController
    {
        private readonly IPackageService packages;

        public PackagesController(IPackageService packages)
        {
            this.packages = packages;
        }

        [HttpPost]
        public async Task<IActionResult> Ship(int id)
        {
            var model = await this.packages.ShipAsync(id);

            TempData.AddSuccessMessage("Package successfully shipped.");

            return View(nameof(Details), model);
        }

        [HttpPost]
        public async Task<IActionResult> Deliver(int id)
        {
            var model = await this.packages.DeliverAsync(id);

            TempData.AddSuccessMessage("Package successfully delivered.");

            return View(nameof(Details), model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
            => View(await this.packages.DetailsAsync(id));

        [HttpGet]
        public async Task<IActionResult> Pending()
             => View(await this.packages.AllPendingAsync());

        [HttpGet]
        public async Task<IActionResult> Shipped()
            => View(await this.packages.AllShippedAsync());

        [HttpGet]
        public async Task<IActionResult> Delivered()
            => View(await this.packages.AllDeliveredAsync());
    }
}