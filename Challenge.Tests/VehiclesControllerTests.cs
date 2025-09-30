using Microsoft.AspNetCore.Mvc;
using challenge.Domain.Entity;
using challenge.Infrastructure.Context;
using challenge.Infrastructure.Services;
using cp_02.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using Moq;
using challenge.Domain.DTOs;

public class VehiclesControllerTests
{
    private ChallengeContext GetDbContextWithData()
    {
        var options = new DbContextOptionsBuilder<ChallengeContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new ChallengeContext(options);
        context.Vehicles.Add(new Vehicle
        {
            VehicleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            LicensePlate = "ABC1234",
            VehicleModel = VehicleModel.SPORT,
            IsCancel = false,
            UserCancelID = Guid.NewGuid()
        });
        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task GetVehicle_ReturnsVehicle_WhenExists()
    {
        var context = GetDbContextWithData();
        var hateoasMock = new Mock<IHateoasService>();
        var controller = new VehiclesController(context, hateoasMock.Object);

        var result = await controller.GetVehicle(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var vehicle = Assert.IsType<Vehicle>(okResult.Value);
        Assert.Equal("ABC1234", vehicle.LicensePlate);
    }

    [Fact]
    public async Task GetVehicle_ReturnsNotFound_WhenNotExists()
    {
        var context = GetDbContextWithData();
        var hateoasMock = new Mock<IHateoasService>();
        var controller = new VehiclesController(context, hateoasMock.Object);

        var result = await controller.GetVehicle(Guid.NewGuid());

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetVehicles_ReturnsPagedResult()
    {
        var context = GetDbContextWithData();
        var hateoasMock = new Mock<IHateoasService>();
        hateoasMock.Setup(h => h.GeneratePaginationLinks<Vehicle>(It.IsAny<PagedResult<Vehicle>>(), "Vehicle", It.IsAny<IUrlHelper>()))
            .Returns(new List<LinkDto> { new LinkDto { Rel = "self", Href = "self", Method = "GET" }, new LinkDto { Rel = "next", Href = "next", Method = "GET" } });

        var controller = new VehiclesController(context, hateoasMock.Object);

        var result = await controller.GetVehicles(1, 10);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var paged = Assert.IsAssignableFrom<PagedResult<Vehicle>>(okResult.Value);
        Assert.Single(paged.Items);
        Assert.Equal(1, paged.CurrentPage);
        Assert.Equal(10, paged.PageSize);
        Assert.Equal(1, paged.TotalItems);
        Assert.NotNull(paged.Links);
    }

    [Fact]
    public async Task PostVehicle_CreatesVehicle()
    {
        var options = new DbContextOptionsBuilder<ChallengeContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new ChallengeContext(options);
        var hateoasMock = new Mock<IHateoasService>();
        var controller = new VehiclesController(context, hateoasMock.Object);

        var vehicle = new Vehicle
        {
            VehicleId = Guid.NewGuid(),
            LicensePlate = "XYZ9876",
            VehicleModel = VehicleModel.POP,
            IsCancel = false,
            UserCancelID = Guid.NewGuid()
        };

        var result = await controller.PostVehicle(vehicle);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdVehicle = Assert.IsType<Vehicle>(createdResult.Value);
        Assert.Equal("XYZ9876", createdVehicle.LicensePlate);
    }

    [Fact]
    public async Task DeleteVehicle_RemovesVehicle()
    {
        var context = GetDbContextWithData();
        var hateoasMock = new Mock<IHateoasService>();
        var controller = new VehiclesController(context, hateoasMock.Object);

        var result = await controller.DeleteVehicle(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        Assert.IsType<NoContentResult>(result);
        Assert.Empty(context.Vehicles);
    }
}