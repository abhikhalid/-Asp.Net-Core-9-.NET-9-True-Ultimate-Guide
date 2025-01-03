using Microsoft.AspNetCore.Mvc;

namespace Controllers_Example.Controllers
{
    [Controller]
    public class StoreController : Controller
    {
        [Route("store/books/{id}")]
        public IActionResult Books()
        {
            int id = Convert.ToInt32(Request.RouteValues["id"]);
            return Content($"<h1>Book Store {id}</h1>","text/html");
        }
    }
}
