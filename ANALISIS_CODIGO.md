# 📋 Análisis Completo del Código - ProServi Backend

## 🏗️ Arquitectura General

```
API Request → AuthController → AuthService → UserRepository → PostgreSQL
                ↓
            AuthController maneja HTTP
                ↓
            AuthService contiene lógica de negocio
                ↓
            UserRepository accede a datos
                ↓
            Entities+ DTOs transfieren datos
```

**Estilo: Clean Architecture (4 capas)**
- **Domain**: Estructura pura (entidades, enums)
- **Application**: Lógica de negocio (servicios, DTOs)
- **Infrastructure**: Acceso a datos (repositories, EF Core, BCrypt)
- **API**: Controladores HTTP, Program.cs

---

## 📁 Estructura de Carpetas

```
backend/
├── ProServi.Domain/                    ← Entidades puras
│   ├── Entities/                       (12 entidades: User, Customer, Professional, etc)
│   └── Enums/                          (6 enums: UserRole, UserStatus, etc)
│
├── ProServi.Application/               ← Lógica de negocio
│   ├── Services/                       (IAuthService, AuthService)
│   ├── DTOs/                           (LoginRequest, RegisterCustomerRequest, etc)
│   └── Interfaces/                     (IPasswordHasher, IJwtTokenProvider, IUserRepository)
│
├── ProServi.Infrastructure/            ← Acceso a datos
│   ├── Data/                           (ProServiDbContext, SeedData)
│   ├── Repositories/                   (Repository<T>, UserRepository)
│   └── Identity/                       (PasswordHasher, JwtTokenProvider)
│
└── ProServi.Api/                       ← HTTP Endpoints
    ├── Controllers/                    (AuthController con 4 endpoints)
    ├── Program.cs                      (DI configuration)
    └── appsettings.json                (conexión a PostgreSQL, JWT secret)
```

---

## 🔐 Sistema de Autenticación

### 1️⃣ Registro de Cliente (`RegisterCustomerAsync`)

**Request:**
```json
{
  "email": "juan@example.com",
  "fullName": "Juan García",
  "phone": "+34612345678",
  "password": "Password123!",
  "confirmPassword": "Password123!",
  "city": "Madrid",
  "country": "Spain",
  "latitude": 40.4168,
  "longitude": -3.7038
}
```

**Pasos internos:**
1. ✅ Validar que email NO existe (`GetByEmailAsync`)
2. ✅ Crear `User` (entidad principal)
3. ✅ Hash de contraseña con BCrypt
4. ✅ Crear `Customer` (datos adicionales de cliente)
5. ✅ Guardar en BD (User + Customer)
6. ✅ Generar JWT Token
7. ✅ Retornar Token + UserData

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 2,
    "email": "juan@example.com",
    "fullName": "Juan García",
    "role": "Customer",
    "profilePhotoUrl": null
  }
}
```

### 2️⃣ Login (`LoginAsync`)

**Request:**
```json
{
  "email": "juan@example.com",
  "password": "Password123!"
}
```

**Pasos internos:**
1. ✅ Buscar usuario por email
2. ✅ Verificar contraseña con BCrypt (`VerifyPassword`)
3. ✅ Validar que Status = `Active` (si no, rechaza)
4. ✅ Actualizar `LastLogin` timestamp
5. ✅ Generar JWT Token
6. ✅ Retornar Token + UserData

**Response:** Mismo formato que registro

---

## 📊 Entidades Principales

### `User.cs` - Entidad Raíz
```csharp
public class User
{
    public int Id { get; set; }
    public string Email { get; set; }           // Único
    public string FullName { get; set; }
    public string Phone { get; set; }           // Único
    public string PasswordHash { get; set; }    // BCrypt
    public UserRole Role { get; set; }          // Customer | Professional
    public UserStatus Status { get; set; }      // Active | Inactive | Suspended
    public bool EmailVerified { get; set; }
    public bool PhoneVerified { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    public DateTime? LastLogin { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Relaciones
    public Customer? Customer { get; set; }          // 1-to-1
    public Professional? Professional { get; set; } // 1-to-1
}
```

### `Customer.cs` - Datos Específicos de Cliente
```csharp
public class Customer
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public decimal Latitude { get; set; }   // Para geolocalización
    public decimal Longitude { get; set; }
    public string? PreferredContactMethod { get; set; }
    public string? Street { get; set; }
    public string? ApartmentNumber { get; set; }
    public decimal AverageRating { get; set; }
    public int TotalProjects { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Relación
    public User User { get; set; }  // 1-to-1
}
```

### `Professional.cs` - Datos Específicos de Profesional
```csharp
public class Professional
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int SpecialtyId { get; set; }     // FK a Specialty
    public string Bio { get; set; }
    public int YearsOfExperience { get; set; }
    public string HourlyRate { get; set; }
    public bool IsVerified { get; set; }
    public bool IsAvailable { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public int CompletedProjects { get; set; }
    public DateTime? LastActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Relaciones
    public User User { get; set; }              // 1-to-1
    public Specialty Specialty { get; set; }   // Many-to-1
}
```

### `Specialty.cs` - Categorías de Profesionales
```csharp
public class Specialty
{
    public int Id { get; set; }
    public string Name { get; set; }               // "Plomería", "Electricidad", etc
    public DateTime CreatedAt { get; set; }
    
    // Relación
    public ICollection<Professional> Professionals { get; set; } // 1-to-Many
}
```

---

## 🔧 Servicios (Lógica de Negocio)

### `IAuthService` - Contrato
```csharp
public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterCustomerAsync(RegisterCustomerRequest request);
    Task<AuthResponse> RegisterProfessionalAsync(RegisterProfessionalRequest request);
    Task<bool> ValidateTokenAsync(string token);
}
```

### `AuthService` - Implementación
- **Responsabilidad:** Orquestar registro/login
- **Dependencias inyectadas:**
  - `IUserRepository` → acceso a datos
  - `IPasswordHasher` → encriptación de contraseñas
  - `IJwtTokenProvider` → generación de tokens

**Métodos clave:**
1. `RegisterCustomerAsync()` → Crear User + Customer
2. `LoginAsync()` → Validar y generar token
3. `RegisterProfessionalAsync()` → Crear User + Professional

---

## 💾 Layer de Datos (Repositories)

### `IRepository<T>` - Generic CRUD
```csharp
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task SaveAsync();
}
```

### `IUserRepository` - Especializado
```csharp
public interface IUserRepository : IRepository<User>
{
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByPhoneAsync(string phone);
    Task<bool> EmailExistsAsync(string email);
}
```

### `UserRepository` - Implementación
```csharp
public class UserRepository : Repository<User>, 
    IUserRepository, 
    ProServi.Application.Services.IUserRepository
{
    public async Task<User> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(u => u.Customer)      // Eager loading
            .Include(u => u.Professional)
            .FirstOrDefaultAsync(u => u.Email == email);
    }
    // ... más métodos
}
```

**Importante:** 
- Usa `.Include()` para cargar relaciones (evita N+1 queries)
- Implementa ambas interfaces (Infrastructure y Application) para inyección

---

## 🔐 Seguridad

### `IPasswordHasher` - Contrato
```csharp
public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}
```

### `PasswordHasher` - BCrypt
```csharp
public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    
    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
