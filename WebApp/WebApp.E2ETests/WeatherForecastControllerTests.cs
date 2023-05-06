using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace WebApp.E2ETests
{
    [TestClass]
    public class WeatherForecastControllerTests
    {
        [TestMethod]
        public void GetShouldRetrieveForecast()
        {
            //Arrange
            var client = new HttpClient();

            //Act
            var response = client.GetAsync("http://sut:5000/weatherforecast");

            response.Wait(10000);
            
            //Assert
            Assert.IsTrue(response.Result.IsSuccessStatusCode);
        }
    }
}