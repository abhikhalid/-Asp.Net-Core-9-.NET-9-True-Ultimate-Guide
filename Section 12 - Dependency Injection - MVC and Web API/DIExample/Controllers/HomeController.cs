using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;

namespace DIExample.Controllers
{
    public class HomeController : Controller
    {
        public readonly ICitiesService _citiesService1;
        public readonly ICitiesService _citiesService2;
        public readonly ICitiesService _citiesService3;

        //constructor injection
        public HomeController(ICitiesService citiesService1, ICitiesService citiesService2, ICitiesService citiesService3)
        {

            _citiesService1 = citiesService1;
            _citiesService2 = citiesService2;
            _citiesService3 = citiesService3;
        }


        [Route("/")]
        public IActionResult Index()
        {
            List<string> cities = _citiesService1.GetCities();

            ViewBag.InstanceId_CitiesService1 = _citiesService1.ServiceInstanceId;
            ViewBag.InstanceId_CitiesService2 = _citiesService2.ServiceInstanceId;
            ViewBag.InstanceId_CitiesService3 = _citiesService3.ServiceInstanceId;

            return View(cities);
        }
    }
}
