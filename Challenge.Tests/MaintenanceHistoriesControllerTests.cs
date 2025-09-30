using Microsoft.AspNetCore.Mvc;
using challenge.Domain.Entity;
using challenge.Infrastructure.Context;
using challenge.Infrastructure.Services;
using challenge.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using Moq;
using challenge.Domain.DTOs;

public class MaintenanceHistoriesControllerTests
{
    private ChallengeContext GetDbContextWithData()
    {
        var options = new DbContextOptionsBuilder<ChallengeContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new ChallengeContext(options);
        context.MaintenanceHistories.Add(new MaintenanceHistory
        {
            MaintenanceHistoryID = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            // Adicione outros campos obrigatórios conforme necessário
        });
        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task GetMaintenanceHistory_ReturnsMaintenanceHistory_WhenExists()
    {
        var context = GetDbContextWithData();
        var hateoasMock = new Mock<IHateoasService>();
        var controller = new MaintenanceHistoriesController(context, hateoasMock.Object);

        var result = await controller.GetMaintenanceHistory(Guid.Parse("33333333-3333-3333-3333-333333333333"));

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var mh = Assert.IsType<MaintenanceHistory>(okResult.Value);
        Assert.Equal(Guid.Parse("33333333-3333-3333-3333-333333333333"), mh.MaintenanceHistoryID);
    }

    [Fact]
    public async Task GetMaintenanceHistory_ReturnsNotFound_WhenNotExists()
    {
        var context = GetDbContextWithData();
        var hateoasMock = new Mock<IHateoasService>();
        var controller = new MaintenanceHistoriesController(context, hateoasMock.Object);

        var result = await controller.GetMaintenanceHistory(Guid.NewGuid());

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetMaintenanceHistories_ReturnsPagedResult()
    {
        var context = GetDbContextWithData();
        var hateoasMock = new Mock<IHateoasService>();
        hateoasMock.Setup(h => h.GeneratePaginationLinks<MaintenanceHistory>(It.IsAny<PagedResult<MaintenanceHistory>>(), "MaintenanceHistory", It.IsAny<IUrlHelper>()))
            .Returns(new List<LinkDto> { new LinkDto { Rel = "self", Href = "self", Method = "GET" }, new LinkDto { Rel = "next", Href = "next", Method = "GET" } });

        var controller = new MaintenanceHistoriesController(context, hateoasMock.Object);

        var result = await controller.GetMaintenanceHistories(1, 10);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var paged = Assert.IsAssignableFrom<PagedResult<MaintenanceHistory>>(okResult.Value);
        Assert.Single(paged.Items);
        Assert.Equal(1, paged.CurrentPage);
        Assert.Equal(10, paged.PageSize);
        Assert.Equal(1, paged.TotalItems);
        Assert.NotNull(paged.Links);
    }

    [Fact]
    public async Task PostMaintenanceHistory_CreatesMaintenanceHistory()
    {
        var options = new DbContextOptionsBuilder<ChallengeContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new ChallengeContext(options);
        var hateoasMock = new Mock<IHateoasService>();
        var controller = new MaintenanceHistoriesController(context, hateoasMock.Object);

        var mh = new MaintenanceHistory
        {
            MaintenanceHistoryID = Guid.NewGuid(),
            // Adicione outros campos obrigatórios conforme necessário
        };

        var result = await controller.PostMaintenanceHistory(mh);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdMH = Assert.IsType<MaintenanceHistory>(createdResult.Value);
        Assert.Equal(mh.MaintenanceHistoryID, createdMH.MaintenanceHistoryID);
    }

    [Fact]
    public async Task PutMaintenanceHistory_UpdatesMaintenanceHistory()
    {
        var context = GetDbContextWithData();
        var hateoasMock = new Mock<IHateoasService>();
        var controller = new MaintenanceHistoriesController(context, hateoasMock.Object);

        var mh = context.MaintenanceHistories.Find(Guid.Parse("33333333-3333-3333-3333-333333333333"));
        // Adicione alteração de campo se necessário

        var result = await controller.PutMaintenanceHistory(mh.MaintenanceHistoryID, mh);

        Assert.IsType<NoContentResult>(result);
        Assert.NotNull(context.MaintenanceHistories.Find(mh.MaintenanceHistoryID));
    }

    [Fact]
    public async Task DeleteMaintenanceHistory_RemovesMaintenanceHistory()
    {
        var context = GetDbContextWithData();
        var hateoasMock = new Mock<IHateoasService>();
        var controller = new MaintenanceHistoriesController(context, hateoasMock.Object);

        var result = await controller.DeleteMaintenanceHistory(Guid.Parse("33333333-3333-3333-3333-333333333333"));

        Assert.IsType<NoContentResult>(result);
        Assert.Empty(context.MaintenanceHistories);
    }
}
