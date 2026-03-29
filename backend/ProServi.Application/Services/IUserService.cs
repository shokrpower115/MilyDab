using ProServi.Application.DTOs.Auth;

namespace ProServi.Application.Services;

public interface IUserService
{
    Task<UserDto> GetUserByIdAsync(int id);
    Task<UserDto> UpdateUserAsync(int id, UpdateUserDto dto);
}
