namespace Panda.Web.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Panda.Models;
    using Panda.Services;
    using System.Threading.Tasks;

    public class PackagesController : Controller
    {
        private readonly IPackageService packages;
        private readonly UserManager<User> userManager;

        public PackagesController(IPackageService packages, UserManager<User> userManager)
        {
            this.packages = packages;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Details(int id)
            => View(await this.packages.DetailsByUser(await this.GetUser(), id));

        private async Task<User> GetUser()
            => await this.userManager.GetUserAsync(HttpContext.User);
    }
}