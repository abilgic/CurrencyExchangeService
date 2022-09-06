using CurrencyExchangeService.Controllers;
using CurrencyExchangeService.Models;
using Data.Models;
using Data.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net.Http.Json;

namespace ExchangeCurrencyTest
{
    [TestClass]
    public class UnitTest
    {
        CurrencyController _controller;
        public UnitTest()
        {
            using var application = new WebApplicationFactory<Program>();

            var client = application.CreateClient();
            using var scope = application.Services.CreateScope();
            var currency = scope.ServiceProvider.GetRequiredService<ICurrencyService>();
            var iconfig = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            
            var cache = new MemoryCache(new MemoryCacheOptions());
            var mock = new Mock<ILogger<CurrencyController>>();
            ILogger<CurrencyController> logger = mock.Object;
            
            logger = Mock.Of<ILogger<CurrencyController>>();
             _controller = new CurrencyController(currency,iconfig, logger, cache);
        }
        [TestMethod]
        public void TestConversionLog()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "Test_AddConversionLog").Options;
            using (var context = new ApplicationDbContext(options))
            {
                var service = new CurrencyService(context);
                var testRecord = new ConversionLog()
                {
                    Currency1 = "USD",
                    Currency2 = "EUR",
                    Amount = 100,
                    Result = 100.9165,
                    Createdate = DateTime.Now
                };
                int returnValue = service.Add(testRecord).Result;
                Assert.AreEqual(true, returnValue > 0);
                var testRecords = service.GetAll().Result;
                Assert.IsNotNull(testRecords);
            }
        }


        [TestMethod]
        public void GetSymbolsTest()
    {
            var todos = _controller.GetSymbolList();

            Assert.IsNotNull(todos);
    }
    
    }
}

