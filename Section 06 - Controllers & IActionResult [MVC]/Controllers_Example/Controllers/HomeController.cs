using Microsoft.AspNetCore.Mvc;

namespace Controllers_Example.Controllers
{
    //this class needs to be public then only it can be instantiated by ASP.NET CORE Internally.
    public class HomeController
    {
        //this is called attribute routing
        [Route("/")]
        [Route("home")]
        // this method could return anything
        public string Index() //first action method name is Index as per the convention.
        {
            return "Hello from Index!";
        }

        [Route("about")]
        public string About()
        {
            return "About Page";
        }

        [Route("contact-us/{mobile:regex(^\\d{10}$)}")]
        public string Contact()
        {
            return "Contact Page";
        }
    }
}
