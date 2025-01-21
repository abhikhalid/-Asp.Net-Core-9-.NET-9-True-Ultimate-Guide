using Microsoft.AspNetCore.Mvc;

namespace ConfigurationExample.Controllers
{
    public class HomeController : Controller
    {
        //private field
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("/")]
        public IActionResult Index()
        {
            ViewBag.Mykey = _configuration["MyKey"];
            ViewBag.MyAPIkey = _configuration.GetValue("MyAPIKey", "the default key"); //added fallback value if the key is missing

            return View();
        }
    }
}
