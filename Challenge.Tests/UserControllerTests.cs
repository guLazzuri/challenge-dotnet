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

public class UserControllerTests
{
    private ChallengeContext GetDbContextWithData()
    {
        var options = new DbContextOptionsBuilder<ChallengeContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new ChallengeContext(options);
        context.Users.Add(new User
        {
            UserID = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Email = "user@email.com",
            Password = "11TesteTeste@",
            Type = UserType.CLIENT,
            IsCancel = false,
            UserCancelID = Guid.NewGuid()
        });
        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task GetUser_ReturnsUser_WhenExists()
    {
        var context = GetDbContextWithData();
        var hateoasMock = new Mock<IHateoasService>();
        var controller = new UserController(context, hateoasMock.Object);

        var result = await controller.GetUser(Guid.Parse("22222222-2222-2222-2222-222222222222"));

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var user = Assert.IsType<User>(okResult.Value);
        Assert.Equal("user@email.com", user.Email);
    }

    [Fact]
    public async Task GetUser_ReturnsNotFound_WhenNotExists()
    {
        var context = GetDbContextWithData();
        var hateoasMock = new Mock<IHateoasService>();
        var controller = new UserController(context, hateoasMock.Object);

        var result = await controller.GetUser(Guid.NewGuid());

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetUsers_ReturnsPagedResult()
    {
        var context = GetDbContextWithData();
        var hateoasMock = new Mock<IHateoasService>();
        hateoasMock.Setup(h => h.GeneratePaginationLinks<User>(It.IsAny<PagedResult<User>>(), "User", It.IsAny<IUrlHelper>()))
            .Returns(new List<LinkDto> { new LinkDto { Rel = "self", Href = "self", Method = "GET" }, new LinkDto { Rel = "next", Href = "next", Method = "GET" } });

        var controller = new UserController(context, hateoasMock.Object);

        var result = await controller.GetUsers(1, 10);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var paged = Assert.IsAssignableFrom<PagedResult<User>>(okResult.Value);
        Assert.Single(paged.Items);
        Assert.Equal(1, paged.CurrentPage);
        Assert.Equal(10, paged.PageSize);
        Assert.Equal(1, paged.TotalItems);
        Assert.NotNull(paged.Links);
    }

    [Fact]
    public async Task PostUser_CreatesUser()
    {
        var options = new DbContextOptionsBuilder<ChallengeContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new ChallengeContext(options);
        var hateoasMock = new Mock<IHateoasService>();
        var controller = new UserController(context, hateoasMock.Object);

        var user = new User
        {
            UserID = Guid.NewGuid(),
            Email = "new@email.com",
            Password = "10TesteTeste@!",
            Type = UserType.ADMIN,
            IsCancel = false,
            UserCancelID = Guid.NewGuid()
        };

        var result = await controller.PostUser(user);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdUser = Assert.IsType<User>(createdResult.Value);
        Assert.Equal("new@email.com", createdUser.Email);
    }

    [Fact]
    public async Task PutUser_UpdatesUser()
    {
        var context = GetDbContextWithData();
        var hateoasMock = new Mock<IHateoasService>();
        var controller = new UserController(context, hateoasMock.Object);

        var user = context.Users.Find(Guid.Parse("22222222-2222-2222-2222-222222222222"));
        user.Email = "updated@email.com";

        var result = await controller.PutUser(user.UserID, user);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal("updated@email.com", context.Users.Find(user.UserID).Email);
    }

    [Fact]
    public async Task DeleteUser_RemovesUser()
    {
        var context = GetDbContextWithData();
        var hateoasMock = new Mock<IHateoasService>();
        var controller = new UserController(context, hateoasMock.Object);

        var result = await controller.DeleteUser(Guid.Parse("22222222-2222-2222-2222-222222222222"));

        Assert.IsType<NoContentResult>(result);
        Assert.Empty(context.Users);
    }
}
