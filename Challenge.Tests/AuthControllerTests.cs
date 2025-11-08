using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using challenge.Controllers;
using Xunit;

namespace Challenge.Tests
{
    public class AuthControllerTests
    {
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Jwt:Key", "ChaveSecretaSuperSeguraParaDesenvolvimento123456"}
                }!)
                .Build();

            _controller = new AuthController(configuration);
        }

        [Fact]
        public void Login_WithValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "admin",
                Password = "admin123"
            };

            // Act
            var result = _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            
            var response = okResult.Value;
            var tokenProperty = response?.GetType().GetProperty("token");
            Assert.NotNull(tokenProperty);
            
            var token = tokenProperty?.GetValue(response)?.ToString();
            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Fact]
        public void Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "invalid",
                Password = "invalid"
            };

            // Act
            var result = _controller.Login(request);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public void Login_WithEmptyUsername_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "",
                Password = "password"
            };

            // Act
            var result = _controller.Login(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Login_WithEmptyPassword_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "user",
                Password = ""
            };

            // Act
            var result = _controller.Login(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData("admin", "admin123")]
        [InlineData("user", "user123")]
        public void Login_WithDifferentValidUsers_ReturnsToken(string username, string password)
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = username,
                Password = password
            };

            // Act
            var result = _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }
    }
}
