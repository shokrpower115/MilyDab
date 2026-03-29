# ✅ CHECKLIST DE IMPLEMENTACIÓN - PROSERVI

## 📋 Estructura de Checklists

Este documento contiene checklists organizados por fase para asegurar que nada se olvide durante la implementación.

---

## 🔧 FASE 0: PREPARACIÓN

### Pre-requisitos
- [ ] PostgreSQL 9.2+ instalado y corriendo
- [ ] Node.js 18+ instalado
- [ ] .NET 8 SDK instalado
- [ ] VS Code instalado con extensiones:
  - [ ] C# Dev Kit
  - [ ] Angular Language Service
  - [ ] REST Client (para probar APIs)
  - [ ] PostgreSQL Explorer
  - [ ] Tailwind CSS IntelliSense

### Documentación Leída
- [ ] GUIA_MAESTRA.md
- [ ] DASHBOARD_PROYECTO.md
- [ ] API_CONTRACTS.md
- [ ] FlujoExito.txt
- [ ] PLAN_DESARROLLO_FASE_1.md

### Configuración Inicial
- [ ] Puerto 5432 (PostgreSQL) disponible
- [ ] Puerto 5001 (Backend) disponible
- [ ] Puerto 4200 (Frontend) disponible
- [ ] Git configurado (opcional pero recomendado)
- [ ] Carpeta de trabajo creada: c:\dev\ProServi

---

## 💾 FASE 1: BASE DE DATOS

### Crear Base de Datos
- [ ] Abrir PostgreSQL psql
- [ ] Ejecutar: `CREATE DATABASE proservi_db;`
- [ ] Conectarse a la BD: `\c proservi_db`

### Ejecutar Script SQL
- [ ] Ejecutar archivo `001_create_tables.sql`
- [ ] Verificar que no hay errores
- [ ] Comando: `\dt` para ver las 16 tablas

### Verificar Estructura
- [ ] Tabla `users` creada con columnas correctas
- [ ] Tabla `customers` creada
- [ ] Tabla `professionals` creada
- [ ] Tabla `specialties` creada
- [ ] Tabla `contact_requests` creada
- [ ] Tabla `budgets` creada
- [ ] Tabla `projects` creada
- [ ] Tabla `payments` creada
- [ ] Tabla `reviews` creada
- [ ] Tabla `notifications` creada
- [ ] Indices creados: `\di`
- [ ] Vistas creadas: `\dv`

### Insertar Datos de Prueba
- [ ] Ejecutar INSERT de especialidades
- [ ] Verificar: `SELECT * FROM specialties;`
- [ ] Datos cargados correctamente

---

## 🖥️ FASE 2: BACKEND (.NET)

### Crear Solución y Proyectos
- [ ] Crear carpeta `backend` si no existe
- [ ] Ejecutar: `dotnet new sln -n ProServi.Api`
- [ ] Crear 4 proyectos:
  - [ ] `ProServi.Domain` (classlib)
  - [ ] `ProServi.Application` (classlib)
  - [ ] `ProServi.Infrastructure` (classlib)
  - [ ] `ProServi.Api` (webapi)
- [ ] Agregar proyectos a solución

### Configurar Referencias de Proyectos
- [ ] ProServi.Api → referencia a Application, Infrastructure
- [ ] ProServi.Application → referencia a Domain
- [ ] ProServi.Infrastructure → referencia a Domain

### Instalar NuGet Packages
- [ ] Npgsql.EntityFrameworkCore.PostgreSQL
- [ ] System.IdentityModel.Tokens.Jwt
- [ ] BCrypt.Net-Next
- [ ] AutoMapper.Extensions.Microsoft.DependencyInjection
- [ ] Microsoft.EntityFrameworkCore.Tools

### Crear Carpeta Estructura
- [ ] ProServi.Domain/Entities/
- [ ] ProServi.Domain/Enums/
- [ ] ProServi.Application/DTOs/
- [ ] ProServi.Application/Services/
- [ ] ProServi.Application/Validators/
- [ ] ProServi.Infrastructure/Data/
- [ ] ProServi.Infrastructure/Repositories/
- [ ] ProServi.Api/Controllers/
- [ ] ProServi.Api/Middleware/

