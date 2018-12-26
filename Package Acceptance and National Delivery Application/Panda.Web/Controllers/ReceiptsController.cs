namespace Panda.Web.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Panda.Models;
    using Panda.Services;
    using System.Threading.Tasks;

    public class ReceiptsController : Controller
    {
        private readonly IReceiptService receipts;
        private readonly UserManager<User> userManager;

        public ReceiptsController(IReceiptService receipts, UserManager<User> userManager)
        {
            this.receipts = receipts;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await this.receipts.ForUserAsync(await this.GetUser()));
        }

        public async Task<IActionResult> Details(int id)
            => View(await this.receipts.ById(await this.GetUser(), id));

        private async Task<User> GetUser()
            => await this.userManager.GetUserAsync(HttpContext.User);
    }
}