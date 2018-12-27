namespace Panda.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Panda.Models;
    using Panda.Services;
    using Panda.Web.Models.Package;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class HomeController : Controller
    {
        private readonly IPackageService packages;
        private readonly UserManager<User> userManager;

        public HomeController(IPackageService packages, UserManager<User> userManager)
        {
            this.packages = packages;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await this.GetUser();

            if (user == null)
            {
                return View(new PackageListingViewModel { IsAuthenticated = false });
            }

            var pending = await this.packages.PendingForUserAsync(user);
            var shipped = await this.packages.ShippedForUserAsync(user);
            var delivered = await this.packages.DeliveredForUserAsync(user);

            var model = new PackageListingViewModel
            {
                Pending = pending,
                Shipped = shipped,
                Delivered = delivered,
                IsAuthenticated = true
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<User> GetUser()
            => await this.userManager.GetUserAsync(HttpContext.User);
    }
}