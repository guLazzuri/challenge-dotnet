using Xunit;

public class IntegrationTest : IClassFixture<WebApplicationFactory<Challenge.Program>>
{
    private readonly WebApplicationFactory<Challenge.Program> _factory;

    public IntegrationTest(WebApplicationFactory<Challenge.Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Swagger_Endpoint_Returns_OK()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/swagger/index.html");
        response.EnsureSuccessStatusCode();
    }
}
