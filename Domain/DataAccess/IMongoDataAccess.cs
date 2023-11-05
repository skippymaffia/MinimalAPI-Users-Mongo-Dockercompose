using Domain.Model;
using MongoDB.Driver;

namespace Domain.DataAccess;

public interface IMongoDataAccess
{
    Task CreateUser(User user);
    Task CreateUsers(List<User> users);
    Task<long> DeleteUserAsync(int id);
    Task<long> DeleteUsersAsync(List<User> users);
    IMongoCollection<User> GetMongoTable();
    Task<User?> GetUserAsync(int id);
    Task<List<User>> GetUsersAsync();
    Task<long> UpdateUserAsync(User user);
}