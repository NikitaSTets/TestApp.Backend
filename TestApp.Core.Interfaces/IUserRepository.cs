using TestApp.Core.Entities.Entities;

namespace TestApp.Core.Interfaces;

public interface IUserRepository
{
    Task AddUserAsync(User user);

    Task<IEnumerable<User>> GetConsentedUsersAsync();

    Task<IEnumerable<User>> GetUsersByEmailAndNameAsync(string email, string name);
}