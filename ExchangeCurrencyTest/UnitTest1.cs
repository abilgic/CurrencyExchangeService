using CurrencyExchangeService.Controllers;
using Data.Models;
using Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeCurrencyTest
{
    [TestClass]
    public class UnitTest
    {        
       
        [TestMethod]       
        public void TestConversionLog()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "Test_AddConversionLog").Options;
            using (var context = new ApplicationDbContext(options))
            {
                var service = new CurrencyService(context);
                var testRecord = new ConversionLog()
                {
                    Currency1= "USD",
                    Currency2 = "EUR",
                    Amount=100,
                    Result= 100.9165,
                    Createdate=DateTime.Now
                };
                int returnValue = service.Add(testRecord).Result;                 
                Assert.AreEqual(true, returnValue > 0);
                var testRecords = service.GetAll().Result;
                Assert.IsNull(testRecords);
            }
        }       

    }
}
