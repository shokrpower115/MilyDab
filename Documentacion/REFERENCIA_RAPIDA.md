# 📖 REFERENCIA RÁPIDA: COMANDOS Y SNIPPETS

## 🗄️ POSTGRESQL - COMANDOS ESENCIALES

### **Conectar a la base de datos**
```powershell
# Conectar con usuario postgres
psql -U postgres

# Conectar a una BD específica
psql -U postgres -d proservi_db

# Ejecutar archivo SQL
psql -U postgres -d proservi_db -f 'c:/dev/ProServi/database/migrations/001_create_tables.sql'
```

### **Comandos de inspección**
```sql
-- Listar todas las tablas
\dt

-- Listar especialidades
SELECT * FROM specialties;

-- Contar usuarios
SELECT COUNT(*) FROM users;

-- Ver estructura de tabla
\d+ users

-- Listar índices
\di

-- Ver vistas creadas
\dv
```

### **Gestión de datos**
```sql
-- Crear base de datos
CREATE DATABASE proservi_db;

-- Eliminar base de datos
DROP DATABASE proservi_db;

-- Ver el tamaño de la BD
SELECT pg_size_pretty(pg_database_size('proservi_db'));

-- Respaldar BD
pg_dump -U postgres proservi_db > backup.sql

-- Restaurar BD
psql -U postgres proservi_db < backup.sql
```

---

## 🖥️ .NET - COMANDOS ESENCIALES

### **Crear proyecto desde cero**
```powershell
# Crear solución
dotnet new sln -n ProServi.Api

# Crear proyectos
dotnet new classlib -n ProServi.Domain -o src/ProServi.Domain
dotnet new classlib -n ProServi.Application -o src/ProServi.Application
dotnet new classlib -n ProServi.Infrastructure -o src/ProServi.Infrastructure
dotnet new webapi -n ProServi.Api -o src/ProServi.Api

# Agregar proyectos a solución
dotnet sln add src/ProServi.Domain/ProServi.Domain.csproj
dotnet sln add src/ProServi.Application/ProServi.Application.csproj
dotnet sln add src/ProServi.Infrastructure/ProServi.Infrastructure.csproj
dotnet sln add src/ProServi.Api/ProServi.Api.csproj
```

### **Gestión de NuGet**
```powershell
# Instalar paquete en proyecto específico
dotnet add src/ProServi.Infrastructure/ProServi.Infrastructure.csproj package Npgsql.EntityFrameworkCore.PostgreSQL

# Listar paquetes instalados
dotnet list package

# Actualizar un paquete
dotnet package update Npgsql.EntityFrameworkCore.PostgreSQL
```

### **Entity Framework**
```powershell
# Crear migración
dotnet ef migrations add InitialCreate --project src/ProServi.Infrastructure

# Ver migraciones
dotnet ef migrations list

# Actualizar BD
dotnet ef database update

# Deshacer última migración
dotnet ef migrations remove

# Generar script SQL
dotnet ef migrations script -o migration.sql
```

### **Ejecutar aplicación**
```powershell
# Debug
dotnet run

# Con watch (reinicia automáticamente)
dotnet watch run

# Especificar puerto
dotnet run --urls "https://localhost:5001"

# Publicar
dotnet publish -c Release -o published/
```

---

## 🅰️ ANGULAR - COMANDOS ESENCIALES

### **Crear proyecto**
```powershell
# Crear nuevo proyecto
ng new ProServi --routing --style=css

# Crear proyecto en carpeta actual
ng new . --routing --style=css --skip-git=true

# Crear proyecto con skip de npm install
ng new . --skip-install
```

### **Generar componentes/servicios**
```powershell
# Componente
ng generate component modules/auth/components/login
ng g c modules/auth/components/login  # Shorthand

# Servicio
ng generate service core/services/auth
ng g s core/services/auth  # Shorthand

# Módulo
ng generate module modules/home

# Guard
ng generate guard core/guards/auth

# Interceptor
ng generate interceptor core/interceptors/auth

# Pipe
ng generate pipe shared/pipes/highlight
```

### **Desarrollo**
```powershell
# Servidor de desarrollo
ng serve

# Con puerto específico
ng serve --port 4300

# Build para producción
ng build

# Build optimizado
ng build --configuration production

# Linting
ng lint

# Pruebas
ng test

# E2E tests
ng e2e
```

### **Instalación de paquetes**
```powershell
# Instalar Angular Material
ng add @angular/material

# Instalar Tailwind CSS
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p

# Instalar dependencias comunes
npm install axios jwt-decode
npm install @ngx-translate/core @ngx-translate/http-loader
npm install bootstrap  # o cualquier otro UI framework
```

