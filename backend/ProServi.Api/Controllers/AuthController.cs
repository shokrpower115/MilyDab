using Microsoft.AspNetCore.Mvc;
using ProServi.Application.DTOs.Auth;
using ProServi.Application.Services;

namespace ProServi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Endpoint de login para usuarios registrados
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning($"Login fallido para email: {request.Email} - {ex.Message}");
            return Unauthorized(new { message = "Credenciales inválidas" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en login");
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }

    /// <summary>
    /// Endpoint de registro para clientes (usuarios que buscan profesionales)
    /// </summary>
    [HttpPost("register/customer")]
    public async Task<ActionResult<AuthResponse>> RegisterCustomer([FromBody] RegisterCustomerRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterCustomerAsync(request);
            return StatusCode(201, result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Error en registro de cliente: {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en registro de cliente");
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }

    /// <summary>
    /// Endpoint de registro para profesionales (usuarios que ofrecen servicios)
    /// </summary>
    [HttpPost("register/professional")]
    public async Task<ActionResult<AuthResponse>> RegisterProfessional([FromBody] RegisterProfessionalRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterProfessionalAsync(request);
            return StatusCode(201, result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Error en registro de profesional: {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en registro de profesional");
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }

    /// <summary>
    /// Endpoint para validar un token JWT
    /// </summary>
    [HttpPost("validate-token")]
    public async Task<ActionResult> ValidateToken([FromBody] ValidateTokenRequest request)
    {
        try
        {
            var isValid = await _authService.ValidateTokenAsync(request.Token);
            if (isValid)
                return Ok(new { valid = true });
            else
                return Unauthorized(new { valid = false, message = "Token inválido o expirado" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando token");
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }
}

public class ValidateTokenRequest
{
    public string Token { get; set; }
}
