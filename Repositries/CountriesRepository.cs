﻿using Entities;
using Microsoft.EntityFrameworkCore;
using RepositaryContracts;

namespace Repositries
{
    public class CountriesRepository : RepositaryContracts.ICountriesRepositary
    {
        private readonly Entities.ApplicationDbContext _db;
        public CountriesRepository(Entities.ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Country> AddCountry(Country country)
        {
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();
            return country;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            return await _db.Countries.ToListAsync();
        }

        public async Task<Country?> GetCountryByCountryID(Guid countryID)
        {
            return await _db.Countries.FirstOrDefaultAsync(temp=>temp.CountryID == countryID);
        }

        public async Task<Country?> GetCountryByCountryName(string countryName)
        {
            return await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryName == countryName);
        }
    }
}
