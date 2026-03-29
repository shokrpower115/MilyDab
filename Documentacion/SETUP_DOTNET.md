# 🚀 GUÍA: CREAR PROYECTO .NET

## Paso 1: Crear solución y proyectos

```powershell
# Navegar a la carpeta backend
cd c:\dev\ProServi\backend

# Crear solución
dotnet new sln -n ProServi.Api

# Crear proyectos por capas
dotnet new webapi -n ProServi.Api
dotnet new classlib -n ProServi.Application
dotnet new classlib -n ProServi.Domain
dotnet new classlib -n ProServi.Infrastructure

# Agregar proyectos a la solución
dotnet sln ProServi.Api.sln add ProServi.Api/ProServi.Api.csproj
dotnet sln ProServi.Api.sln add ProServi.Application/ProServi.Application.csproj
dotnet sln ProServi.Api.sln add ProServi.Domain/ProServi.Domain.csproj
dotnet sln ProServi.Api.sln add ProServi.Infrastructure/ProServi.Infrastructure.csproj

# Agregar referencias entre proyectos
cd ProServi.Api
dotnet add reference ../ProServi.Application/ProServi.Application.csproj
dotnet add reference ../ProServi.Domain/ProServi.Domain.csproj

cd ../ProServi.Application
dotnet add reference ../ProServi.Domain/ProServi.Domain.csproj
dotnet add reference ../ProServi.Infrastructure/ProServi.Infrastructure.csproj

cd ../ProServi.Infrastructure
dotnet add reference ../ProServi.Domain/ProServi.Domain.csproj
```

---

## Paso 2: Instalar NuGet Packages

```powershell
cd c:\dev\ProServi\backend\ProServi.Infrastructure

# Entity Framework Core para PostgreSQL
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0

# JWT Authentication
dotnet add package System.IdentityModel.Tokens.Jwt --version 7.1.0
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0

# Password Hashing
dotnet add package BCrypt.Net-Next --version 4.0.3

cd ../ProServi.Application
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1

cd ../ProServi.Api
# Los demás packages vienen automáticamente con las referencias
```

---

## Paso 3: Estructura de carpetas para cada proyecto

### **ProServi.Domain/**
```
ProServi.Domain/
├── Entities/
│   ├── User.cs
│   ├── Customer.cs
│   ├── Professional.cs
│   ├── ContactRequest.cs
│   ├── Budget.cs
│   ├── Project.cs
│   ├── Payment.cs
│   ├── Review.cs
│   ├── Notification.cs
│   ├── Certification.cs
│   ├── Specialty.cs
│   └── SavedCard.cs
├── Enums/
│   ├── UserRole.cs
│   ├── UserStatus.cs
│   ├── ContactRequestStatus.cs
│   ├── ContactMethod.cs
│   ├── PaymentStatus.cs
│   └── ... (otros enums)
└── ValueObjects/
    ├── Money.cs
    └── Address.cs
```

### **ProServi.Application/**
```
ProServi.Application/
├── DTOs/
│   ├── Auth/
│   │   ├── LoginRequest.cs
│   │   ├── RegisterCustomerRequest.cs
│   │   ├── RegisterProfessionalRequest.cs
│   │   └── AuthResponse.cs
│   ├── User/
│   │   ├── UserDto.cs
│   │   └── UpdateUserDto.cs
│   ├── Professional/
│   │   ├── ProfessionalDto.cs
│   │   ├── ProfessionalDetailDto.cs
│   │   └── SearchProfessionalResponse.cs
│   └── ... (otros DTOs)
├── Services/
│   ├── IAuthService.cs
│   ├── AuthService.cs
│   ├── IUserService.cs
│   ├── UserService.cs
│   ├── IProfessionalService.cs
│   ├── ProfessionalService.cs
│   └── ... (otros servicios)
├── Mappers/
│   └── MappingProfile.cs
└── Exceptions/
    ├── DomainException.cs
    └── ValidationException.cs
```

### **ProServi.Infrastructure/**
```
ProServi.Infrastructure/
├── Data/
│   ├── ProServiDbContext.cs
│   ├── Configurations/
│   │   ├── UserConfiguration.cs
│   │   ├── CustomerConfiguration.cs
│   │   ├── ProfessionalConfiguration.cs
│   │   └── ... (otras configuraciones)
│   └── Migrations/
│       └── (generadas por EF)
├── Repositories/
│   ├── IRepository.cs
│   ├── Repository.cs
│   ├── IUserRepository.cs
│   ├── UserRepository.cs
│   └── ... (otros repositorios)
└── Identity/
    ├── JwtTokenProvider.cs
    ├── PasswordHasher.cs
    └── Claims.cs
```

### **ProServi.Api/**
```
ProServi.Api/
├── Controllers/
│   ├── AuthController.cs
│   ├── UsersController.cs
│   ├── ProfessionalsController.cs
│   ├── ContactRequestsController.cs
│   └── ... (otros controladores)
├── Middleware/
│   ├── ExceptionHandlingMiddleware.cs
│   └── LoggingMiddleware.cs
├── Program.cs
├── appsettings.json
└── appsettings.Development.json
```

---

## Paso 4: Archivos principales para empezar

### **ProServi.Domain/Enums/UserRole.cs**
```csharp
namespace ProServi.Domain.Enums;

public enum UserRole
{
    Customer,
    Professional
}
```

