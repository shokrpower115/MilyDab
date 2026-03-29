# 🏢 ProServi - Marketplace de Profesionales Locales

## 📖 Descripción

**ProServi** es un marketplace digital que conecta usuarios locales con profesionales especializados en sus ciudades.

- ✅ **Clientes**: Buscan y contactan profesionales (fontaneros, electricistas, abogados, etc.)
- ✅ **Profesionales**: Ofrecen servicios por especialidad
- ✅ **Pagos Seguros**: Sistema de escrow para proteger ambas partes
- ✅ **Reseñas Verificadas**: Rating basado en experiencias reales

---

## 🚀 INICIO RÁPIDO

### 1️⃣ Ejecutar SQL Script
```powershell
psql -U postgres -d proservi_db -f database/migrations/001_create_tables.sql
```

### 2️⃣ Setup Backend (.NET)
```bash
# Seguir: Documentacion/SETUP_DOTNET.md
cd backend
dotnet build
dotnet run
```

### 3️⃣ Setup Frontend (Angular)
```bash
# Seguir: Documentacion/SETUP_ANGULAR.md
cd frontend
npm install
ng serve
```

---

## 📚 DOCUMENTACIÓN

### 🎯 Comienza aquí
- **[GUIA_MAESTRA.md](Documentacion/GUIA_MAESTRA.md)** ⭐ Guía de inicio rápido
- **[INDICE.md](Documentacion/INDICE.md)** - Índice completo de documentación
- **[RESUMEN_EJECUTIVO.md](Documentacion/RESUMEN_EJECUTIVO.md)** - Resumen del proyecto

### 📋 Especificaciones
- **[FlujoExito.txt](Documentacion/FlujoExito.txt)** - Flujo completo del usuario (7 fases)
- **[API_CONTRACTS.md](Documentacion/API_CONTRACTS.md)** - Especificación de endpoints
- **[ANALISIS_BASE_DATOS.md](Documentacion/ANALISIS_BASE_DATOS.md)** - Diseño de base de datos

### 🛠️ Setup & Implementación
- **[SETUP_DOTNET.md](Documentacion/SETUP_DOTNET.md)** - Crear proyecto .NET
- **[SETUP_ANGULAR.md](Documentacion/SETUP_ANGULAR.md)** - Crear proyecto Angular
- **[REFERENCIA_RAPIDA.md](Documentacion/REFERENCIA_RAPIDA.md)** - Comandos y snippets

### 💼 Diseño & Negocio
- **[DASHBOARD_PROYECTO.md](Documentacion/DASHBOARD_PROYECTO.md)** - Visión general del proyecto
- **[SISTEMA_NOTIFICACIONES.md](Documentacion/SISTEMA_NOTIFICACIONES.md)** - Estrategia de notificaciones
- **[RESUMEN_PAGOS_Y_DISPUTAS.md](Documentacion/RESUMEN_PAGOS_Y_DISPUTAS.md)** - Sistema de pagos

---

## 🏗️ Estructura del Proyecto

```
ProServi/
├── 📁 Documentacion/          # Toda la especificación y guías
│   ├── GUIA_MAESTRA.md       # ⭐ Lee primero
│   ├── INDICE.md             # Índice completo
│   ├── FlujoExito.txt        # Flujo del usuario
│   ├── API_CONTRACTS.md      # Contratos API
│   ├── SETUP_DOTNET.md       # Setup backend
│   ├── SETUP_ANGULAR.md      # Setup frontend
│   └── ... (10 docs más)
│
├── 📁 database/
│   ├── 📁 migrations/
│   │   └── 001_create_tables.sql  # ⭐ Script SQL completo
│   └── 📁 seeds/
│
├── 📁 backend/               # Proyecto .NET (pendiente crear)
│   └── src/
│       ├── ProServi.Domain/
│       ├── ProServi.Application/
│       ├── ProServi.Infrastructure/
│       └── ProServi.Api/
│
├── 📁 frontend/              # Proyecto Angular (pendiente crear)
│   └── src/
│       └── app/
│           ├── core/
│           ├── modules/
│           └── shared/
│
└── 📄 README_PROSERVI.md    # ← TÚ ESTÁS AQUÍ
```

---

## 💻 Tech Stack

### Backend
- **Framework**: ASP.NET Core 8
- **Language**: C#
- **Architecture**: Clean Architecture (4 proyectos)
- **ORM**: Entity Framework Core
- **Auth**: JWT + Bcrypt
- **Database**: PostgreSQL 9.2+

