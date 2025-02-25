using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesUploaderService : ICountriesUploaderService
    {
        private readonly ICountriesRepository _countriesRepository;

        //injecting 'ApplicationDbContext'
        public CountriesUploaderService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }

        public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            int countriesInserted = 0;

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                //as a developer, we have to provide a template for the end user.
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Countries"];
                int rowCount = worksheet.Dimension.Rows;
                //row no 1 is header row. so we start from row no 2
                for(int row = 2; row<=rowCount; row++)
                {
                   string? cellVaue = Convert.ToString(worksheet.Cells[row, 1].Value);
                   
                   if(!string.IsNullOrEmpty(cellVaue))
                   {
                        string countryName = cellVaue;

                        if(_countriesRepository.GetCountryByCountryName(countryName) !=null)
                        {
                            Country country = new Country()
                            {
                                CountryName = countryName,
                            };

                            await _countriesRepository.AddCountry(country);   
                            countriesInserted++;
                        }
                   }           
                }
                
            }
            return countriesInserted;
        }
    }
}