namespace TestProject
{
    [TestClass]
    public class UnitTest
    {
        await using var application = new WebApplicationFactory<Program>();
   using var client = application.CreateClient();
   var response = await client.GetAsync("/customers");
    var data = await response.Content.ReadAsStringAsync();
    Assert.Equal(HttpStatusCode.OK, response.StatusCode
    }
}