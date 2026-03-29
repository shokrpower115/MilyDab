using ProServi.Application.DTOs.Auth;

namespace ProServi.Application.Services;

public class UserService : IUserService
{
    // Nota: En una implementación real, esto tendría inyección de IUserRepository
    // pero como Application no debe depender de Infrastructure,
    // esta lógica se movería al layer de Infrastructure
    
    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        throw new NotImplementedException("Este método debe ser implementado con inyección de dependencias de Infrastructure");
    }

    public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto dto)
    {
        throw new NotImplementedException("Este método debe ser implementado con inyección de dependencias de Infrastructure");
    }
}
