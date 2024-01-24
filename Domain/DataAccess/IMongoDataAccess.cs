using Domain.Model;
using MongoDB.Driver;

namespace Domain.DataAccess;

public interface IMongoDataAccess
{
    IMongoCollection<User> GetMongoTable();

    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserAsync(int id);

    Task<long> CreateUserAsync(User? user);
    Task<long> CreateUsersAsync(List<User> users);

    Task<long> UpdateUserAsync(User? user);

    Task<long> DeleteUserAsync(int id);
    Task<long> DeleteUsersAsync(List<User> users);
}