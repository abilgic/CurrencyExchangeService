using Data.Repository;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System.Net;

namespace UnitTest
{
    public class UnitTest
    {
        private readonly Mock<ICurrencyService> _currencyService;
        public UnitTest()
        {
            _currencyService = new Mock<ICurrencyService>();
        }

        [TestMethod]
        public void GetReturnsProduct()
        {
            // Arrange
            var controller = new ProductsController(_currencyService);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Get(10);

            // Assert
            Product product;
            Assert.IsTrue(response.TryGetContentValue<Product>(out product));
            Assert.AreEqual(10, product.Id);
        }
    }
}