using ProServi.Application.DTOs.Auth;

namespace ProServi.Application.Services;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterCustomerAsync(RegisterCustomerRequest request);
    Task<AuthResponse> RegisterProfessionalAsync(RegisterProfessionalRequest request);
    Task<bool> ValidateTokenAsync(string token);
}