---

## 📁 ESTRUCTURA DE CARPETAS .NET

```
backend/
├── src/
│   ├── ProServi.Domain/
│   │   ├── Entities/
│   │   │   ├── User.cs
│   │   │   ├── Customer.cs
│   │   │   ├── Professional.cs
│   │   │   ├── ContactRequest.cs
│   │   │   ├── Budget.cs
│   │   │   ├── Project.cs
│   │   │   ├── Payment.cs
│   │   │   └── Review.cs
│   │   └── Enums/
│   │       ├── UserRole.cs
│   │       ├── ContactMethod.cs
│   │       └── PaymentStatus.cs
│   │
│   ├── ProServi.Application/
│   │   ├── DTOs/
│   │   │   ├── LoginRequest.cs
│   │   │   ├── AuthResponse.cs
│   │   │   └── ...
│   │   ├── Services/
│   │   │   ├── IAuthService.cs
│   │   │   ├── AuthService.cs
│   │   │   └── ...
│   │   └── Validators/
│   │       └── LoginRequestValidator.cs
│   │
│   ├── ProServi.Infrastructure/
│   │   ├── Data/
│   │   │   ├── ProServiDbContext.cs
│   │   │   └── Migrations/
│   │   ├── Repositories/
│   │   │   └── UserRepository.cs
│   │   └── Configuration/
│   │       └── EntityConfiguration.cs
│   │
│   └── ProServi.Api/
│       ├── Controllers/
│       │   ├── AuthController.cs
│       │   ├── ProfessionalsController.cs
│       │   └── ...
│       ├── Program.cs
│       ├── appsettings.json
│       └── appsettings.Development.json
│
└── ProServi.Api.sln
```

---

## 📁 ESTRUCTURA DE CARPETAS ANGULAR

```
frontend/
├── src/
│   ├── app/
│   │   ├── core/
│   │   │   ├── guards/
│   │   │   │   ├── auth.guard.ts
│   │   │   │   └── not-auth.guard.ts
│   │   │   ├── interceptors/
│   │   │   │   └── auth.interceptor.ts
│   │   │   └── services/
│   │   │       ├── auth.service.ts
│   │   │       ├── user.service.ts
│   │   │       └── professional.service.ts
│   │   │
│   │   ├── modules/
│   │   │   ├── auth/
│   │   │   │   ├── components/
│   │   │   │   │   ├── login/
│   │   │   │   │   ├── register/
│   │   │   │   │   └── role-selector/
│   │   │   │   ├── auth.module.ts
│   │   │   │   └── auth-routing.module.ts
│   │   │   │
│   │   │   ├── home/
│   │   │   │   ├── components/
│   │   │   │   │   ├── dashboard/
│   │   │   │   │   ├── search/
│   │   │   │   │   └── navbar/
│   │   │   │   ├── home.module.ts
│   │   │   │   └── home-routing.module.ts
│   │   │   │
│   │   │   └── shared/
│   │   │       ├── components/
│   │   │       │   ├── button/
│   │   │       │   ├── card/
│   │   │       │   └── modal/
│   │   │       ├── pipes/
│   │   │       └── directives/
│   │   │
│   │   ├── app-routing.module.ts
│   │   ├── app.module.ts
│   │   ├── app.component.ts
│   │   └── app.component.html
│   │
│   ├── assets/
│   │   ├── images/
│   │   ├── icons/
│   │   └── logo/
│   │
│   ├── styles.css
│   ├── styles/
│   │   └── tailwind.css
│   │
│   └── main.ts
│
├── angular.json
├── package.json
├── tailwind.config.js
├── postcss.config.js
└── tsconfig.json
```

---

## 🔑 VARIABLES DE ENTORNO

