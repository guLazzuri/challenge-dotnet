using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using challenge.Infrastructure.Context;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;
using challenge.Controllers;

namespace Challenge.Tests
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove o contexto de banco de dados real
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ChallengeContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Adiciona contexto em memória para testes
                    services.AddDbContext<ChallengeContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDatabase");
                    });

                    // Build the service provider
                    var sp = services.BuildServiceProvider();

                    // Create a scope to obtain a reference to the database context
                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<ChallengeContext>();

                        // Ensure the database is created
                        db.Database.EnsureCreated();
                    }
                });
            });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task HealthCheck_ReturnsHealthy()
        {
            // Act
            var response = await _client.GetAsync("/health");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Healthy", content);
        }

        [Fact]
        public async Task HealthCheck_Ready_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/health/ready");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Auth_Login_WithValidCredentials_ReturnsToken()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Username = "admin",
                Password = "admin123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("token", content);
        }

        [Fact]
        public async Task Auth_Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Username = "invalid",
                Password = "invalid"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Vehicles_GetWithoutAuth_ReturnsUnauthorized()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/vehicles");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Vehicles_GetWithAuth_ReturnsOk()
        {
            // Arrange
            var token = await GetAuthToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/api/v1/vehicles");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Users_GetWithAuth_ReturnsOk()
        {
            // Arrange
            var token = await GetAuthToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/api/v1/user");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task MaintenanceHistories_GetWithAuth_ReturnsOk()
        {
            // Arrange
            var token = await GetAuthToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/api/v1/maintenancehistories");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Health_Controller_Get_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/health");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("status", content);
        }

        [Fact]
        public async Task Health_Controller_Ready_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/health/ready");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Health_Controller_Live_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/health/live");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Swagger_IsAccessible()
        {
            // Act
            var response = await _client.GetAsync("/swagger/index.html");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        private async Task<string> GetAuthToken()
        {
            var loginRequest = new LoginRequest
            {
                Username = "admin",
                Password = "admin123"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            return jsonDoc.RootElement.GetProperty("token").GetString() ?? string.Empty;
        }
    }
}
