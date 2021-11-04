using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApp.Controllers;

namespace WebApp.UnitTests
{
    [TestClass]
    public class WeatherForecastControllerUnitTests
    {
        [TestMethod]
        public void WhenGetMethodIsCalledThenWeatherResultsAreSent()
        {
           // Arrange
           var loggerMock = new Mock<ILogger<WeatherForecastController>>();
           var weatherForecastController = new WeatherForecastController(loggerMock.Object);


            // Act
            var result = weatherForecastController.Get();

            // Assert
            Assert.IsTrue(result.Count() == 5);
        }
    }
}
