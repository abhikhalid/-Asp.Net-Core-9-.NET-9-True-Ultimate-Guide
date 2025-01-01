using Microsoft.AspNetCore.Mvc;

namespace Controllers_Example.Controllers
{
    //this class needs to be public then only it can be instantiated by ASP.NET CORE Internally.
    public class HomeController
    {
        //this is called attribute routing
        [Route("/")]
        // this method could return anything
        public string method1()
        {
            return "Hello from method1!";
        }
    }
}
