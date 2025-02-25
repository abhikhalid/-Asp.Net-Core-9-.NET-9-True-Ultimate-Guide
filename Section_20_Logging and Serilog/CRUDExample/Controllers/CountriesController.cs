using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace CRUDExample.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        private readonly ICountriesUploaderService _countriesUploaderService;
        public CountriesController(ICountriesUploaderService countriesUploaderService) {
            _countriesUploaderService = countriesUploaderService;
        }


        [Route("UploadFromExcel")]
        public IActionResult UploadFromExcel()
        {
            return View();
        }

        [HttpPost]
        [Route("UploadFromExcel")]
        public async Task<IActionResult> UploadFromExcel(IFormFile excelFile)
        {
            if(excelFile == null || excelFile.Length  == 0) //no file is submitted by the user 
            {
                ViewBag.ErrorMessage = "Please select an xlsx file";
                return View();
            }

            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx",StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMessage = "Unsupported file. 'xlsx' file is expected";
                return View();
            }

            int countriesCountInserted = await _countriesUploaderService.UploadCountriesFromExcelFile(excelFile);

            ViewBag.Message = $"{countriesCountInserted} Countries Uploaded";
            return View();
        }
    }
}
