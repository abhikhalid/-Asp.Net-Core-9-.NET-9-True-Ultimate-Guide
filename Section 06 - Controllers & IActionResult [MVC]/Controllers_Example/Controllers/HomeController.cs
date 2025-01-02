using Controllers_Example.Models;
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


        [Route("person")]
        public JsonResult Person()
        {
            Person person = new Person()
            {
                Id = Guid.NewGuid(),
                FirstName = "Khalid",
                LastName = "Mahmud"
            };

            //return new JsonResult(person);
            //or
            return Json(person);
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

        [Route("file-download")]
        public VirtualFileResult FileDownload()
        {
            //relative path from the wwwroot folder
            //return new VirtualFileResult("/sample.pdf","application/pdf");
            //or
            //(internally it does the same)
            return File("/sample.pdf", "application/pdf");
        }
        
        [Route("file-download2")]
        public PhysicalFileResult FileDownload2()
        {
            //actual path
            //return new PhysicalFileResult("D:\\Cracking-the-Coding-Interview-6th-Edition-189-Programming-Questions-and-Solutions.pdf", "application/pdf");
            return PhysicalFile("D:\\Cracking-the-Coding-Interview-6th-Edition-189-Programming-Questions-and-Solutions.pdf", "application/pdf");
        }

        [Route("file-download3")]
        public IActionResult FileDownload3()
        {
            byte[] bytes = System.IO.File.ReadAllBytes("D:\\Cracking-the-Coding-Interview-6th-Edition-189-Programming-Questions-and-Solutions.pdf");
                
            //return new FileContentResult(bytes, "application/pdf");
            return File(bytes, "application/pdf");
        }
    }
}
