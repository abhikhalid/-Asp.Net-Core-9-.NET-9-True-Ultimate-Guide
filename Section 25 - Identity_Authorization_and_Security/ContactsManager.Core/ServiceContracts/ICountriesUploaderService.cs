using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    ///  Represents business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesUploaderService
    {   
        /// <summary>
        /// Upload countries from excel file into Database
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        /// IFormFile is a file that is submitted from the web browser 

        Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
    }
}
