using Microsoft.AspNetCore.Mvc;

namespace CRUDExample.Controllers
{
    [Controller]
    public class CountriesController : Controller
    {
        [Route("UploadFromExcel")]
        public IActionResult UploadFromExcel()
        {
            return View();
        }
    }
}
