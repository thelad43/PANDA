namespace Panda.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Panda.Services;
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Deliver(int id)
        {
            return View();
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