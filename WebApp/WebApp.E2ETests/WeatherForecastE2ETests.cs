using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebApp.E2ETests
{
    [TestClass]
    public class WeatherForecastE2ETests
    {

        [TestMethod]
        public void GetShouldRetrieveForecast()
        {
            //Arrange
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(2);

            //Act
            var task = httpClient.GetAsync($"http://sut:8080/WeatherForecast");

            Assert.IsTrue(task.Wait(TimeSpan.FromSeconds(3)));

            //Assert
            Assert.IsTrue(task.Result.IsSuccessStatusCode);
        }
    }
}