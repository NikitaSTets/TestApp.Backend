using MongoDB.Driver;
using TestApp.Core.Entities.Entities;
using TestApp.Core.Interfaces;

namespace TestApp.Infrastrucutre.Data.Data;

public class MongoUserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public MongoUserRepository(IMongoDatabase database)
    {
        _users = database.GetCollection<User>("Users");
        CreateIndexes();
    }

    public async Task AddUserAsync(User user)
    {
        await _users.InsertOneAsync(user);
    }

    public async Task<IEnumerable<User>> GetConsentedUsersAsync()
    {
        return await _users.Find(user => user.ConsentToStoreData).ToListAsync();
    }

    public async Task<IEnumerable<User>> GetUsersByEmailAndNameAsync(string email, string name)
    {
        var filter = Builders<User>.Filter.And(
            Builders<User>.Filter.Eq(u => u.Email, email),
            Builders<User>.Filter.Eq(u => u.Name, name)
        );

        return await _users.Find(filter).ToListAsync();
    }

    private void CreateIndexes()
    {
        var indexKeysDefinition = Builders<User>.IndexKeys.Combine(
            Builders<User>.IndexKeys.Ascending(u => u.Email),
            Builders<User>.IndexKeys.Ascending(u => u.Name)
        );

        var indexModel = new CreateIndexModel<User>(indexKeysDefinition);
        _users.Indexes.CreateOne(indexModel);
    }
}