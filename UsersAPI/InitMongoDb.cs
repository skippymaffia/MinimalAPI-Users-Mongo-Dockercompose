using Domain.DataAccess;
using Domain.Model;
using System.Text.Json;

namespace UsersAPI;

public static class InitMongoDb
{
    public static async Task FillMongoDbAsync(this WebApplication app,
        string connectionString,
        string databaseName,
        string tableName)
    {
        var dbAccess = new MongoDataAccess(connectionString, databaseName, tableName);

        var jsonText = File.ReadAllText("users.json")
            ?? throw new Exception("json text is null");

        List<User> users = JsonSerializer.Deserialize<List<User>>(jsonText)
            ?? throw new Exception("users is null");

        await dbAccess.DeleteUsersAsync(users);
        await dbAccess.CreateUsersAsync(users);
    }
}
