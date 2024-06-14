using System;
using System.Collections.Generic;
using AutoFixture;
using Moq;
using ServiceContracts;
using Xunit;
using FluentAssertions;
using CRUDExample.Controllers;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Microsoft.AspNetCore.Mvc;
using Entities;

namespace CRUDTests
{
    public class PersonsControllerTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;

        private readonly Mock<IPersonsService> _personsServiceMock;
        private readonly Mock<ICountriesService> _countriesServiceMock;

        private readonly Fixture _fixture;

        //constructor
        public PersonsControllerTest()
        {
            _fixture = new Fixture();
            _countriesServiceMock = new Mock<ICountriesService>();
            _personsServiceMock = new Mock<IPersonsService>();

            _personsService = _personsServiceMock.Object;
            _countriesService = _countriesServiceMock.Object; 
        }

        #region Index

        [Fact]
        public async Task Index_ShouldReturnIndexViewWithPersonsList()
        {
            //Arrange
            List<PersonResponse> personResponsesList = _fixture.Create<List<PersonResponse>>();

            //creating an object for the controller
            PersonsController personsController = new PersonsController(_personsService, _countriesService);

            //Mocking
            _personsServiceMock.Setup(temp=>temp.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(personResponsesList);
            _personsServiceMock.Setup(temp => temp.GetSortedPerson(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>())).ReturnsAsync(personResponsesList);

            //Act
            IActionResult result = await personsController.Index(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<SortOrderOptions>());

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
            viewResult.ViewData.Model.Should().Be(personResponsesList);
        }

        #endregion

        #region Create

        [Fact]
        public async Task Create_IfModelErrors_ToReturnCreateView()
        {
            //Arrange
            PersonAddRequest person_Add_Request = _fixture.Create<PersonAddRequest>();

            PersonResponse personResponse = _fixture.Create<PersonResponse>();

            List<CountryResponse> countryResponsesList = _fixture.Create<List<CountryResponse>>();

            //creating an object for the controller

            //Mocking
            _countriesServiceMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countryResponsesList);
            _personsServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(personResponse);

            PersonsController personsController = new PersonsController(_personsService, _countriesService);
            //Act
            personsController.ModelState.AddModelError("PersonName", "Person Name cant be blank");
            
            IActionResult result = await personsController.Create(person_Add_Request);

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();
            viewResult.ViewData.Model.Should().Be(person_Add_Request);
        }

        [Fact]
        public async Task Create_IfNoModelErrors_ToRedirectToIndexView()
        {
            PersonAddRequest personResponsesList = _fixture.Create<PersonAddRequest>();

            PersonResponse personResponse = _fixture.Create<PersonResponse>();

            List<CountryResponse> countryResponsesList = _fixture.Create<List<CountryResponse>>();


            //Mocking
            _countriesServiceMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countryResponsesList);
            _personsServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(personResponse);

            //creating an object for the controller
            PersonsController personsController = new PersonsController(_personsService, _countriesService);
            
            //Act
            IActionResult result = await personsController.Create(personResponsesList);

            //Assert
            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);
            redirectResult.ActionName.Should().Be("Index");
        }

        #endregion

        #region Edit

        [Fact]
        public async Task Edit_IfPersonIDIsNull()
        {
            //Arrange
            Guid? personId = null;

            //Act
            _personsServiceMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid?>()));

            PersonsController personsController = new PersonsController(_personsService, _countriesService);
            IActionResult result = await personsController.Edit(personId);

            //Assert
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task Edit_WithProperPersonID()
        {
            //Arrange
            Guid? personId = _fixture.Create<Guid?>();
            PersonResponse personResponse = _fixture.Build<PersonResponse>().With(temp => temp.PersonID, personId).With(temp=>temp.Gender, Convert.ToString(GenderOptions.Male)).Create();
            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

            //Act
            _personsServiceMock.Setup(temp=>temp.GetPersonByPersonID(It.IsAny<Guid?>())).ReturnsAsync(personResponse);
            _countriesServiceMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countries);
            PersonsController personsController = new PersonsController(_personsService, _countriesService);

            IActionResult result = await personsController.Edit(personId);
            
            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonUpdateRequest>();
            viewResult.ViewData.Model.Should().Be(personUpdateRequest);
        }

        #endregion
    }
}
