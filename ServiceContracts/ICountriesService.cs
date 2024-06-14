using ServiceContracts.DTO;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts
{
    /// <summary>
    /// Represents Business Logic for maniulating Country entity
    /// </summary>
    public interface ICountriesService
    {/// <summary>
    /// Add a country object to the list of countries 
    /// </summary>
    /// <param name="countryAddRequest">Country object to add</param>
    /// <returns>Returns country object after adding it (including newly generated country id)</returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// returns all countires from the list
        /// </summary>
        /// <returns>returns all countires from the list as list<CountryResponse></returns>
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// Returns a country object based on a countryID 
        /// </summary>
        /// <param name="countryID">CountryID (guid) to search</param>
        /// <returns>Matching country as CountryResponse Object</returns>
        Task<CountryResponse?> GetCountryByCountryID(Guid? countryID);

        /// <summary>
        /// Uploads countries from excel files into database
        /// </summary>
        /// <param name="formFiles">Excel files with list of countries</param>
        /// <returns>returns number of countries added</returns>
        Task<int> UploadCountriesFromExcelFile(IFormFile formFiles);
    }
}
