using Api;
using Domain.Model;
using FluentAssertions;
using Xunit;

namespace UsersAPITest;

public class TestEndpoints
{
    private readonly MongoDataAccessMock _dbAccess = new();
    private readonly User _user = new() { Id = 1, Name = "test" };

    [Fact]
    public async void CreateUserAsyncNullBadRequestTest()
    {
        var res = await Endpoints.CreateUserAsync(_dbAccess, null);

        res.Should().NotBeNull();
        res.As<Microsoft.AspNetCore.Http.HttpResults.BadRequest>().StatusCode.Should().Be(400);
    }

    [Fact]
    public async void CreateUserAsyncNotNullCreatedTest()
    {
        var res = await Endpoints.CreateUserAsync(_dbAccess, _user);

        res.Should().NotBeNull();
        res.Should().Be(201);
    }

    [Fact]
    public async void GetUserAsync_Null_BadRequest_Test()
    {
        var res = await Endpoints.GetUserByIdAsync(_dbAccess, 0);

        res.Should().NotBeNull();
        res.Should().Be(400);
    }

    [Fact]
    public async void GetUserAsync_Not_Null_Ok_Test()
    {
        var res = await Endpoints.GetUserByIdAsync(_dbAccess, 1);

        res.Should().NotBeNull();
        res.Should().Be(200);
    }


    [Fact]
    public async void GetUsersAsync_Ok_Test()
    {
        var res = await Endpoints.GetUsersAsync(_dbAccess);

        res.Should().NotBeNull();
        res.Should().Be(200);
    }

    [Fact]
    public async void UpdateUserAsync_Null_BadRequest_Test()
    {
        var res = await Endpoints.UpdateUserAsync(_dbAccess, null);

        res.Should().NotBeNull();
        res.Should().Be(400);
    }

    [Fact]
    public async void UpdateUserAsync_Not_Null_Accepted_Test()
    {
        _user.Name = "updated";
        var res = await Endpoints.UpdateUserAsync(_dbAccess, _user);

        res.Should().NotBeNull();
        res.Should().Be(202);
    }

    [Fact]
    public async void DeleteUserAsync_Not_Null_Accepted_Test()
    {
        var res = await Endpoints.DeleteUserAsync(_dbAccess, 2);

        res.Should().NotBeNull();
        res.Should().Be(202);
    }
}