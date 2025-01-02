using Microsoft.AspNetCore.Mvc;

namespace Controllers_Example.Controllers
{
    //this class needs to be public then only it can be instantiated by ASP.NET CORE Internally.
    public class HomeController : Controller
    {
        //this is called attribute routing
        [Route("/")]
        [Route("home")]
        // this method could return anything
        public ContentResult Index() //first action method name is Index as per the convention.
        {
            //return new ContentResult()
            //{
            //    Content = "Hello from Index",
            //    ContentType= "text/plain"
            //};

            //or
            //return Content("Hello from Index", "text/plain");
            return Content("<h1>Welcome</h1> <h2>Hello from Index</h2>","text/html");
        }

        [Route("about")]
        public string About()
        {
            return "About Page";
        }

        [Route("contact-us/{{mobile:regex(^\\d{10}$)}}")]
        public string Contact()
        {
            return "Contact Page";
        }
    }
}