### **ProServi.Domain/Entities/User.cs**
```csharp
using ProServi.Domain.Enums;

namespace ProServi.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public UserRole Role { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string PasswordHash { get; set; }
    public string FullName { get; set; }
    public string ProfilePhotoUrl { get; set; }
    public UserStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool EmailVerified { get; set; }
    public bool PhoneVerified { get; set; }
    public DateTime? LastLogin { get; set; }

    // Relaciones
    public virtual Customer Customer { get; set; }
    public virtual Professional Professional { get; set; }
    public virtual ICollection<Notification> Notifications { get; set; }
    public virtual ICollection<SavedCard> SavedCards { get; set; }
}

public enum UserStatus
{
    Active,
    Inactive,
    Suspended,
    PendingVerification
}
```

### **ProServi.Application/DTOs/Auth/LoginRequest.cs**
```csharp
using System.ComponentModel.DataAnnotations;

namespace ProServi.Application.DTOs.Auth;

public class LoginRequest
{
    [Required(ErrorMessage = "Email es requerido")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Contraseña es requerida")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    public string Password { get; set; }
}
```

### **ProServi.Application/DTOs/Auth/AuthResponse.cs**
```csharp
namespace ProServi.Application.DTOs.Auth;

public class AuthResponse
{
    public string Token { get; set; }
    public UserData User { get; set; }
}

public class UserData
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }
    public string ProfilePhotoUrl { get; set; }
}
```

### **ProServi.Application/Services/IAuthService.cs**
```csharp
using ProServi.Application.DTOs.Auth;

namespace ProServi.Application.Services;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterCustomerAsync(RegisterCustomerRequest request);
    Task<AuthResponse> RegisterProfessionalAsync(RegisterProfessionalRequest request);
    Task<bool> ValidateTokenAsync(string token);
}
```

### **ProServi.Api/Controllers/AuthController.cs**
```csharp
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

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning($"Login fallido para email: {request.Email}");
            return Unauthorized(new { message = "Credenciales inválidas" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en login");
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }

    [HttpPost("register/customer")]
    public async Task<ActionResult<AuthResponse>> RegisterCustomer([FromBody] RegisterCustomerRequest request)
    {
        try
        {
            var result = await _authService.RegisterCustomerAsync(request);
            return CreatedAtAction(nameof(RegisterCustomer), result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en registro de cliente");
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }

    [HttpPost("register/professional")]
    public async Task<ActionResult<AuthResponse>> RegisterProfessional([FromBody] RegisterProfessionalRequest request)
    {
        try
        {
            var result = await _authService.RegisterProfessionalAsync(request);
            return CreatedAtAction(nameof(RegisterProfessional), result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en registro de profesional");
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }
}
```

### **ProServi.Api/Program.cs** (Configuración principal)
```csharp
using ProServi.Application.Services;
using ProServi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProServiDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProfessionalService, ProfessionalService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Database migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProServiDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
```

### **appsettings.json**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=proservi_db;Username=postgres;Password=tu_password;"
  },
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-that-is-at-least-32-characters-long-for-security",
    "ExpirationMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

---

## Paso 5: Entity Framework Core DbContext

### **ProServi.Infrastructure/Data/ProServiDbContext.cs**
```csharp
using Microsoft.EntityFrameworkCore;
using ProServi.Domain.Entities;

namespace ProServi.Infrastructure.Data;

public class ProServiDbContext : DbContext
{
    public ProServiDbContext(DbContextOptions<ProServiDbContext> options) 
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Professional> Professionals { get; set; }
    public DbSet<ContactRequest> ContactRequests { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Certification> Certifications { get; set; }
    public DbSet<Specialty> Specialties { get; set; }
    public DbSet<SavedCard> SavedCards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar relaciones y constraints
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Phone)
            .IsUnique();

        modelBuilder.Entity<Customer>()
            .HasOne(c => c.User)
            .WithOne(u => u.Customer)
            .HasForeignKey<Customer>(c => c.UserId);

        modelBuilder.Entity<Professional>()
            .HasOne(p => p.User)
            .WithOne(u => u.Professional)
            .HasForeignKey<Professional>(p => p.UserId);

        // Seeding inicial
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Insertar especialidades
        var specialties = new[]
        {
            new Specialty { Id = 1, Name = "Plomería", Description = "Servicios de reparación y instalación de tuberías" },
            new Specialty { Id = 2, Name = "Electricidad", Description = "Trabajos eléctricos y reparación" },
            new Specialty { Id = 3, Name = "Pintura", Description = "Servicios de pintura interior y exterior" },
            new Specialty { Id = 4, Name = "Carpintería", Description = "Trabajos de carpintería y muebles" },
            new Specialty { Id = 5, Name = "Limpieza", Description = "Servicios de limpieza profesional" },
            new Specialty { Id = 6, Name = "Reparación General", Description = "Reparaciones varias en el hogar" }
        };

        modelBuilder.Entity<Specialty>().HasData(specialties);
    }
}
```

---

## 🧪 Paso 6: Crear migraciones

```powershell
cd c:\dev\ProServi\backend

# Crear inicial migration
dotnet ef migrations add InitialCreate --project ProServi.Infrastructure --startup-project ProServi.Api

# Aplicar migración (opcional, también se aplica al iniciar la app)
dotnet ef database update --project ProServi.Infrastructure --startup-project ProServi.Api
```

---

## ✅ Checklist

- [ ] Proyectos .NET creados
- [ ] Referencias entre proyectos agregadas
- [ ] NuGet packages instalados
- [ ] Estructura de carpetas creada
- [ ] Entidades básicas creadas
- [ ] DTOs creados
- [ ] AuthController creado
- [ ] DbContext configurado
- [ ] appsettings.json actualizado con conexión a BD
- [ ] Migraciones ejecutadas

---

## 🚀 Próximo: Ejecutar la aplicación

```powershell
cd c:\dev\ProServi\backend\ProServi.Api
dotnet run
```

La API estará disponible en: `https://localhost:5001/swagger`

---

**Guía Creada**: 28/03/2026
