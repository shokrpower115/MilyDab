using ProServi.Application.DTOs.Auth;
using ProServi.Domain.Entities;
using ProServi.Domain.Enums;

namespace ProServi.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenProvider _jwtTokenProvider;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenProvider jwtTokenProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenProvider = jwtTokenProvider;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        // Buscar usuario por email
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            throw new UnauthorizedAccessException("Email o contraseña incorrectos");

        // Verificar contraseña
        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Email o contraseña incorrectos");

        // Verificar que usuario esté activo
        if (user.Status != UserStatus.Active)
            throw new UnauthorizedAccessException("Usuario inactivo o suspendido");

        // Actualizar último login
        user.LastLogin = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        // Generar token JWT
        var token = _jwtTokenProvider.GenerateToken(user);

        // Construir respuesta
        return new AuthResponse
        {
            Token = token,
            User = new UserData
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                ProfilePhotoUrl = user.ProfilePhotoUrl
            }
        };
    }

    public async Task<AuthResponse> RegisterCustomerAsync(RegisterCustomerRequest request)
    {
        // Validar que email no exista
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
            throw new ArgumentException("El email ya está registrado");

        // Crear usuario
        var user = new User
        {
            Email = request.Email,
            FullName = request.FullName,
            Phone = request.Phone,
            Role = UserRole.Customer,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            Status = UserStatus.Active,
            EmailVerified = false,
            PhoneVerified = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Crear cliente
        var customer = new Customer
        {
            User = user,
            City = request.City,
            Country = request.Country,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        user.Customer = customer;

        // Guardar en base de datos
        await _userRepository.AddAsync(user);
        await _userRepository.SaveAsync();

        // Generar token
        var token = _jwtTokenProvider.GenerateToken(user);

        return new AuthResponse
        {
            Token = token,
            User = new UserData
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                ProfilePhotoUrl = user.ProfilePhotoUrl
            }
        };
    }

    public async Task<AuthResponse> RegisterProfessionalAsync(RegisterProfessionalRequest request)
    {
        // Validar que email no exista
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
            throw new ArgumentException("El email ya está registrado");

        // Crear usuario
        var user = new User
        {
            Email = request.Email,
            FullName = request.FullName,
            Phone = request.Phone,
            Role = UserRole.Professional,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            Status = UserStatus.Active,
            EmailVerified = false,
            PhoneVerified = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Crear profesional
        var professional = new Professional
        {
            User = user,
            SpecialtyId = request.SpecialtyId,
            City = request.City,
            Country = request.Country,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Bio = request.Bio,
            YearsOfExperience = request.YearsOfExperience,
            HourlyRate = request.HourlyRate?.ToString(),
            IsVerified = false,
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        user.Professional = professional;

        // Guardar en base de datos
        await _userRepository.AddAsync(user);
        await _userRepository.SaveAsync();

        // Generar token
        var token = _jwtTokenProvider.GenerateToken(user);

        return new AuthResponse
        {
            Token = token,
            User = new UserData
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                ProfilePhotoUrl = user.ProfilePhotoUrl
            }
        };
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            // Por ahora retorna true si no es nulo
            // La validación real se hace en middleware JWT de Program.cs
            return !string.IsNullOrEmpty(token);
        }
        catch
        {
            return false;
        }
    }
}
