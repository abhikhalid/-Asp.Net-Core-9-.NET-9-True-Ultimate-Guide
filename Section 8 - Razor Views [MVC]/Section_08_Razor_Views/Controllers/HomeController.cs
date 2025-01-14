using Microsoft.AspNetCore.Mvc;

namespace Section_08_Razor_Views.Controllers
{
    public class HomeController : Controller
    {
        [Route("home")]
        [Route("/")]
        public IActionResult Index()
        {
            // if you don't specify the view name, it will look for a view with the same name as the action method. (Index.cshtml)
            // default location is Views/Home/Index.cshtml
            return View(); 
            //return View("abc"); //abc.cshtml
            //return new ViewResult { ViewName = "abc" }; //abc.cshtml
        }
    }
}