### **.env Backend (appsettings.json)**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=proservi_db;Username=postgres;Password=YOUR_PASSWORD"
  },
  "Jwt": {
    "SecretKey": "YOUR_VERY_LONG_SECRET_KEY_AT_LEAST_32_CHARACTERS_LONG",
    "ExpirationHours": 24,
    "Issuer": "ProServi",
    "Audience": "ProServi.User"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

### **Variables Frontend (environment.ts)**
```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001',
  apiBaseUrl: 'https://localhost:5001/api',
  jwtTokenName: 'auth_token',
  defaultPageSize: 20,
  maxPageSize: 100
};
```

---

## 🔐 JWT TOKENS - CHEAT SHEET

### **Estructura de un JWT**
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
```

**Parte 1 (Header)**: `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9` → {alg: "HS256", typ: "JWT"}
**Parte 2 (Payload)**: `eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ` → Datos del usuario
**Parte 3 (Signature)**: `SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c` → Validación

### **Decodificar JWT en Angular**
```typescript
import * as jwt_decode from 'jwt-decode';

const token = localStorage.getItem('token');
const decoded = jwt_decode.jwtDecode(token);

console.log(decoded);
// Output: { id: 1, email: "user@example.com", role: "CUSTOMER", iat: ..., exp: ... }
```

### **Decodificar JWT en .NET**
```csharp
var tokenHandler = new JwtSecurityTokenHandler();
var token = tokenHandler.ReadToken(jwtToken) as JwtSecurityToken;

var userId = token?.Claims.First(claim => claim.Type == "id").Value;
var email = token?.Claims.First(claim => claim.Type == "email").Value;
```

---

## 🧪 PRUEBAS CON CURL

### **Login**
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "TestPassword123!"
  }'
```

### **Registrar Cliente**
```bash
curl -X POST https://localhost:5001/api/auth/register/customer \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "Juan Pérez",
    "email": "juan@example.com",
    "phone": "+34912345678",
    "city": "Madrid",
    "password": "TestPassword123!"
  }'
```

### **Búsqueda de Profesionales**
```bash
curl -X GET "https://localhost:5001/api/professionals/search?specialtyId=1&city=Madrid" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### **Crear Solicitud de Contacto**
```bash
curl -X POST https://localhost:5001/api/contact-requests \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{
    "professionalId": 3,
    "contactMethod": "VISIT",
    "description": "Necesito revisar una fuga de agua",
    "preferredDateTime": "2024-04-05T14:00:00",
    "address": "Calle Principal 123, Madrid"
  }'
```

---

## 🐛 DEBUGGING

### **Visual Studio Code - Launch Configuration (.vscode/launch.json)**
```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": ".NET Backend",
      "type": "cppdbg",
      "request": "launch",
      "program": "${workspaceFolder}/backend/src/ProServi.Api/bin/Debug/net8.0/ProServi.Api",
      "args": [],
      "stopAtEntry": false,
      "cwd": "${workspaceFolder}/backend",
      "environment": [
        {
          "name": "ASPNETCORE_ENVIRONMENT",
          "value": "Development"
        }
      ],
      "externalConsole": false,
      "MIMode": "gdb"
    }
  ]
}
```

### **Console Logs comunes**

**Angular**:
```typescript
console.log('Autenticación', authResponse);
console.table(professionals);
console.error('Error:', error);
```

**.NET**:
```csharp
_logger.LogInformation("Usuario {Email} logueado", user.Email);
_logger.LogError("Error al buscar profesionales: {Error}", ex.Message);
Debug.WriteLine("Debug message");
```

---

## 🚀 DEPLOYMENT RÁPIDO

### **Backend - Publicar en IIS**
```powershell
# Compilar
dotnet publish -c Release -o published/

# Crear pool de aplicaciones en IIS
# Copiar carpeta 'published' a C:\inetpub\wwwroot\proservi-api
# Configurar binding en IIS (Puerto 443, HTTPS)
```

### **Frontend - Compilar para producción**
```powershell
# Build optimizado
ng build --configuration production

# Resultado en dist/ProServi/
# Copiar a servidor web (Nginx/Apache/IIS)
```

---

## 📊 QUERIES ÚTILES PARA TESTING

```sql
-- Verificar usuarios creados
SELECT id, email, fullName, role, createdAt FROM users ORDER BY createdAt DESC;

-- Ver solicitudes de contacto
SELECT cr.id, cr.status, u.fullName as customer, p.fullName as professional
FROM contact_requests cr
JOIN users u ON cr.customer_id = u.id
JOIN users p ON cr.professional_id = p.id;

-- Pagos por estado
SELECT status, COUNT(*) as total, SUM(amount) as monto_total
FROM payments
GROUP BY status;

-- Profesionales mejor valorados
SELECT u.fullName, AVG(r.rating) as promedio_rating, COUNT(r.id) as total_reviews
FROM users u
JOIN professionals pr ON u.id = pr.user_id
LEFT JOIN reviews r ON pr.id = r.professional_id
GROUP BY u.id, u.fullName
ORDER BY promedio_rating DESC;
```

---

**Referencia Rápida v1.0**
**Última actualización**: 28/03/2026
