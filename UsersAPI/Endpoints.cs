using Domain.DataAccess;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;
using UsersAPI;

namespace Api;

public static class Endpoints
{
    public static void AddEndpoints(
        this WebApplication app,
        string connectionString,
        string databaseName,
        string tableName)
    {
        IMongoDataAccess dbAccess =
            new MongoDataAccess(connectionString, databaseName, tableName);

        app.MapGet(
            ApiConst.API_ROOT,
            async () => await ApiFunctions.GetAllUsersAsync(dbAccess))
            .WithName("GetAllUsers");

        app.MapGet(
            ApiConst.API_ROOT + "/{id}",
            async (int id) => await ApiFunctions.GetUserByIdAsync(dbAccess, id))
            .WithName("GetUserById");

        app.MapPost(
            ApiConst.API_ROOT,
            async ([FromBody] User? u) => await ApiFunctions.CreateUserAsync(dbAccess, u))
            .WithName("CreateUser");

        app.MapPut(
            ApiConst.API_ROOT,
            async ([FromBody] User u) => await ApiFunctions.UpdateUserAsync(dbAccess, u))
            .WithName("UpdateUser");

        app.MapDelete(
            ApiConst.API_ROOT + "/{id}",
            async (int id) => await ApiFunctions.DeleteUserAsync(dbAccess, id))
            .WithName("DeleteUser");
    }

}
