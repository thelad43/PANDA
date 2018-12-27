namespace Panda.Web.Areas.Admin.Controllers
{
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Models;
    using Panda.Models;
    using Panda.Models.Enums;
    using Panda.Services;
    using Panda.Services.Models.Package;
    using System.Linq;
    using System.Threading.Tasks;

    public class PackagesController : BaseAdminController
    {
        private readonly IPackageService packages;
        private readonly UserManager<User> userManager;

        public PackagesController(IPackageService packages, UserManager<User> userManager)
        {
            this.packages = packages;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var users = this.userManager.Users.ToList();

            var model = new AdminCreatePackageViewModel
            {
                AllUsers = users.Select(u => new SelectListItem
                {
                    Text = u.UserName,
                    Value = u.Id
                })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminCreatePackageViewModel model)
        {
            var package = await this.packages
                 .CreateAsync(
                 model.Description,
                 model.Weigth,
                 model.ShippingAddress,
                 model.RecipientId);

            TempData.AddSuccessMessage("Successfully created package.");

            var viewModel = new PackageDetailsServiceModel
            {
                Description = package.Description,
                EstimatedDeliveryTime = package.EstimatedDeliveryDate,
                RecipientName = package.RecipientName,
                ShippingAddress = package.ShippingAddress,
                Status = Status.Pending,
                Weight = package.Weight
            };

            return View(nameof(Details), viewModel);
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