using Domain.DataAccess;
using Domain.DataAccess.Mongo;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace UsersAPI;

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
            ApiConst.ApiRoot,
            async () => await ApiFunctions.GetAllUsersAsync(dbAccess))
            .WithName("GetAllUsers");

        app.MapGet(
            ApiConst.ApiRoot + "/{id}",
            async (int id) => await ApiFunctions.GetUserByIdAsync(dbAccess, id))
            .WithName("GetUserById");

        app.MapPost(
            ApiConst.ApiRoot,
            async ([FromBody] User? u) => await ApiFunctions.CreateUserAsync(dbAccess, u))
            .WithName("CreateUser");

        app.MapPut(
            ApiConst.ApiRoot,
            async ([FromBody] User u) => await ApiFunctions.UpdateUserAsync(dbAccess, u))
            .WithName("UpdateUser");

        app.MapDelete(
            ApiConst.ApiRoot + "/{id}",
            async (int id) => await ApiFunctions.DeleteUserAsync(dbAccess, id))
            .WithName("DeleteUser");
    }

}