```

**Ventajas de BCrypt:**
- ✅ Slow hash (dificulta ataques de fuerza bruta)
- ✅ Salt incluido automáticamente
- ✅ Cada hash es diferente aunque sea mismo password

---

## 🎫 JWT Token

### `IJwtTokenProvider` - Contrato
```csharp
public interface IJwtTokenProvider
{
    string GenerateToken(User user);
}
```

### `JwtTokenProvider` - OpenID
```csharp
public class JwtTokenProvider : IJwtTokenProvider
{
    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

**Estructura del Token:**
- **Header:** `{alg: HS256, typ: JWT}`
- **Payload:** Claims (id, email, name, role)
- **Signature:** HMAC-SHA256 con SecretKey

---

## 🌐 HTTP Endpoints

### `AuthController`

#### 1. POST `/api/auth/register/customer`
```csharp
[HttpPost("register/customer")]
public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerRequest request)
{
    try
    {
        var response = await _authService.RegisterCustomerAsync(request);
        return Ok(response);
    }
    catch (ArgumentException ex)
    {
        return BadRequest(new { message = ex.Message });
    }
    catch (Exception ex)
    {
        _logger.LogError($"Error: {ex.Message}");
        return StatusCode(500, new { message = "Error interno del servidor" });
    }
}
```

**Status Codes:**
- ✅ 200 OK → Registro exitoso
- ❌ 400 Bad Request → Email duplicado
- ❌ 500 Internal Server Error → Error no esperado

#### 2. POST `/api/auth/register/professional`
Similar a customer, pero con campos adicionales (specialty, hourlyRate, bio)

#### 3. POST `/api/auth/login`
```csharp
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest request)
{
    try
    {
        var response = await _authService.LoginAsync(request);
        return Ok(response);
    }
    catch (UnauthorizedAccessException ex)
    {
        return Unauthorized(new { message = ex.Message });
    }
    // ...
}
```

**Status Codes:**
- ✅ 200 OK → Login exitoso
- ❌ 401 Unauthorized → Email/password incorrecto o usuario inactivo

#### 4. POST `/api/auth/validate-token`
Valida que un JWT token sea válido

---

## ⚙️ Configuración (Program.cs)

### Database
```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProServiDbContext>(options =>
    options.UseNpgsql(connectionString)
);
```

### Authentication
```csharp
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
        ClockSkew = TimeSpan.Zero,
        ValidateLifetime = true
    };
});
```

### Dependency Injection
```csharp
// Servicios
builder.Services.AddScoped<IAuthService, AuthService>();

// Repositorios
builder.Services.AddScoped<ProServi.Infrastructure.Repositories.IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Utilities
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
```

---

## 🗄️ Base de Datos

### Conexión (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=proservi_db;Username=postgres;Password=tu_password;"
  },
  "JwtSettings": {
    "SecretKey": "tu-clave-secreta-muy-larga-de-al-menos-32-caracteres",
    "ExpirationMinutes": 1440
  }
}
```

### Migrations
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Genera tablas:
- `Users` (principal)
- `Customers` (1-to-1 con Users)
- `Professionals` (1-to-1 con Users)
- `Specialties` (catálogo)
- Y 9 más para proyectos, pagos, reviews, etc.

---

## 🔄 Flujo de una Petición (Ejemplo: Register)

```
1. Cliente (Postman) → POST /api/auth/register/customer
   {email, fullName, phone, password, city, country, lat, lon}

