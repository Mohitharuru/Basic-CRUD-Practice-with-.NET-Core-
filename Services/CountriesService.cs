using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RepositaryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {

        //private list
        private readonly ICountriesRepositary _countriesRepository;

        //constructor to initilize the field
        public CountriesService(ICountriesRepositary countriesRepositary)
        {
            _countriesRepository = countriesRepositary;
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {

            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            if (await _countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName) != null)
            {
                throw new ArgumentException("Given country name already exist");
            }

            //convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();

            //generate CountryID
            country.CountryID = Guid.NewGuid();
            //Add country object to _countries
            await _countriesRepository.AddCountry(country);
            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()  
        {
            return (await _countriesRepository.GetAllCountries()).Select(country => country.ToCountryResponse()).ToList();
        }

        public async Task<CountryResponse?> GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null)
            {
                return null;
            }

            Country? country_response_from_list = await _countriesRepository.GetCountryByCountryID(countryID.Value);

            if (country_response_from_list == null) { return null; }
            return country_response_from_list.ToCountryResponse();
        }

        public async Task<int> UploadCountriesFromExcelFile(IFormFile formFiles)
        {
            MemoryStream memoryStream = new MemoryStream();
            await formFiles.CopyToAsync(memoryStream);

            int countriesCount = 0;
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Countries"];
                int rowCount = worksheet.Dimension.Rows;
                for (int row = 2; row <= rowCount; row++)
                {
                    string? cellValue = Convert.ToString(worksheet.Cells[row, 1].Value);
                    if (cellValue != null)
                    {
                        string countryName = cellValue;
                        if (await _countriesRepository.GetCountryByCountryName(countryName) == null)
                        {
                            Country country = new Country()
                            {
                                CountryName = countryName,
                            };
                            await _countriesRepository.AddCountry(country);
                            countriesCount++;
                        }
                    }
                }
            }
            return countriesCount;
        }
    }
}
