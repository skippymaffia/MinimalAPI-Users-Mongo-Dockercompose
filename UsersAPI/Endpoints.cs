using Domain.DataAccess;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace Api;

public static class Endpoints
{
    private const string API_ROOT = "/api/users";
    public static void AddEndpoints(
        this WebApplication app,
        string connectionString,
        string databaseName,
        string tableName)
    {
        var dbAccess = new MongoDataAccess(connectionString, databaseName, tableName);

        app.MapGet(
            API_ROOT,
            async () => await GetUsersAsync(dbAccess))
            .WithName("GetUsers");

        app.MapGet(
            API_ROOT + "/{id}",
            async (int id) => await GetUserByIdAsync(dbAccess, id))
            .WithName("GetUser");

        app.MapPost(
            API_ROOT,
            async ([FromBody] User u) => await CreateUserAsync(dbAccess, u))
            .WithName("CreateUser");

        app.MapPut(
            API_ROOT,
            async ([FromBody] User u) => await UpdateUserAsync(dbAccess, u))
            .WithName("UpdateUser");

        app.MapDelete(
            API_ROOT + "/{id}",
            async (int id) => await DeleteUserAsync(dbAccess, id))
            .WithName("DeleteUser");
    }

    public static async Task<IResult> DeleteUserAsync(IMongoDataAccess dbAccess, int id)
    {
        await dbAccess.DeleteUserAsync(id);

        return Results.Accepted(API_ROOT + "/{id}", id);
    }

    public static async Task<IResult> UpdateUserAsync(IMongoDataAccess dbAccess, User? u)
    {
        if (u is null)
        {
            return Results.BadRequest("User can not be null!");
        }

        await dbAccess.UpdateUserAsync(u);

        return Results.Accepted(API_ROOT + "/{id}", u.Id);
    }

    public static async Task<IResult> CreateUserAsync(IMongoDataAccess dbAccess, User? u)
    {
        if (u is null)
        {
            return Results.BadRequest("User can not be null!");
        }

        await dbAccess.CreateUser(u);

        return Results.Created(API_ROOT + "/{id}", u.Id);
    }

    public static async Task<IResult> GetUserByIdAsync(IMongoDataAccess dbAccess, int id)
    {
        var user = await dbAccess.GetUserAsync(id);

        if (user == null)
        {
            return Results.BadRequest("User not found!");
        }

        return Results.Ok(user);
    }

    public static async Task<IResult> GetUsersAsync(IMongoDataAccess dbAccess)
    {
        var users = await dbAccess.GetUsersAsync();

        return Results.Ok(users);
    }
}
