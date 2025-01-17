using Microsoft.AspNetCore.Mvc;

namespace ViewComponents_Example.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
        
        [Route("about")]
        public IActionResult About()
        {
            return View();
        }
    }
}
