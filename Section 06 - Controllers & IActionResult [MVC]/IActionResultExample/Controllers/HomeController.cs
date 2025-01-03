using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers
{
    public class HomeController : Controller
    {
        //https://localhost:7119?bookid=123&isloggedin=true
        [Route("bookstore/{bookid?}/{isLoggedIn?}")]
        [Route("/")]
        public IActionResult Index(int? bookid, bool? isLoggedIn)
        {
            //Book id should be applied
            if (bookid.HasValue == false)
            {
                return BadRequest("Book id is not supplied or empty");
            }

            //Book id should be between 1 to 1000
            //int bookId = Convert.ToInt16(ControllerContext.HttpContext.Request.Query["bookid"]);
            if(bookid <= 0)
            {
                //Response.StatusCode = 400;
                //return Content("Book id can't be less or equal to zero");
                return BadRequest("Book id can't be less or equal to zero");
            }

            if(bookid > 1000)
            {
                //Response.StatusCode = 404;
                //return Content("Book id can't be greater than  1000");
                return NotFound("Book id can't be greater than 1000");
            }

            //isloggedin should be true
            //if (Convert.ToBoolean(Request.Query["isloggedin"]) == false)
            if (isLoggedIn == false)
            {
                //Response.StatusCode = 401;
                //return Content("User must be authenticated");
                return Unauthorized("User must be authenticated");
            }

            return Content($"Book id: {bookid}","text/plain"); 
        }
    }
}