### Crear Entidades (Domain)
- [ ] User.cs con propiedades básicas
- [ ] UserRole.cs enum
- [ ] UserStatus.cs enum
- [ ] Customer.cs
- [ ] Professional.cs
- [ ] Specialty.cs
- [ ] ContactRequest.cs
- [ ] Budget.cs
- [ ] Project.cs
- [ ] Payment.cs
- [ ] Review.cs

### Crear DTOs (Application)
- [ ] LoginRequest.cs
- [ ] AuthResponse.cs
- [ ] UserData.cs
- [ ] RegisterCustomerRequest.cs
- [ ] RegisterProfessionalRequest.cs
- [ ] ProfessionalSearchResponse.cs

### Crear Services (Application)
- [ ] IAuthService.cs interface
- [ ] AuthService.cs implementación
- [ ] Métodos: LoginAsync, RegisterCustomerAsync, RegisterProfessionalAsync

### Crear DbContext (Infrastructure)
- [ ] ProServiDbContext.cs
- [ ] DbSet<> para todas las entidades
- [ ] OnModelCreating() con configuraciones
- [ ] SeedData() con datos de prueba

### Crear Controllers (Api)
- [ ] AuthController.cs con endpoints:
  - [ ] POST /api/auth/login
  - [ ] POST /api/auth/register/customer
  - [ ] POST /api/auth/register/professional

### Configurar Program.cs
- [ ] AddDbContext para PostgreSQL
- [ ] AddAuthentication con JWT
- [ ] AddSwaggerGen
- [ ] AddCors (AllowAll para dev)
- [ ] AddAutoMapper
- [ ] app.UseAuthentication()
- [ ] app.UseAuthorization()
- [ ] app.UseSwagger() y UseSwaggerUI()

### Configurar appsettings.json
- [ ] ConnectionString a `proservi_db`
- [ ] JWT:SecretKey (32+ caracteres)
- [ ] JWT:ExpirationHours
- [ ] Logging settings

### Crear y Ejecutar Migraciones
- [ ] `dotnet ef migrations add InitialCreate`
- [ ] `dotnet ef database update`
- [ ] Verificar que las tablas se crearon

### Pruebas Básicas
- [ ] `dotnet run` para iniciar servidor
- [ ] Acceder a: https://localhost:5001/swagger
- [ ] Probar endpoint POST /api/auth/register/customer
- [ ] Probar endpoint POST /api/auth/login
- [ ] Verificar que retorna JWT token

### Detener Servidor
- [ ] Presionar Ctrl+C en terminal
- [ ] Backend listo para siguiente fase

---

## 🅰️ FASE 3: FRONTEND (ANGULAR)

### Crear Proyecto Angular
- [ ] Navegar a carpeta `frontend`
- [ ] `ng new . --routing --style=css --skip-git=true`
- [ ] Esperar a que npm instale dependencias
- [ ] Verificar que se creó `package.json`

### Instalar Tailwind CSS
- [ ] `npm install -D tailwindcss postcss autoprefixer`
- [ ] `npx tailwindcss init -p`
- [ ] Verificar `tailwind.config.js` creado
- [ ] Verificar `postcss.config.js` creado

### Configurar Tailwind
- [ ] Actualizar `tailwind.config.js` con paths de templates
- [ ] Actualizar `src/styles.css` con @tailwind directives
- [ ] Crear custom Tailwind config con colores

### Instalar Dependencias Adicionales
- [ ] `npm install axios`
- [ ] `npm install jwt-decode`
- [ ] `npm install @ngx-translate/core` (opcional para futuros idiomas)

### Crear Estructura de Carpetas
- [ ] src/app/core/guards/
- [ ] src/app/core/interceptors/
- [ ] src/app/core/services/
- [ ] src/app/modules/auth/components/{login,register,role-selector}
- [ ] src/app/modules/home/components/{dashboard,navbar}
- [ ] src/app/modules/shared/components/
- [ ] src/assets/{images,icons,logo}

### Crear Servicios (Core)
- [ ] auth.service.ts con métodos:
  - [ ] login()
  - [ ] registerCustomer()
  - [ ] registerProfessional()
  - [ ] logout()
  - [ ] isLoggedIn()
  - [ ] getCurrentUser()

### Crear Guards (Core)
- [ ] auth.guard.ts (solo usuarios autenticados)
- [ ] not-auth.guard.ts (solo usuarios NO autenticados)

