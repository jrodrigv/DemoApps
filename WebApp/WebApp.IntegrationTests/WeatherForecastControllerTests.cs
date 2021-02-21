using System.Net.Http;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace WebApp.IntegrationTests
{
    [TestClass]
    public class WeatherForecastControllerTests
    {
        [TestMethod]
        public void GetShouldRetrieveForecast()
        {
            //Arrange
            
            var fixture = new WebApplicationFactory<WebApp.Startup>();

            var client = fixture.CreateClient();


            //Act

            var response =  client.GetAsync("/weatherforecast");

            response.Wait(10000);
            var content = response.Result.Content.ReadAsStringAsync();

            content.Wait(10000);

            var forecast = JsonConvert.DeserializeObject<WeatherForecast[]>(content.Result);


            //Assert
            Assert.IsTrue(forecast.Length == 5);
        }
    }
}
