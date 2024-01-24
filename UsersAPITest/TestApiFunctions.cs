using Domain.DataAccess;
using Domain.Model;
using FluentAssertions;
using UsersAPI;
using Xunit;

namespace ApiTest;

public class TestApiFunctions
{
    private readonly IMongoDataAccess dbAccess;
    private readonly User user;

    public TestApiFunctions()
    {
        dbAccess = new MongoDataAccessMock();
        user = new User() { Id = 1, Name = "test" };
    }

    [Fact]
    public async void CreateUserAsync_InvalidUser_Test()
    {
        var res = await ApiFunctions.CreateUserAsync(dbAccess, null);

        res.Should().NotBeNull();
        var typedRes = res.As<Microsoft.AspNetCore.Http.HttpResults.BadRequest<string>>();
        typedRes.StatusCode.Should().Be(400);
        typedRes.Value.Should().Be(ApiConst.NULL_USER_ERROR);
    }

    [Fact]
    public async void CreateUserAsync_ValidUser_Test()
    {
        var res = await ApiFunctions.CreateUserAsync(dbAccess, user);

        res.Should().NotBeNull();
        var typedRes = res.As<Microsoft.AspNetCore.Http.HttpResults.Ok<long>>();
        typedRes.StatusCode.Should().Be(200);
        typedRes.Value.Should().Be(1L);
    }

    [Fact]
    public async void GetUserAsync_InvalidUserId_Test()
    {
        var res = await ApiFunctions.GetUserByIdAsync(dbAccess, 0);

        var typedRes = res.As<Microsoft.AspNetCore.Http.HttpResults.BadRequest<string>>();
        typedRes.StatusCode.Should().Be(400);
        typedRes.Value.Should().Be(ApiConst.USER_NOT_FOUND_ERROR);
    }

    [Fact]
    public async void GetUserAsync_ValidUserId_Test()
    {
        _ = await ApiFunctions.CreateUserAsync(dbAccess, user);
        var res = await ApiFunctions.GetUserByIdAsync(dbAccess, 1);

        var typedRes = res.As<Microsoft.AspNetCore.Http.HttpResults.Ok<User>>();
        typedRes.StatusCode.Should().Be(200);
        typedRes.Value.Id.Should().Be(1);
    }

    [Fact]
    public async void GetUsersAsync_Ok_Test()
    {
        var res = await ApiFunctions.GetAllUsersAsync(dbAccess);

        res.Should().NotBeNull();
        var typedRes = res.As<Microsoft.AspNetCore.Http.HttpResults.Ok<List<User>>>();
        typedRes.StatusCode.Should().Be(200);
        typedRes.Value.Count.Should().Be(0);
    }

    [Fact]
    public async void UpdateUserAsync_InvalidUser_Test()
    {
        var res = await ApiFunctions.UpdateUserAsync(dbAccess, null);

        res.Should().NotBeNull();
        var typedRes = res.As<Microsoft.AspNetCore.Http.HttpResults.BadRequest<string>>();
        typedRes.StatusCode.Should().Be(400);
        typedRes.Value.Should().Be(ApiConst.NULL_USER_ERROR);
    }

    [Fact]
    public async void UpdateUserAsync_Not_Null_Accepted_Test()
    {
        _ = await ApiFunctions.CreateUserAsync(dbAccess, user);

        user.Name = "updated";
        var res = await ApiFunctions.UpdateUserAsync(dbAccess, user);

        res.Should().NotBeNull();
        var typedRes = res.As<Microsoft.AspNetCore.Http.HttpResults.Accepted<int>>();
        typedRes.StatusCode.Should().Be(202);
        typedRes.Value.Should().Be(user.Id);
    }

    [Fact]
    public async void DeleteUserAsync_Not_Null_Accepted_Test()
    {
        _ = await ApiFunctions.CreateUserAsync(dbAccess, user);

        var res = await ApiFunctions.DeleteUserAsync(dbAccess, user.Id);

        res.Should().NotBeNull();
        var typedRes = res.As<Microsoft.AspNetCore.Http.HttpResults.Accepted<int>>();
        typedRes.StatusCode.Should().Be(202);
        typedRes.Value.Should().Be(user.Id);
    }
}