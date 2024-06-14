using System;
using System.Collections.Generic;
using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
using Services;
using Xunit;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using Moq;
using FluentAssertions;
using AutoFixture;
using RepositaryContracts;

namespace CRUDTests
{
    public class CountiresServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly Mock<ICountriesRepositary> _countriesRepositoryMock;
        private readonly ICountriesRepositary _countriesRepository;
        private readonly Fixture _fixture;

        //controller
        public CountiresServiceTest()
        {
            _countriesRepositoryMock = new Mock<ICountriesRepositary>();
            _countriesRepository = _countriesRepositoryMock.Object;

            _countriesService = new CountriesService(_countriesRepository);
            _fixture = new Fixture();
        }

        #region AddCountry
        //when CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task AddCountry_NullCountry_ToBeArgumentNullException()
        {
            //Arrange
            CountryAddRequest? request = null;
            Country country = _fixture.Build<Country>()
            .With(temp => temp.Persons, null as List<Person>).Create();

            _countriesRepositoryMock.Setup(temp => temp.AddCountry(It.IsAny<Country>())).ReturnsAsync(country);
            //Assert
            var action = (async () =>
            {
                //Act
                await _countriesService.AddCountry(request);
            });
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //when the CountryName is Null it should throw ArgumentException 
        [Fact]
        public async Task AddCountry_CountryNameIsNull_ToBeArgumentException()
        {
            //Arrange
            CountryAddRequest? request = _fixture.Build<CountryAddRequest>().With(temp => temp.CountryName, null as string).Create();
            Country country = _fixture.Build<Country>()
            .With(temp => temp.Persons, null as List<Person>).Create();

            _countriesRepositoryMock.Setup(temp => temp.AddCountry(It.IsAny<Country>())).ReturnsAsync(country);

            //Assert
            var action = (async () =>
            {
                //Act
                await _countriesService.AddCountry(request);
            });
            await action.Should().ThrowAsync<ArgumentException>();
        }
        //When the CountryName is duplicate, it should throw ArgumentException

        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest first_country_request = _fixture.Build<CountryAddRequest>()
              .With(temp => temp.CountryName, "Test name").Create();
            CountryAddRequest second_country_request = _fixture.Build<CountryAddRequest>()
              .With(temp => temp.CountryName, "Test name").Create();

            Country first_country = first_country_request.ToCountry();
            Country second_country = second_country_request.ToCountry();

            _countriesRepositoryMock
             .Setup(temp => temp.AddCountry(It.IsAny<Country>()))
             .ReturnsAsync(first_country);

            //Return null when GetCountryByCountryName is called
            _countriesRepositoryMock
             .Setup(temp => temp.GetCountryByCountryName(It.IsAny<string>()))
             .ReturnsAsync(null as Country);

            CountryResponse first_country_from_add_country = await _countriesService.AddCountry(first_country_request);

            //Act
            var action = async () =>
            {
                //Return first country when GetCountryByCountryName is called
                _countriesRepositoryMock.Setup(temp => temp.AddCountry(It.IsAny<Country>())).ReturnsAsync(first_country);

                _countriesRepositoryMock.Setup(temp => temp.GetCountryByCountryName(It.IsAny<string>())).ReturnsAsync(first_country);

                await _countriesService.AddCountry(second_country_request);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //when You supply proper country name, it should insert the same into countryname

        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest country_request = _fixture.Create<CountryAddRequest>();
            Country country = country_request.ToCountry();
            CountryResponse country_response = country.ToCountryResponse();

            _countriesRepositoryMock
             .Setup(temp => temp.AddCountry(It.IsAny<Country>()))
             .ReturnsAsync(country);

            _countriesRepositoryMock
             .Setup(temp => temp.GetCountryByCountryName(It.IsAny<string>()))
             .ReturnsAsync(null as Country);


            //Act
            CountryResponse country_from_add_country = await _countriesService.AddCountry(country_request);

            country.CountryID = country_from_add_country.CountryID;
            country_response.CountryID = country_from_add_country.CountryID;

            //Assert
            country_from_add_country.CountryID.Should().NotBe(Guid.Empty);
            country_from_add_country.Should().BeEquivalentTo(country_response);
        }

        #endregion

        #region GetAllCountries

        [Fact]
        //list of countries should be empty by default
        public async Task GetAllCountries_EmptyList()
        {
            List<Country> country = new List<Country>();
            _countriesRepositoryMock.Setup(temp=>temp.GetAllCountries()).ReturnsAsync(country);
            //Acts
            List<CountryResponse> actual_countries_response_List = await _countriesService.GetAllCountries();

            //Assert
            actual_countries_response_List.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllCountries_AddFewCountires()
        {
            //Arrange
            //Arrange
            List<Country> country_list = new List<Country>() {
        _fixture.Build<Country>()
        .With(temp => temp.Persons, null as List<Person>).Create(),
        _fixture.Build<Country>()
        .With(temp => temp.Persons, null as List<Person>).Create()
      };

            List<CountryResponse> country_response_list = country_list.Select(temp => temp.ToCountryResponse()).ToList();

            _countriesRepositoryMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(country_list);

            //Act
            List<CountryResponse> actualCountryResponseList = await _countriesService.GetAllCountries();

            //Assert
            actualCountryResponseList.Should().BeEquivalentTo(country_response_list);
        }

        #endregion

        #region GetCountryByCountryID

        [Fact]
        public async Task GetCountryByCountryID_NullCountryID()
        {
            Guid? countryID = null;

            _countriesRepositoryMock
             .Setup(temp => temp.GetCountryByCountryID(It.IsAny<Guid>()))
             .ReturnsAsync(null as Country);

            //Act
            CountryResponse? country_response_from_get_method = await _countriesService.GetCountryByCountryID(countryID);


            //Assert
            country_response_from_get_method.Should().BeNull();
        }

        [Fact]
        public async Task GetCountryByCountryID_ValidCountryID()
        {
            //Arrange
            Country country = _fixture.Build<Country>()
              .With(temp => temp.Persons, null as List<Person>)
              .Create();
            CountryResponse country_response = country.ToCountryResponse();

            _countriesRepositoryMock
             .Setup(temp => temp.GetCountryByCountryID(It.IsAny<Guid>()))
             .ReturnsAsync(country);

            //Act
            CountryResponse? country_response_from_get = await _countriesService.GetCountryByCountryID(country.CountryID);


            //Assert
            country_response_from_get.Should().Be(country_response);
        }

        #endregion
    }
}
