using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        [Route("admin/home/index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}


