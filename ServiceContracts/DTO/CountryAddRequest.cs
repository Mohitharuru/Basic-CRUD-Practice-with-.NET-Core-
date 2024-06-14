using System;
using System.Collections.Generic;
using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class for adding new country 
    /// </summary>
    
    //when ever we need to add a country then we need to add country name to this dto object. now we have the country name in CountryAddRequest
    //Now we need to assign this value to the country.cs the entity in order to make country entry in the database. 
    public class CountryAddRequest
    {
        public string? CountryName { get; set; }

        public Country ToCountry()
        {
            return new Country()
            {
                CountryName = CountryName
            };
        }
    }
}
