using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;

namespace DIExample.Controllers
{
    public class HomeController : Controller
    {
        //public readonly ICitiesService _citiesService;

        //constructor injection
        //public HomeController(ICitiesService citiesService)
        //{

        //    _citiesService = citiesService;
        //}


        [Route("/")]
        //method injection
        public IActionResult Index([FromServices]ICitiesService _citiesService)
        {
            //we are in the controller, without worrying about where actual citiesService are coming, we can simply demand the service.
            //now, it is responsibility of the service to provide the data.
            List<string> cities = _citiesService.GetCities();
            //now we can supply the cities to the view so that the view can be strongly typed view.
            return View(cities);
        }
    }
}
