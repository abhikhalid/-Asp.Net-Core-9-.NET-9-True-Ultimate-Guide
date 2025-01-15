using Microsoft.AspNetCore.Mvc;

namespace Section_08_Razor_Views.Controllers
{
    public class ProductsController : Controller
    {
        [Route("products/all")]
        public IActionResult All()
        {
            return View();
            //Views/Products/All.cshtml
            //Views/Shared/All.cshtml
        }
    }
}