2. AuthController.RegisterCustomer()
   ├─ Valida modelo [FromBody]
   └─ Llama: _authService.RegisterCustomerAsync(request)

3. AuthService.RegisterCustomerAsync()
   ├─ _userRepository.GetByEmailAsync(email)  ← Query a BD
   ├─ if (existingUser != null) → Throw ArgumentException
   ├─ new User { Email, FullName, Phone, PasswordHash=... }
   ├─ _passwordHasher.HashPassword(password)  ← BCrypt
   ├─ new Customer { User, City, Country, Latitude, Longitude }
   ├─ _userRepository.AddAsync(user)
   ├─ _userRepository.SaveAsync()             ← INSERT a BD
   ├─ _jwtTokenProvider.GenerateToken(user)
   └─ return AuthResponse { Token, User }

4. AuthController
   ├─ catch ArgumentException → 400 Bad Request
   ├─ catch Exception → 500 Internal Error
   └─ return Ok(response)

5. Cliente
   ← HTTP 200 OK
     {
       "token": "eyJhbGc...",
       "user": {id, email, fullName, role, ...}
     }
```

---

## ✅ Validaciones Implementadas

| Validación | Ubicación | Tipo |
|---|---|---|
| Email único | `AuthService.RegisterCustomerAsync()` | Lógica |
| Email formato | `[EmailAddress]` en `LoginRequest` | Data Annotation |
| Password longitud | `[MinLength(8)]` en `LoginRequest` | Data Annotation |
| Password match | `RegisterCustomerRequest` | Lógica |
| Usuario activo en login | `AuthService.LoginAsync()` | Lógica |
| Password correcto | `IPasswordHasher.VerifyPassword()` | Criptografía |

---

## 🐛 Problemas Encontrados y Resueltos

### 1. NullReferenceException en Registro
**Problema:** `_userRepository` era null en `AuthService`
**Causa:** DI mapping incorrecto (casting con `as` retornaba null)
**Solución:** `UserRepository` implementa ambas interfaces

### 2. Login rechazaba usuario válido
**Problema:** "Credenciales inválidas" aunque email/password eran correctos
**Causa:** Usuarios registrados con Status `PendingVerification`, login solo aceptaba `Active`
**Solución:** Cambiar registro a Status `Active`

### 3. Warnings de versiones de paquetes
**Problema:** `System.IdentityModel.Tokens.Jwt 7.1.2` vs `7.1.0`
**Impacto:** Ninguno, solo warning, código funciona igual

---

## 📦 NuGet Packages Utilizados

| Package | Versión | Propósito |
|---|---|---|
| `Microsoft.EntityFrameworkCore` | 8.0.0 | ORM |
| `Npgsql.EntityFrameworkCore.PostgreSQL` | 8.0.0 | Driver PostgreSQL |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 8.0.0 | JWT Auth |
| `System.IdentityModel.Tokens.Jwt` | 7.1.2 | JWT creation |
| `BCrypt.Net-Core` | 1.6.0 | Password hashing |
| `AutoMapper.Extensions.Microsoft.DependencyInjection` | 12.0.0 | DTO mapping |

---

## 🎯 Próximos Pasos

### Backend (Completar)
- [ ] Swagger/OpenAPI para documentación
- [ ] Logging avanzado (Serilog)
- [ ] Validaciones más robustas
- [ ] Email verification flow
- [ ] Password reset
- [ ] Endpoints de búsqueda de profesionales

### Frontend (Angular)
- [ ] Scaffolding del proyecto
- [ ] Auth service en Angular
- [ ] Login/Register components
- [ ] Interceptor para JWT
- [ ] Protección de rutas

---

## 📝 Comentarios Clave

✅ **Fortalezas:**
- Clean Architecture bien implementada
- Separación clara de responsabilidades
- Inyección de dependencias correcta
- Seguridad (BCrypt + JWT)
- Relaciones 1-to-1 bien modeladas
- Error handling con try-catch

⚠️ **Mejoras Futuras:**
- Logging centralizado (ahora hay `_logger.LogError()`)
- Validación de DTOs más completa
- Unit tests
- Integration tests
- Rate limiting
- HTTPS en producción
