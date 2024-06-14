using Entities;
using System;

namespace RepositaryContracts
{
    /// <summary>
    /// Represents data access logic for managing country entity
    /// </summary>
    public interface ICountriesRepositary
    {
        /// <summary>
        /// Add a new country object to data store
        /// </summary>
        /// <param name="country">Country object to add</param>
        /// <returns>returns the country object after adding it to the data store</returns>
        Task<Country> AddCountry(Country country);

        /// <summary>
        /// returns all countries in the data store
        /// </summary>
        /// <returns>All countries from the table</returns>
        Task<List<Country>> GetAllCountries();

        /// <summary>
        /// Returns a country object based on the given country id, otherwise it returns null 
        /// </summary>
        /// <param name="countryID">countryID to search</param>
        /// <returns>Matching country or null</returns>
        Task<Country?> GetCountryByCountryID(Guid countryID);

        /// <summary>
        /// Returns a country object based on the given country name, otherwise it returns null
        /// </summary>
        /// <param name="countryName">country name to search</param>
        /// <returns>Matching country or null </returns>
        Task<Country?> GetCountryByCountryName(string countryName);
    }
}
