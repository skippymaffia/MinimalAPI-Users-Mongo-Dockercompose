using Domain.DataAccess.Mongo;
using Domain.Model;

namespace UsersAPI;

public static class ApiFunctions
{
    public static async Task<IResult> GetAllUsersAsync(IMongoDataAccess dbAccess)
    {
        var users = await dbAccess.GetAllUsersAsync();

        return Results.Ok(users);
    }

    public static async Task<IResult> GetUserByIdAsync(IMongoDataAccess dbAccess, int id)
    {
        var user = await dbAccess.GetUserAsync(id);

        if (user == null)
        {
            return Results.BadRequest(ApiConst.UserNotFoundError);
        }

        return Results.Ok(user);
    }

    public static async Task<IResult> CreateUserAsync(IMongoDataAccess dbAccess, User? u)
    {
        if (u is null)
        {
            return Results.BadRequest(ApiConst.NullUserError);
        }

        var result = await dbAccess.CreateUserAsync(u);

        return Results.Ok(result);
    }

    public static async Task<IResult> DeleteUserAsync(IMongoDataAccess dbAccess, int id)
    {
        await dbAccess.DeleteUserAsync(id);

        return Results.Accepted(ApiConst.ApiRoot + "/{id}", id);
    }

    public static async Task<IResult> UpdateUserAsync(IMongoDataAccess dbAccess, User? u)
    {
        if (u is null)
        {
            return Results.BadRequest(ApiConst.NullUserError);
        }

        await dbAccess.UpdateUserAsync(u);

        return Results.Accepted(ApiConst.ApiRoot + "/{id}", u.Id);
    }
}
