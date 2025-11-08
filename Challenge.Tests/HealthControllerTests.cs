using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using challenge.Controllers;
using Xunit;

namespace Challenge.Tests
{
    public class HealthControllerTests
    {
        [Fact]
        public async Task Get_WhenHealthy_ReturnsOk()
        {
            // Arrange
            var mockHealthCheckService = new Mock<HealthCheckService>();
            var healthReport = new HealthReport(
                new Dictionary<string, HealthReportEntry>(),
                HealthStatus.Healthy,
                TimeSpan.FromMilliseconds(100)
            );

            mockHealthCheckService
                .Setup(s => s.CheckHealthAsync(It.IsAny<Func<HealthCheckRegistration, bool>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthReport);

            var controller = new HealthController(mockHealthCheckService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Get_WhenUnhealthy_ReturnsServiceUnavailable()
        {
            // Arrange
            var mockHealthCheckService = new Mock<HealthCheckService>();
            var healthReport = new HealthReport(
                new Dictionary<string, HealthReportEntry>
                {
                    { "database", new HealthReportEntry(HealthStatus.Unhealthy, "Database connection failed", TimeSpan.FromMilliseconds(100), null, null) }
                },
                HealthStatus.Unhealthy,
                TimeSpan.FromMilliseconds(100)
            );

            mockHealthCheckService
                .Setup(s => s.CheckHealthAsync(It.IsAny<Func<HealthCheckRegistration, bool>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthReport);

            var controller = new HealthController(mockHealthCheckService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(503, statusCodeResult.StatusCode);
        }

        [Fact]
        public void Ready_ReturnsOk()
        {
            // Arrange
            var mockHealthCheckService = new Mock<HealthCheckService>();
            var controller = new HealthController(mockHealthCheckService.Object);

            // Act
            var result = controller.Ready();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public void Live_ReturnsOk()
        {
            // Arrange
            var mockHealthCheckService = new Mock<HealthCheckService>();
            var controller = new HealthController(mockHealthCheckService.Object);

            // Act
            var result = controller.Live();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }
    }
}
