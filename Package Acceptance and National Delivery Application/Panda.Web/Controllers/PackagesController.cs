namespace Panda.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Panda.Models;
    using Panda.Services;
    using Panda.Web.Infrastructure.Extensions;
    using System.Threading.Tasks;

    [Authorize]
    public class PackagesController : Controller
    {
        private readonly IPackageService packages;
        private readonly UserManager<User> userManager;

        public PackagesController(IPackageService packages, UserManager<User> userManager)
        {
            this.packages = packages;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
            => View(await this.packages.DetailsByUserAsync(await this.GetUser(), id));

        [HttpPost]
        public async Task<IActionResult> Acquire(int id)
        {
            await this.packages.AcquireAsync(id);
            TempData.AddSuccessMessage("Package successfully acquired.");
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller", string.Empty));
        }

        private async Task<User> GetUser()
            => await this.userManager.GetUserAsync(HttpContext.User);
    }
}