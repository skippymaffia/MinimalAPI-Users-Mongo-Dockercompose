using Domain.DataAccess;
using Domain.Model;
using MongoDB.Driver;

namespace ApiTest;

public class MongoDataAccessMock : IMongoDataAccess
{
    private readonly List<User> _users = new();

    public Task CreateUser(User user)
    {
        _users.Add(user);

        return Task.CompletedTask;
    }

    public Task CreateUsers(List<User> users)
    {
        _users.AddRange(users);

        return Task.CompletedTask;
    }

    public Task<long> DeleteUserAsync(int id)
    {
        var user = _users.Find(x => x.Id == id);
        if (user is null)
        {
            return Task.FromResult(0L);
        }

        _users.Remove(user);
        return Task.FromResult(1L);
    }

    public async Task<long> DeleteUsersAsync(List<User> users)
    {
        long result = 0L;
        for (int i = 0; i < users.Count; i++)
        {
            result += await DeleteUserAsync(users[i].Id);
        }

        return result;
    }

    public IMongoCollection<User> GetMongoTable()
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetUserAsync(int id)
    {
        var user = _users.Find(x => x.Id == id);

        return Task.FromResult(user);
    }

    public Task<List<User>> GetUsersAsync()
    {
        return Task.FromResult(_users);
    }

    public Task<long> UpdateUserAsync(User user)
    {
        var usr = _users.Find(x => x.Id == user.Id);
        if (usr is not null)
        {
            var index = _users.IndexOf(usr);
            _users[index] = user;

            return Task.FromResult(1L);
        }

        return Task.FromResult(0L);
    }
}