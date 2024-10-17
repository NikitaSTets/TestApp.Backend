using TestApp.Core.DTOs;
using TestApp.Core.Entities.Entities;
using TestApp.Core.Interfaces;

namespace TestApp.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserDto> CreateAsync(UserCreateDto userDto)
    {
        var user = new User
        {
            Name = userDto.Name,
            Email = userDto.Email,
            ConsentToStoreData = userDto.ConsentToStoreData,
            PasswordHash = _passwordHasher.HashPassword(userDto.Password)
        };

        await _userRepository.AddUserAsync(user);

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            ConsentToStoreData = user.ConsentToStoreData
        };
    }

    public async Task<IEnumerable<UserDto>> GetConsentedUsersAsync()
    {
        var users = await _userRepository.GetConsentedUsersAsync();

        return users.Select(user => new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            ConsentToStoreData = user.ConsentToStoreData
        });
    }

    public async Task<IEnumerable<UserDto>> GetUsersByEmailAndNameAsync(string email, string name)
    {
        var users = await _userRepository.GetUsersByEmailAndNameAsync(email, name);

        return users.Select(user => new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            ConsentToStoreData = user.ConsentToStoreData
        });
    }
}