### Frontend
- **Framework**: Angular 15+
- **Styling**: Tailwind CSS
- **Language**: TypeScript
- **State**: RxJS Observables

### Database
- **Engine**: PostgreSQL 9.2+
- **Tables**: 16
- **Stored Procedures**: Triggers para auditoría
- **Views**: 6 para reporting

---

## ✨ Características MVP (Fase 1)

### ✅ Autenticación (Semana 1)
- Registro de Clientes y Profesionales
- Login con JWT
- Perfil de usuario
- Dashboard

### ✅ Búsqueda (Semana 2)
- Buscar por especialidad y ciudad
- Ver lista de profesionales
- Ver detalles y ratings

### ✅ Contacto (Semana 3)
- Crear solicitud de contacto
- Notificaciones
- Aceptar/rechazar

### ✅ Pagos (Semana 4)
- Presupuesto
- Pago seguro (escrow)
- Reseñas y ratings

---

## 🔒 Seguridad

- ✅ JWT tokens para autenticación
- ✅ Bcrypt para contraseñas
- ✅ HTTPS/TLS en producción
- ✅ CORS configurado
- ✅ Input validation
- ✅ SQL injection prevention

---

## 📊 Estado del Proyecto

```
Status: 🟢 LISTO PARA DESARROLLO

Completado:
✅ 15,000+ líneas de documentación
✅ Especificación completa
✅ Diseño de BD (16 tablas)
✅ Contratos API (12 endpoints)
✅ Scripts SQL
✅ Guías de setup

Próximo Paso:
⏳ Crear proyectos y comenzar implementación
```

---

## 🚀 Primeros Pasos

### 1. Leer Documentación
- [ ] Lee [GUIA_MAESTRA.md](Documentacion/GUIA_MAESTRA.md) (10 minutos)
- [ ] Lee [DASHBOARD_PROYECTO.md](Documentacion/DASHBOARD_PROYECTO.md) (15 minutos)
- [ ] Revisa [API_CONTRACTS.md](Documentacion/API_CONTRACTS.md) (15 minutos)

### 2. Ejecutar SQL Script
- [ ] Abre PostgreSQL
- [ ] Ejecuta `001_create_tables.sql`
- [ ] Verifica que se crearon 16 tablas

### 3. Crear Proyectos
- [ ] Sigue [SETUP_DOTNET.md](Documentacion/SETUP_DOTNET.md) para backend
- [ ] Sigue [SETUP_ANGULAR.md](Documentacion/SETUP_ANGULAR.md) para frontend
- [ ] Prueba que ambos inician correctamente

### 4. Integración Básica
- [ ] Prueba endpoint POST /api/auth/login
- [ ] Prueba componente RoleSelector
- [ ] Verifica flujo login → dashboard

---

## 📞 Recursos

### Documentación de Tecnologías
- [Angular Docs](https://angular.io/)
- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core/)
- [PostgreSQL Docs](https://www.postgresql.org/docs/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Tailwind CSS](https://tailwindcss.com/)

### Herramientas Recomendadas
- Visual Studio Code (IDE)
- PostgreSQL (BD)
- Postman (API testing)
- Git (Version control)

---

## 📋 Checklist Pre-Desarrollo

- [ ] PostgreSQL 9.2+ instalado
- [ ] Node.js 18+ instalado
- [ ] .NET 8 SDK instalado
- [ ] VS Code instalado
- [ ] Leído GUIA_MAESTRA.md
- [ ] SQL script ejecutado
- [ ] Ports disponibles (5432, 5001, 4200)
- [ ] Claro el flujo del usuario
- [ ] Claro el contrato API

---

## 🤝 Contribución

Este proyecto está en fase de desarrollo inicial. Para cambios:

1. Actualiza la documentación relevante primero
2. Sigue Clean Architecture patterns
3. Mantén código documentado
4. Escribe tests para nuevas features

---

## 📅 Timeline

| Semana | Hito | Estado |
|--------|------|--------|
| 1 | Setup BD, Backend, Frontend | ⏳ Por comenzar |
| 2 | Búsqueda de profesionales | ⏳ Por comenzar |
| 3 | Sistema de contacto | ⏳ Por comenzar |
| 4 | Pagos y reseñas | ⏳ Por comenzar |
| 5+ | Testing, deploy, mejoras | ⏳ Por comenzar |

---

## 📝 Licencia

Proyecto privado para uso interno.

---

## 📧 Contacto

Para preguntas sobre documentación o arquitectura, revisar los archivos .md en la carpeta `Documentacion/`.

---

**ProServi v1.0**
**Listo para desarrollo**
**Creado**: 28/03/2026
