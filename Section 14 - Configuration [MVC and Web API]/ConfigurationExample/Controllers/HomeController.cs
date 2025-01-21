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
            //ViewBag.ClientID = _configuration["weatherapi:ClientID"];
            //ViewBag.ClientSecret = _configuration.GetValue("weatherapi:ClientSecret", "the default client secret"); //added fallback value if the key is missing


            //or
            ViewBag.ClientID = _configuration.GetSection("weatherapi")["ClientID"];
            ViewBag.ClientSecret = _configuration.GetSection("weatherapi")["ClientSecret"];

            return View();
        }
    }
}
