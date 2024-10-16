﻿using TestApp.Core.DTOs;

namespace TestApp.Core.Interfaces;

public interface IUserService
{
    public Task<UserDto> CreateAsync(UserCreateDto user);

    public Task<IEnumerable<UserDto>> GetAllAsync();

    Task<IEnumerable<UserDto>> GetUsersByEmailAndNameAsync(string email, string name);
}
