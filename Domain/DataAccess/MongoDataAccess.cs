using Domain.Model;
using MongoDB.Driver;

namespace Domain.DataAccess;

public class MongoDataAccess : IMongoDataAccess
{
    private readonly string _connectionString;
    private readonly string _databaseName;
    private readonly string _tableName;

    public MongoDataAccess(string connectionString, string databaseName, string tableName)
    {
        _connectionString = connectionString;
        _databaseName = databaseName;
        _tableName = tableName;
    }

    public IMongoCollection<User> GetMongoTable()
    {
        try
        {
            var client = new MongoClient(_connectionString);
            var db = client.GetDatabase(_databaseName);
            var table = db.GetCollection<User>(_tableName);

            return table;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error while initiating mongodb!");
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<List<User>> GetUsersAsync()
    {
        var table = GetMongoTable();
        var users = await table.FindAsync(_ => true);

        return users is not null ? users.ToList() : new List<User>();
    }

    public async Task<User?> GetUserAsync(int id)
    {
        var table = GetMongoTable();
        var users = await table.FindAsync(x => x.Id == id);
        var results = users is not null ? users.ToList() : new List<User>();
        if (results.Count > 0)
        {
            return results.First();
        }

        return null;
    }

    public Task<long> CreateUsersAsync(List<User> users)
    {
        var table = GetMongoTable();

        table.InsertManyAsync(users);

        return Task.FromResult((long)users.Count);
    }

    public Task<long> CreateUserAsync(User? user)
    {
        if (user is null)
        {
            return Task.FromResult(0L);
        }

        var table = GetMongoTable();

        table.InsertOneAsync(user);

        return Task.FromResult(1L);
    }

    public async Task<long> UpdateUserAsync(User? user)
    {
        if (user is null)
        {
            return 0L;
        }

        var table = GetMongoTable();
        var filter = Builders<User>.Filter.Eq("Id", user.Id);
        var update =
            Builders<User>.Update
                .Set(x => x.Name, user.Name)
                .Set(y => y.UserName, user.UserName);
        var result = await table.UpdateOneAsync(filter, update);

        return result is not null ? result.ModifiedCount : 0L;
    }

    public async Task<long> DeleteUserAsync(int id)
    {
        var table = GetMongoTable();
        var filter = Builders<User>.Filter.Eq("Id", id);
        var result = await table.DeleteOneAsync(filter);

        return result is not null ? result.DeletedCount : 0L;
    }

    public async Task<long> DeleteUsersAsync(List<User> users)
    {
        long result = 0;
        for (int i = 0; i < users.Count; i++)
        {
            result += await DeleteUserAsync(users[i].Id);
        }

        return result;
    }
}