### Crear Interceptors (Core)
- [ ] auth.interceptor.ts (agrega JWT a headers)

### Crear Módulos
- [ ] auth.module.ts
- [ ] auth-routing.module.ts
- [ ] home.module.ts
- [ ] home-routing.module.ts

### Crear Componentes - Auth Module
- [ ] role-selector.component.ts
  - [ ] Template con 2 opciones (Cliente/Profesional)
  - [ ] Styling Tailwind
  - [ ] Navigate a register/customer o register/professional
- [ ] login.component.ts
  - [ ] Formulario con email y password
  - [ ] Validaciones
  - [ ] Llamar a authService.login()
- [ ] register-customer.component.ts
  - [ ] Formulario con nombre, email, phone, ciudad, password
  - [ ] Validaciones
  - [ ] Llamar a authService.registerCustomer()
- [ ] register-professional.component.ts
  - [ ] Formulario extendido con especialidad, años experiencia, etc
  - [ ] Llamar a authService.registerProfessional()

### Crear Componentes - Home Module
- [ ] dashboard.component.ts (pantalla principal)
- [ ] navbar.component.ts (navegación)

### Configurar Rutas
- [ ] app-routing.module.ts con rutas principales
- [ ] Rutas auth protegidas con NotAuthGuard
- [ ] Rutas home protegidas con AuthGuard
- [ ] Wildcard redirige a /auth/role-selector

### Configurar App Component
- [ ] app.component.ts con AuthService inyectado
- [ ] app.component.html con <router-outlet>

### Configurar App Module
- [ ] Importar HttpClientModule
- [ ] Registrar AuthInterceptor
- [ ] Importar AppRoutingModule

### Pruebas Básicas
- [ ] `ng serve --open`
- [ ] Verificar que carga en http://localhost:4200
- [ ] Navega a /auth/role-selector
- [ ] Verifica que es visible la página de bienvenida

---

## 🔗 FASE 4: INTEGRACIÓN FRONTEND-BACKEND

### Configurar URL de API
- [ ] En AuthService: `apiUrl = 'https://localhost:5001/api'`
- [ ] Verificar que backend está corriendo

### Prueba de Autenticación Completa
- [ ] Abrir ngroke o usar CORS headers
- [ ] Click en "Ofrecer Servicios"
- [ ] Llenar formulario de registro (profesional)
- [ ] Submit
- [ ] Verificar que:
  - [ ] Backend retorna token
  - [ ] Frontend guarda token en localStorage
  - [ ] Redirige a /home
  - [ ] Dashboard muestra datos del usuario

### Prueba de Login
- [ ] Logout
- [ ] Ir a /auth/login
- [ ] Ingresar credenciales
- [ ] Submit
- [ ] Verificar que:
  - [ ] Retorna token
  - [ ] Redirige a /home
  - [ ] Data del usuario es correcta

### Prueba de Seguridad
- [ ] Intentar acceder a /home sin token
- [ ] Debe redirigir a /auth/role-selector
- [ ] Intentar acceder a /auth/login con token
- [ ] Debe redirigir a /home

### Verificar en Swagger
- [ ] Abrir https://localhost:5001/swagger
- [ ] Probar todos los endpoints implementados
- [ ] Guardar token del primer login

---

## 📱 FASE 5: BÚSQUEDA DE PROFESIONALES (Semana 2)

### Backend - Crear Endpoint Búsqueda
- [ ] GET /api/professionals/search
- [ ] Query params: specialtyId, city, latitude, longitude, radiusKm, page, pageSize
- [ ] Retorna PaginatedResponse con lista de profesionales
- [ ] Agregar a Swagger

### Backend - Crear Endpoint Detalles
- [ ] GET /api/professionals/{id}
- [ ] Retorna detalles completos del profesional
- [ ] Incluye reviews
- [ ] Incluye certificaciones

### Backend - Crear Servicio de Búsqueda
- [ ] IProfessionalService interface
- [ ] ProfessionalService.cs con SearchAsync(), GetByIdAsync()
- [ ] Lógica de filtrado por especialidad y ciudad

### Frontend - Crear Componente SearchBar
- [ ] Select de especialidades (GET /api/specialties)
- [ ] Input de ciudad
- [ ] Sliders para distancia (opcional)
- [ ] Botón buscar

