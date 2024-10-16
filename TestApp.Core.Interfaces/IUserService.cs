using TestApp.Core.DTOs;

namespace TestApp.Core.Interfaces;

public interface IUserService
{
    public Task<UserDto> CreateAsync(UserCreateDto user);

    public Task<IEnumerable<UserDto>> GetConsentedUsersAsync();

    Task<IEnumerable<UserDto>> GetUsersByEmailAndNameAsync(string email, string name);
}
