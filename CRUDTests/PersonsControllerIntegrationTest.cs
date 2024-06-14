using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Fizzler;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace CRUDTests
{
    public class PersonsControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public PersonsControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        #region Index

        [Fact]
        public async void Index_ToReturnView()
        {
            //Arrange
            //for creating httpclient for our application we need to create a custome web application factory 

            //Act
            HttpResponseMessage response = await _client.GetAsync("/Persons/Index");

            string responseBody = await response.Content.ReadAsStringAsync();

            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(responseBody);
            var document = html.DocumentNode;
            document.QuerySelectorAll("table");

            //Assert
            response.Should().BeSuccessful();
        }

        #endregion
    }
}