### Frontend - Crear Componente ProfessionalCard
- [ ] Card reutilizable para mostrar profesional
- [ ] Nombre, especialidad, rating
- [ ] Botones: Ver detalles, Contactar

### Frontend - Crear Componente SearchResults
- [ ] Mostrar lista de ProfessionalCard
- [ ] Paginación
- [ ] Integrar con backend

### Frontend - Crear Componente ProfessionalDetail
- [ ] Modal o página completa
- [ ] Información detallada
- [ ] Reviews
- [ ] Botón "Solicitar Contacto"

### Pruebas
- [ ] Buscar profesionales por especialidad
- [ ] Filtrar por ciudad
- [ ] Ver detalles de profesional
- [ ] Paginación funciona

---

## 📞 FASE 6: SISTEMA DE CONTACTO (Semana 3)

### Backend - Crear Endpoint Contacto
- [ ] POST /api/contact-requests
- [ ] Body: professionalId, method, description, preferredDateTime, address
- [ ] Retorna CreatedAtAction con 201
- [ ] Crear notificación para profesional

### Backend - Crear Endpoint Ver Solicitudes
- [ ] GET /api/contact-requests
- [ ] Query: status, page, pageSize
- [ ] Retorna solicitudes del usuario (según rol)

### Backend - Crear Endpoint Responder
- [ ] PUT /api/contact-requests/{id}
- [ ] Body: status (ACCEPTED/REJECTED)
- [ ] Crear notificación para cliente

### Backend - Crear Servicio de Contacto
- [ ] IContactRequestService interface
- [ ] ContactRequestService.cs

### Frontend - Crear Componente ContactForm
- [ ] Formulario con método de contacto (radio buttons)
- [ ] Textarea para descripción
- [ ] DateTime picker para fecha preferida
- [ ] Input para dirección (si es visita)
- [ ] Botón enviar

### Frontend - Crear Componente RequestList
- [ ] Lista de solicitudes creadas (para cliente)
- [ ] Lista de solicitudes recibidas (para profesional)
- [ ] Botones de acción (aceptar, rechazar)
- [ ] Mostrar estado de cada solicitud

### Frontend - Crear Servicio
- [ ] ContactRequestService con métodos HTTP

### Pruebas
- [ ] Crear solicitud de contacto
- [ ] Profesional recibe notificación
- [ ] Profesional ve solicitud en su dashboard
- [ ] Profesional acepta
- [ ] Cliente recibe notificación

---

## 💳 FASE 7: SISTEMA DE PAGOS (Semana 4)

### Backend - Crear Endpoint Pago
- [ ] POST /api/payments/create
- [ ] Body: contactRequestId, amount, description, paymentMethod
- [ ] Crear registro en tabla payments
- [ ] Escrow status: PENDING

### Backend - Crear Endpoint Listar Pagos
- [ ] GET /api/payments
- [ ] GET /api/payments/{id}

### Backend - Crear Lógica de Escrow
- [ ] Cuando pago se crea: dinero PENDING
- [ ] Cuando proyecto se marca COMPLETED: dinero RELEASED
- [ ] Guardar fecha de liberación

### Frontend - Crear Componente PaymentForm
- [ ] Mostrar presupuesto
- [ ] Método de pago (selector)
- [ ] Confirmación
- [ ] Integración (sin Stripe por ahora, solo mock)

### Frontend - Ver Pagos
- [ ] Dashboard muestra pagos pendientes
- [ ] Dashboard muestra pagos completados
- [ ] Mostrar estado de escrow

### Pruebas
- [ ] Crear presupuesto
- [ ] Cliente ve presupuesto
- [ ] Cliente "paga" (mock)
- [ ] Dinero en escrow
- [ ] Cliente marca como completado
- [ ] Dinero se libera

---

## ⭐ FASE 8: RESEÑAS Y RATINGS

### Backend - Crear Endpoint Reseña
- [ ] POST /api/reviews
- [ ] Body: projectId, rating, comment, wouldRecommend
- [ ] Actualizar rating promedio del profesional

### Backend - Crear Endpoint Listar Reviews
- [ ] GET /api/professionals/{id}/reviews

