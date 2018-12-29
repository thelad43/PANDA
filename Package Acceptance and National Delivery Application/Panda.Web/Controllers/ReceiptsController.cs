namespace Panda.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Panda.Models;
    using Services;
    using System.Threading.Tasks;

    [Authorize]
    public class ReceiptsController : Controller
    {
        private readonly IReceiptService receipts;
        private readonly UserManager<User> userManager;

        public ReceiptsController(IReceiptService receipts, UserManager<User> userManager)
        {
            this.receipts = receipts;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await this.receipts.ForUserAsync(await this.GetUser()));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
            => View(await this.receipts.ByIdAsync(await this.GetUser(), id));

        private async Task<User> GetUser()
            => await this.userManager.GetUserAsync(HttpContext.User);
    }
}