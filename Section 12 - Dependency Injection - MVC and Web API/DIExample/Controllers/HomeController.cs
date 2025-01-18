using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;

namespace DIExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICitiesService _citiesService1;
        private readonly ICitiesService _citiesService2;
        private readonly ICitiesService _citiesService3;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        //constructor injection
        public HomeController(ICitiesService citiesService1, ICitiesService citiesService2, ICitiesService citiesService3, IServiceScopeFactory serviceScopeFactory)
        {

            _citiesService1 = citiesService1;
            _citiesService2 = citiesService2;
            _citiesService3 = citiesService3;
            _serviceScopeFactory = serviceScopeFactory;
        }


        [Route("/")]
        public IActionResult Index()
        {
            List<string> cities = _citiesService1.GetCities();

            ViewBag.InstanceId_CitiesService1 = _citiesService1.ServiceInstanceId;
            ViewBag.InstanceId_CitiesService2 = _citiesService2.ServiceInstanceId;
            ViewBag.InstanceId_CitiesService3 = _citiesService3.ServiceInstanceId;

            //since we are using the 'using' block, it will automatically call the 'dispose' method
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                //assume we have injected 'CitiesService' here. 
                //so automatically it will call the constructor and open a new database connection.
                ICitiesService citiesService =  scope.ServiceProvider.GetRequiredService<ICitiesService>();

                ViewBag.InstanceId_CitiesService_InScope = citiesService.ServiceInstanceId;

                //DB WORK
            }
            //end of the scope
            //so the database connection will be closed automatically. It calls the CitiesService.Dispose() method 

            return View(cities);
        }
    }
}