### Frontend - Crear Componente ReviewForm
- [ ] Star rating (1-5)
- [ ] Textarea para comentario
- [ ] Checkbox "recommend"
- [ ] Botón submit

### Frontend - Mostrar Reviews
- [ ] En detail del profesional
- [ ] Calificación promedio
- [ ] Lista de reviews

### Pruebas
- [ ] Dejar reseña después de completar trabajo
- [ ] Rating se actualiza
- [ ] Review aparece en perfil del profesional

---

## 📬 FASE 9: NOTIFICACIONES

### Backend - Crear Sistema de Notificaciones
- [ ] Guardar notificaciones en BD
- [ ] GET /api/notifications (lista)
- [ ] PUT /api/notifications/{id}/read (marcar como leída)

### Frontend - Mostrar Notificaciones
- [ ] Bell icon en navbar
- [ ] Mostrar lista de notificaciones no leídas
- [ ] Marcar como leída cuando se leen

### Email Integration (Futuro)
- [ ] [ ] Configurar SendGrid o similar
- [ ] [ ] Enviar email en cada evento

### WhatsApp Integration (Futuro)
- [ ] [ ] Configurar Twilio
- [ ] [ ] Enviar mensajes WhatsApp

---

## 🧪 FASE 10: TESTING

### Tests Unitarios Backend
- [ ] AuthService tests
- [ ] ProfessionalService tests
- [ ] ContactRequestService tests
- [ ] PaymentService tests

### Tests E2E Frontend
- [ ] Login flow
- [ ] Búsqueda flow
- [ ] Contacto flow
- [ ] Pago flow

### Tests de Integración
- [ ] Frontend → Backend endpoints
- [ ] Flujo completo de usuario

---

## 🚀 FASE 11: DEPLOYMENT

### Backend Deployment
- [ ] Publicar a servidor (IIS, Azure, AWS)
- [ ] Configurar appsettings.production.json
- [ ] Configurar HTTPS/SSL
- [ ] Backup de BD automático

### Frontend Deployment
- [ ] Build production: `ng build --configuration production`
- [ ] Publicar a hosting (Netlify, Vercel, Azure)
- [ ] Configurar custom domain
- [ ] Configurar redirects

### Documentación
- [ ] Actualizar URLs de producción
- [ ] Documentar proceso de deployment
- [ ] Crear guía de troubleshooting

---

## ✨ FASE 12: MEJORAS Y OPTIMIZACIONES

### Funcionalidades Adicionales
- [ ] Integración de pagos real (Stripe)
- [ ] Chat en tiempo real
- [ ] Geocodificación avanzada
- [ ] Sistema de favoritos
- [ ] Historial de búsqueda

### Performance
- [ ] Lazy loading de módulos
- [ ] Caching en frontend
- [ ] Optimizar queries de BD
- [ ] Compresión de assets

### Seguridad
- [ ] Verificación de identidad
- [ ] Verificación de profesionales
- [ ] Rate limiting
- [ ] GDPR compliance

---

## 📊 RESUMEN DE PROGRESO

| Fase | Descripción | Semana | Estado |
|------|-------------|--------|--------|
| 0 | Preparación | - | ⏳ |
| 1 | Base de Datos | 1 | ⏳ |
| 2 | Backend Setup | 1 | ⏳ |
| 3 | Frontend Setup | 1 | ⏳ |
| 4 | Integración | 1 | ⏳ |
| 5 | Búsqueda | 2 | ⏳ |
| 6 | Contacto | 3 | ⏳ |
| 7 | Pagos | 4 | ⏳ |
| 8 | Reseñas | 4 | ⏳ |
| 9 | Notificaciones | 4 | ⏳ |
| 10 | Testing | 5 | ⏳ |
| 11 | Deployment | 6 | ⏳ |
| 12 | Optimizaciones | 7+ | ⏳ |

---

## 💡 NOTAS IMPORTANTES

1. **Orden**: Seguir el orden de fases. No saltar pasos.
2. **Testing**: Probar después de cada función importante
3. **Documentación**: Mantener docs actualizadas
4. **Git**: Hacer commits después de cada fase importante
5. **Backups**: Respaldar BD regularmente

---

**Checklist v1.0**
**Creado**: 28/03/2026
**Úsalo para rastrear tu progreso**
