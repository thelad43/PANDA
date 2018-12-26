namespace Panda.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ReceiptsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}