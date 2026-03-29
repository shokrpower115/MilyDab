using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProServi.Infrastructure.Data;
using ProServi.Application.Services;
using ProServi.Infrastructure.Repositories;
using ProServi.Infrastructure.Identity;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// === SERVICIOS ===

// Controladores
builder.Services.AddControllers();

// Base de Datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProServiDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// Configuración JWT
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
        ClockSkew = TimeSpan.Zero,
        ValidateLifetime = true
    };
});

// Servicios de aplicación (Dependency Injection)
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProfessionalService, ProfessionalService>();

// Repositorios
builder.Services.AddScoped<ProServi.Infrastructure.Repositories.IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Registrar la misma instancia de UserRepository también para Application.Services.IUserRepository
builder.Services.AddScoped<ProServi.Application.Services.IUserRepository>(sp =>
    sp.GetRequiredService<ProServi.Infrastructure.Repositories.IUserRepository>() as ProServi.Application.Services.IUserRepository);

// Utilidades
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();

// CORS - Permitir peticiones desde Angular (localhost:4200)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// AutoMapper - Mapear entidades a DTOs
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// === CONSTRUCCIÓN DE APP ===

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    // Swagger se habilita si está instalado
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

// Comentado para desarrollo local (sin certificado HTTPS confiado)
// app.UseHttpsRedirection();

app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// === INICIALIZACIÓN DE BD ===

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProServiDbContext>();
    
    // Aplicar migraciones automáticamente
    dbContext.Database.Migrate();
    
    // Opcionalmente, seed de datos si BD está vacía
    if (!dbContext.Specialties.Any())
    {
        // Las especialidades se insertan en DbContext.OnModelCreating()
        await dbContext.SaveChangesAsync();
    }
}

app.Run();
