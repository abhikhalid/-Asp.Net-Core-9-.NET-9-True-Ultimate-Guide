using Microsoft.AspNetCore.Mvc;
using Services;

namespace DIExample.Controllers
{
    public class HomeController : Controller
    {
        public readonly CitiesService _citiesService;

        public HomeController()
        {
            //create object of CitiesService class
            _citiesService = new CitiesService();
        }


        [Route("/")]
        public IActionResult Index()
        {
            //we are in the controller, without worrying about where actual citiesService are coming, we can simply demand the service.
            //now, it is responsibility of the service to provide the data.
            List<string> cities = _citiesService.GetCities();
            //now we can supply the cities to the view so that the view can be strongly typed view.
            return View(cities);
        }
    }
}
