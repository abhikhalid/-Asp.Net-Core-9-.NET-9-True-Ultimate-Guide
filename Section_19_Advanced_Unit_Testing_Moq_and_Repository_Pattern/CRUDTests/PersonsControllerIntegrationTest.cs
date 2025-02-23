using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;

namespace CRUDTests
{
    public class PersonsControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public PersonsControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
        }


        #region
        [Fact]
        public async void Index_ToReturnVie()
        {
            //Arrange
            //Act
            HttpResponseMessage response = await _httpClient.GetAsync("/Persons/Index");
            //Assert
            response.Should().BeSuccessful(); //200

            string responseBody = await response.Content.ReadAsStringAsync();

            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(responseBody);

            var document = html.DocumentNode;
            //we want the table tag with class 'persons'
            document.QuerySelectorAll("table.persons").Should().NotBeNull(); 
        }
        #endregion
    }
}
